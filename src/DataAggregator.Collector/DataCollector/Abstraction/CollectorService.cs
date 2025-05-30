using DataAggregator.Collector.DataCollector.Abstraction.Configuration;
using DataAggregator.Collector.DataCollector.DataStorage;
using DataAggregator.Collector.DataCollector.LocalStorage;
using DataAggregator.Collector.DataCollector.Models;
using DataAggregator.Collector.DataCollector.Registration;
using DataAggregator.Shared.Configuration.TimeSeries;
using Serilog;

namespace DataAggregator.Collector.DataCollector.Abstraction;

// TODO CJS -> Read and clean

/// <summary>
/// Abstract base class for collector services.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="CollectorService"/> class.
/// </remarks>
/// <param name="dataSourceConnector">The data source connector.</param>
/// <param name="dataRepository">The data repository.</param>
/// <param name="initializationService">The collector initialization service.</param>
/// <param name="dataBufferService">The data buffer service.</param>
/// <param name="configuration">The collector configuration.</param>
public abstract class CollectorService(
    IDataSourceConnector dataSourceConnector,
    IDataRepository dataRepository,
    CollectorInitializationService initializationService,
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
            // Initialize the collector by registering with the central service
            await initializationService.InitializeAsync();

            // Initialize the data repository with the endpoint configuration
            await dataRepository.InitializeAsync();

            // Connect to the data source
            InfluxEndpoint influxConfig = initializationService.GetInfluxConfig();
            await dataSourceConnector.ConnectAsync(influxConfig.Endpoint);

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
    private async Task<bool> ProcessDataAsync(IEnumerable<IMeasurementData> data)
    {
        await _processingLock.WaitAsync();
        try
        {
            Log.Debug("Processing {Count} data points", data.Count());

            // Group data by type for type-specific processing
            Dictionary<Type, List<IMeasurementData>> groupedData = GroupDataByType(data);
            bool overallSuccess = true;

            // Process each type of data
            foreach (KeyValuePair<Type, List<IMeasurementData>> group in groupedData)
            {
                bool success = await ProcessDataGroupAsync(group.Key, group.Value);
                overallSuccess = overallSuccess && success;
            }

            if (overallSuccess)
            {
                LastDataSent = DateTime.UtcNow;
            }

            return overallSuccess;
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
    /// Groups data by their type for type-specific processing.
    /// </summary>
    /// <param name="data">The data to group.</param>
    /// <returns>A dictionary of type to list of data.</returns>
    private Dictionary<Type, List<IMeasurementData>> GroupDataByType(IEnumerable<IMeasurementData> data)
    {
        var result = new Dictionary<Type, List<IMeasurementData>>();

        foreach (IMeasurementData item in data)
        {
            if (!result.TryGetValue(item.ValueType, out List<IMeasurementData>? list))
            {
                list = [];
                result[item.ValueType] = list;
            }

            list.Add(item);
        }

        return result;
    }

    /// <summary>
    /// Processes a group of data of the same type.
    /// </summary>
    /// <param name="type">The type of the data.</param>
    /// <param name="data">The data to process.</param>
    /// <returns>A boolean indicating whether the processing was successful.</returns>
    private async Task<bool> ProcessDataGroupAsync(Type type, List<IMeasurementData> data)
    {
        if (!data.Any())
            return true;

        // Use reflection to call the generic method with the correct type
        System.Reflection.MethodInfo? method = typeof(CollectorService).GetMethod(
            nameof(ProcessTypedDataAsync),
            System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);

        System.Reflection.MethodInfo? genericMethod = method?.MakeGenericMethod(type);

        if (genericMethod == null)
        {
            Log.Error("Failed to create generic method for type {Type}", type.Name);
            return false;
        }

        try
        {
            // Invoke the generic method
            return await (Task<bool>)genericMethod.Invoke(this, new object[] { data })!;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error processing data of type {Type}", type.Name);
            return false;
        }
    }

    /// <summary>
    /// Processes data of a specific type.
    /// </summary>
    /// <typeparam name="T">The type of the data value.</typeparam>
    /// <param name="data">The data to process.</param>
    /// <returns>A boolean indicating whether the processing was successful.</returns>
    private async Task<bool> ProcessTypedDataAsync<T>(List<IMeasurementData> data)
    {
        // Convert the data to the correct generic type
        var typedData = data.Cast<MeasurementData<T>>().ToList();

        try
        {
            // Try to insert the data
            bool success = await dataRepository.BulkInsertAsync(typedData, configuration);

            if (!success)
            {
                // Buffer the data if insertion failed
                dataBufferService.AddRange(typedData);
                return false;
            }

            return true;
        }
        catch (Exception ex)
        {
            Log.Error(
                ex,
                "Error inserting data of type {Type}, buffering {Count} measurements",
                typeof(T).Name,
                data.Count);

            // Buffer the data if insertion fails
            dataBufferService.AddRange(data);
            return false;
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
}
