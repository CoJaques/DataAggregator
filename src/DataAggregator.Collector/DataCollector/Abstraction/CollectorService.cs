using DataAggregator.Collector.DataCollector.Abstraction.Configuration;
using DataAggregator.Collector.DataCollector.DataStorage;
using DataAggregator.Collector.DataCollector.LocalStorage;
using DataAggregator.Collector.DataCollector.Models;
using DataAggregator.Collector.DataCollector.Registration;
using Serilog;

namespace DataAggregator.Collector.DataCollector.Abstraction;

/// <summary>
/// Abstract base class for collector services.
/// </summary>
public abstract class CollectorService
{
    private readonly IDataSourceConnector _dataSourceConnector;
    private readonly IDataRepository _dataRepository;
    private readonly CollectorInitializationService _initializationService;
    private readonly DataBufferService _dataBufferService;
    private readonly CollectorConfiguration _configuration;
    
    private bool _isRunning;
    private CancellationTokenSource? _cancellationTokenSource;
    private readonly SemaphoreSlim _processingLock = new(1, 1);

    /// <summary>
    /// Gets the timestamp of the last data sent successfully.
    /// </summary>
    public DateTime LastDataSent { get; private set; } = DateTime.MinValue;

    /// <summary>
    /// Initializes a new instance of the <see cref="CollectorService"/> class.
    /// </summary>
    /// <param name="dataSourceConnector">The data source connector.</param>
    /// <param name="dataRepository">The data repository.</param>
    /// <param name="initializationService">The collector initialization service.</param>
    /// <param name="dataBufferService">The data buffer service.</param>
    /// <param name="configuration">The collector configuration.</param>
    protected CollectorService(
        IDataSourceConnector dataSourceConnector,
        IDataRepository dataRepository,
        CollectorInitializationService initializationService,
        DataBufferService dataBufferService,
        CollectorConfiguration configuration)
    {
        _dataSourceConnector = dataSourceConnector;
        _dataRepository = dataRepository;
        _initializationService = initializationService;
        _dataBufferService = dataBufferService;
        _configuration = configuration;
    }

    /// <summary>
    /// Starts the collector service asynchronously.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task StartAsync()
    {
        if (_isRunning)
        {
            Log.Debug("Collector service is already running for device {DeviceName}", _configuration.DeviceName);
            return;
        }

        Log.Information("Starting collector service for device {DeviceName}", _configuration.DeviceName);
        
        try
        {
            // Initialize the collector by registering with the central service
            await _initializationService.InitializeAsync();
            
            // Initialize the data repository with the endpoint configuration
            await _dataRepository.InitializeAsync();
            
            // Connect to the data source
            var influxConfig = _initializationService.GetInfluxConfig();
            await _dataSourceConnector.ConnectAsync(influxConfig.Endpoint);
            
            _cancellationTokenSource = new CancellationTokenSource();
            _isRunning = true;
            
            // Process any data in the buffer first
            await ProcessBufferedDataAsync();
            
            // Start the data collection loop
            _ = Task.Run(DataCollectionLoopAsync);
            
            Log.Information("Collector service started for device {DeviceName}", _configuration.DeviceName);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Failed to start collector service for device {DeviceName}", _configuration.DeviceName);
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
            Log.Debug("Collector service is not running for device {DeviceName}", _configuration.DeviceName);
            return;
        }

        Log.Information("Stopping collector service for device {DeviceName}", _configuration.DeviceName);
        
        _isRunning = false;
        _cancellationTokenSource?.Cancel();
        
        try
        {
            // Disconnect from the data source
            await _dataSourceConnector.DisconnectAsync();
            
            // Process any remaining data in the buffer
            await ProcessBufferedDataAsync();
            
            Log.Information("Collector service stopped for device {DeviceName}", _configuration.DeviceName);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error while stopping collector service for device {DeviceName}", _configuration.DeviceName);
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
                    var data = await _dataSourceConnector.FetchDataAsync();
                    
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
            var groupedData = GroupDataByType(data);
            bool overallSuccess = true;
            
            // Process each type of data
            foreach (var group in groupedData)
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
        
        foreach (var item in data)
        {
            if (!result.TryGetValue(item.ValueType, out var list))
            {
                list = new List<IMeasurementData>();
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
        var method = typeof(CollectorService).GetMethod(nameof(ProcessTypedDataAsync), 
            System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
        
        var genericMethod = method?.MakeGenericMethod(type);
        
        if (genericMethod == null)
        {
            Log.Error("Failed to create generic method for type {Type}", type.Name);
            return false;
        }
        
        try
        {
            // Invoke the generic method
            return (bool)(await (Task<bool>)genericMethod.Invoke(this, new object[] { data })!);
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
            bool success = await _dataRepository.BulkInsertAsync(typedData);
            
            if (!success)
            {
                // Buffer the data if insertion failed
                int buffered = _dataBufferService.AddRange(typedData);
                Log.Warning("Buffered {Count} measurements of type {Type}", buffered, typeof(T).Name);
                return false;
            }
            
            return true;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error inserting data of type {Type}, buffering {Count} measurements", 
                typeof(T).Name, data.Count);
            
            // Buffer the data if insertion fails
            _dataBufferService.AddRange(data);
            return false;
        }
    }
    
    /// <summary>
    /// Processes any data stored in the buffer.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    private async Task ProcessBufferedDataAsync()
    {
        var bufferedData = _dataBufferService.GetAndClearBuffer();
        if (bufferedData.Any())
        {
            Log.Information("Processing {Count} items from buffer", bufferedData.Count());
            await ProcessDataAsync(bufferedData);
        }
    }
}
