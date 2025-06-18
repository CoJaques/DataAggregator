using DataAggregator.Collector.Shared.Abstraction.Configuration;
using DataAggregator.Collector.Shared.DataStorage;
using DataAggregator.Collector.Shared.LocalStorage;
using DataAggregator.Collector.Shared.Models;
using Serilog;

namespace DataAggregator.Collector.Shared.Abstraction;

/// <summary>
/// Base class for collector services.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="CollectorService"/> class.
/// </remarks>
/// <param name="dataSourceConnector">The data source connector.</param>
/// <param name="dataRepository">The data repository.</param>
/// <param name="dataBufferService">The data buffer service.</param>
/// <param name="configuration">The collector configuration.</param>
public class CollectorService(
    IDataSourceConnector dataSourceConnector,
    IDataRepository dataRepository,
    DataBufferService dataBufferService,
    CollectorConfiguration configuration)
{
    private readonly SemaphoreSlim _processingLock = new(1, 1);

    private bool _isRunning;
    private CancellationTokenSource? _cancellationTokenSource;

    /// <summary>
    /// Gets the timestamp of the last data sent successfully.
    /// </summary>
    public DateTime LastDataSent { get; private set; } = DateTime.MinValue;

    /// <summary>
    /// Gets the size of the data buffer used for storing measurements before sending them to the repository.
    /// </summary>
    public int BufferSize => dataBufferService.GetBufferSize();

    /// <summary>
    /// Starts the collector service asynchronously.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task StartAsync()
    {
        if (_isRunning)
        {
            Log.Debug("Collector service is already running for device {DeviceName}", configuration.DeviceName);
            return;
        }

        Log.Information("Starting collector service for device {DeviceName}", configuration.DeviceName);

        try
        {
            await dataRepository.InitializeAsync();
            await dataSourceConnector.ConnectAsync();

            _cancellationTokenSource = new CancellationTokenSource();
            _isRunning = true;

            // Process any data in the buffer first
            await ProcessBufferedDataAsync();

            // Start the data collection loop
            _ = Task.Run(DataCollectionLoopAsync);

            Log.Information("Collector service started for device {DeviceName}", configuration.DeviceName);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Failed to start collector service for device {DeviceName}", configuration.DeviceName);
            _isRunning = false;
            _cancellationTokenSource?.Cancel();
            throw;
        }
    }

    /// <summary>
    /// Stops the collector service asynchronously.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task StopAsync()
    {
        if (!_isRunning)
        {
            Log.Debug("Collector service is not running for device {DeviceName}", configuration.DeviceName);
            return;
        }

        Log.Information("Stopping collector service for device {DeviceName}", configuration.DeviceName);

        _isRunning = false;
        _cancellationTokenSource?.Cancel();

        try
        {
            // Disconnect from the data source
            await dataSourceConnector.DisconnectAsync();

            // Process any remaining data in the buffer
            await ProcessBufferedDataAsync();

            Log.Information("Collector service stopped for device {DeviceName}", configuration.DeviceName);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error while stopping collector service for device {DeviceName}", configuration.DeviceName);
        }
    }

    /// <summary>
    /// Main loop for collecting and processing data.
    /// </summary>
    private async Task DataCollectionLoopAsync()
    {
        int consecutiveErrors = 0;

        try
        {
            while (_isRunning && _cancellationTokenSource != null && !_cancellationTokenSource.Token.IsCancellationRequested)
            {
                try
                {
                    // Fetch data from the source
                    IEnumerable<IMeasurementData> data = await dataSourceConnector.FetchDataAsync();

                    // Process the data
                    if (data.Any())
                    {
                        bool success = await ProcessDataAsync(data);

                        if (success)
                        {
                            consecutiveErrors = 0;
                        }
                    }

                    // Add a small delay before the next fetch
                    await Task.Delay(100, _cancellationTokenSource.Token);
                }
                catch (OperationCanceledException)
                {
                    // Expected when cancellation is requested
                    break;
                }
                catch (Exception ex)
                {
                    consecutiveErrors++;

                    Log.Error(ex, "Error in data collection loop ({ErrorCount} consecutive errors)", consecutiveErrors);

                    // Exponential backoff for consecutive errors
                    int delay = Math.Min(100 * (int)Math.Pow(2, consecutiveErrors), 30000);
                    await Task.Delay(delay, _cancellationTokenSource.Token);
                }
            }
        }
        catch (OperationCanceledException)
        {
            // Expected when cancellation is requested
            Log.Information("Data collection loop canceled");
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Unhandled error in data collection loop");
            _isRunning = false;
        }
    }

    /// <summary>
    /// Processes the collected data asynchronously.
    /// </summary>
    /// <param name="data">The data to process.</param>
    /// <returns>A boolean indicating whether the processing was successful.</returns>
    protected async Task<bool> ProcessDataAsync(IEnumerable<IMeasurementData> data)
    {
        await _processingLock.WaitAsync();
        try
        {
            Log.Debug("Processing {Count} data points", data.Count());
            bool sucess = await dataRepository.BulkInsertAsync(data, configuration);

            if (sucess)
            {
                LastDataSent = DateTime.UtcNow;
            }

            return sucess;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error processing data");
            return false;
        }
        finally
        {
            _processingLock.Release();
        }
    }

    /// <summary>
    /// Processes any data stored in the buffer.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    private async Task ProcessBufferedDataAsync()
    {
        IEnumerable<IMeasurementData> bufferedData = dataBufferService.GetAndClearBuffer();
        if (bufferedData.Any())
        {
            Log.Information("Processing {Count} items from buffer", bufferedData.Count());
            await ProcessDataAsync(bufferedData);
        }
    }

    /// <summary>
    /// Methods to check if the repository is connected to the data source asynchronously.
    /// </summary>
    /// <returns>True if connected, false otherwise.</returns>
    public async Task<bool> IsRepositoryConnected()
        => await dataRepository.IsConnectedAsync();
}
