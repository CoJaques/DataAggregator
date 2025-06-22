using System.Collections.Concurrent;
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
    #region Private Fields

    private readonly SemaphoreSlim _processingLock = new(1, 1);
    private readonly ConcurrentQueue<IMeasurementData> _dataQueue = new();
    private readonly TimeSpan _flushInterval = TimeSpan.FromSeconds(1); // Flush every 1 second

    private bool _isRunning;
    private CancellationTokenSource? _cancellationTokenSource;

    private DateTime _lastFlushTime = DateTime.UtcNow;
    #endregion

    #region Public Properties

    /// <summary>
    /// Gets the timestamp of the last data sent successfully.
    /// </summary>
    public DateTime LastDataSent { get; private set; } = DateTime.MinValue;

    /// <summary>
    /// Gets the size of the data buffer used for storing measurements before sending them to the repository.
    /// </summary>
    public int BufferSize => dataBufferService.GetBufferSize() + _dataQueue.Count;

    #endregion

    #region Public Methods

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

            await ProcessBufferedDataAsync();

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
            await dataSourceConnector.DisconnectAsync();
            await ProcessBufferedDataAsync();

            Log.Information("Collector service stopped for device {DeviceName}", configuration.DeviceName);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error while stopping collector service for device {DeviceName}", configuration.DeviceName);
        }
    }

    /// <summary>
    /// Methods to check if the repository is connected to the data source asynchronously.
    /// </summary>
    /// <returns>True if connected, false otherwise.</returns>
    public async Task<bool> IsRepositoryConnected()
        => await dataRepository.IsConnectedAsync();

    #endregion

    #region Private Methods

    private async Task DataCollectionLoopAsync()
    {
        int consecutiveErrors = 0;

        try
        {
            while (_isRunning && _cancellationTokenSource != null && !_cancellationTokenSource.Token.IsCancellationRequested)
            {
                try
                {
                    IEnumerable<IMeasurementData> fetchedData = await dataSourceConnector.FetchDataAsync();
                    foreach (IMeasurementData measurement in fetchedData)
                    {
                        _dataQueue.Enqueue(measurement);
                    }

                    // Check if it's time to flush
                    if (DateTime.UtcNow - _lastFlushTime >= _flushInterval)
                    {
                        _ = Task.Run(ProcessQueuedDataAsync);
                        _lastFlushTime = DateTime.UtcNow;
                    }

                    await Task.Delay(10, _cancellationTokenSource.Token); // very short delay to allow frequent fetches
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    consecutiveErrors++;
                    Log.Error(ex, "Error in data collection loop ({ErrorCount} consecutive errors)", consecutiveErrors);
                    int delay = Math.Min(100 * (int)Math.Pow(2, consecutiveErrors), 30000);
                    await Task.Delay(delay, _cancellationTokenSource.Token);
                }
            }
        }
        catch (OperationCanceledException)
        {
            Log.Information("Data collection loop canceled");
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Unhandled error in data collection loop");
            _isRunning = false;
        }
    }

    private async Task ProcessQueuedDataAsync()
    {
        if (_dataQueue.IsEmpty)
            return;

        await _processingLock.WaitAsync();
        try
        {
            var batch = new List<IMeasurementData>();

            while (_dataQueue.TryDequeue(out IMeasurementData? item))
            {
                batch.Add(item);
            }

            if (batch.Any())
            {
                Log.Debug("Flushing {Count} measurements to repository", batch.Count);
                bool success = await dataRepository.BulkInsertAsync(batch, configuration);

                if (success)
                {
                    LastDataSent = DateTime.UtcNow;
                }
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error processing queued data");
        }
        finally
        {
            _processingLock.Release();
        }
    }

    private async Task ProcessBufferedDataAsync()
    {
        if (_dataQueue.Count > 0)
        {
            Log.Information("Processing {Count} items from buffer", _dataQueue.Count);
            await ProcessQueuedDataAsync();
        }
    }

    #endregion
}
