using DataAggregator.Collector.DataCollector.Models;
using DataAggregator.Collector.DataCollector.Registration;
using Serilog;

namespace DataAggregator.Collector.DataCollector.DataStorage.Influx;

/// <summary>
/// Repository implementation for InfluxDB time series database.
/// </summary>
public class InfluxDbRepository : IDataRepository
{
    private readonly CollectorInitializationService _initializationService;
    private InfluxDbConfig _config;
    private bool _isConfigured;
    private readonly SemaphoreSlim _configLock = new(1, 1);

    // This would be replaced with actual InfluxDB client in the implementation
    private object? _client;

    /// <summary>
    /// Initializes a new instance of the <see cref="InfluxDbRepository"/> class.
    /// </summary>
    /// <param name="initializationService">The service for initialization and endpoint management.</param>
    public InfluxDbRepository(CollectorInitializationService initializationService)
    {
        _initializationService = initializationService;
        _initializationService.EndpointRenewed += HandleEndpointRenewal;
    }

    /// <inheritdoc/>
    public async Task InitializeAsync()
    {
        await _configLock.WaitAsync();
        try
        {
            if (_isConfigured)
            {
                return;
            }

            // Get the initial configuration from the initialization service
            _config = _initializationService.GetInfluxConfig();

            await InitializeClientAsync();
            _isConfigured = true;

            Log.Information("InfluxDB repository initialized with endpoint: {Endpoint}", _config.Endpoint);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Failed to initialize InfluxDB repository");
            throw;
        }
        finally
        {
            _configLock.Release();
        }
    }

    /// <inheritdoc/>
    public async Task<bool> BulkInsertAsync<T>(IEnumerable<MeasurementData<T>> data)
    {
        // Ensure we're initialized before attempting any operations
        if (!_isConfigured)
        {
            try
            {
                await InitializeAsync();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to initialize InfluxDB repository");
                return false;
            }
        }

        try
        {
            Log.Debug("Inserting {Count} measurements to InfluxDB at {Endpoint}", data.Count(), _config.Endpoint);
            return await TryBulkInsertWithRetryAsync(data);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error inserting measurements to InfluxDB at {Endpoint}", _config.Endpoint);
            return false;
        }
    }

    private async Task<bool> TryBulkInsertWithRetryAsync<T>(IEnumerable<MeasurementData<T>> data)
    {
        try
        {
            // First attempt
            return await BulkInsertInternalAsync(data);
        }
        catch (Exception ex) when (IsConnectionException(ex))
        {
            Log.Warning(ex, "Connection issue with InfluxDB at {Endpoint}, attempting to renew endpoint", _config.Endpoint);

            // Try to renew the endpoint
            if (await _initializationService.TryRenewEndpointAsync())
            {
                Log.Information("Endpoint renewed, retrying operation");

                // Second attempt after renewal
                try
                {
                    return await BulkInsertInternalAsync(data);
                }
                catch (Exception retryEx)
                {
                    Log.Error(retryEx, "Failed to insert data after endpoint renewal");
                    return false;
                }
            }

            Log.Error("Failed to renew endpoint, data insertion failed");
            return false;
        }
    }

    private async Task<bool> BulkInsertInternalAsync<T>(IEnumerable<MeasurementData<T>> data)
    {
        // TODO: Implement actual InfluxDB client logic
        // This is a placeholder implementation that will be replaced with actual InfluxDB client code

        Log.Debug("Writing {Count} points to InfluxDB", data.Count());

        foreach (var measurement in data)
        {
            // Convert measurement to InfluxDB point
            object point = ConvertToInfluxPoint(measurement);

            // Write point to InfluxDB
            // await _client.WritePointAsync(point, _config.Bucket, _config.Org);
        }

        // Simulate write operation delay
        await Task.Delay(10);

        return true;
    }

    private object ConvertToInfluxPoint<T>(MeasurementData<T> measurement)
    {
        // TODO: Implement conversion logic from MeasurementData to InfluxDB point
        // This is a placeholder implementation that will be replaced with actual conversion logic

        // Example structure for an InfluxDB data point:
        /*
        var point = PointData.Measurement("sensor_data")
            .Tag("sensor", measurement.SensorName)
            .Field("value", measurement.Value)
            .Timestamp(measurement.TimeStamp, WritePrecision.Ns);
        */

        return new object(); // Placeholder
    }

    private bool IsConnectionException(Exception ex)
    {
        // This would check if the exception is related to connection issues
        // For example, HttpRequestException, SocketException, etc.
        return ex is System.Net.Http.HttpRequestException ||
               ex is System.Net.Sockets.SocketException ||
               ex is System.TimeoutException ||
               ex.Message.Contains("connection", StringComparison.OrdinalIgnoreCase);
    }

    private async Task InitializeClientAsync()
    {
        // TODO: Implement actual InfluxDB client initialization
        // This is a placeholder implementation that will be replaced with actual client initialization

        // Example:
        // _client = new InfluxDBClient(_config.Endpoint, _config.Token);

        // Simulate initialization delay
        await Task.Delay(50);

        Log.Information("InfluxDB client initialized with endpoint: {Endpoint}", _config.Endpoint);
    }

    private void HandleEndpointRenewal(object? sender, InfluxDbConfig newConfig)
    {
        _configLock.Wait();
        try
        {
            _config = newConfig;

            // TODO: Dispose and reinitialize the client with the new configuration
            // if (_client is IDisposable disposable)
            // {
            //     disposable.Dispose();
            // }

            // Create a new client with the updated configuration
            // _client = new InfluxDBClient(_config.Endpoint, _config.Token);

            Log.Information("InfluxDB client updated with new endpoint: {Endpoint}", _config.Endpoint);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Failed to update InfluxDB client after endpoint renewal");
            _isConfigured = false; // Force reinitialization on next operation
        }
        finally
        {
            _configLock.Release();
        }
    }
}
