using System.Net.Sockets;
using DataAggregator.Collector.Shared.Abstraction.Configuration;
using DataAggregator.Collector.Shared.Models;
using DataAggregator.Collector.Shared.Registration;
using DataAggregator.Shared.Configuration.TimeSeries;
using InfluxDB3.Client;
using InfluxDB3.Client.Write;
using Serilog;

namespace DataAggregator.Collector.Shared.DataStorage.Influx;

/// <summary>
/// Implementation of a data repository for InfluxDB time series database.
/// </summary>
public class InfluxDbRepository : IDataRepository, IDisposable
{
    #region Private Fields
    private readonly CollectorEndpointManager _collectorEndpointManager;
    private readonly SemaphoreSlim _configLock = new(1, 1);
    private InfluxDBClient? _client;
    private InfluxEndpoint? _config;
    private bool _isConfigured;
    #endregion

    #region Constructor & Initialization

    /// <summary>
    /// Initializes a new instance of the <see cref="InfluxDbRepository"/> class.
    /// </summary>
    /// <param name="initializationService">The initialization service which must register the device and get
    /// repository endpoint informations.</param>
    public InfluxDbRepository(CollectorEndpointManager initializationService)
    {
        _collectorEndpointManager = initializationService;
        _collectorEndpointManager.EndpointRenewed += async (s, e) => await HandleEndpointRenewalAsync(e);
    }

    /// <summary>
    /// Intializes the InfluxDB repository by configuring the client.
    /// </summary>
    /// <returns>Task.</returns>
    public async Task InitializeAsync()
    {
        await _configLock.WaitAsync();
        try
        {
            if (_isConfigured)
                return;

            await _collectorEndpointManager.InitializeAsync();
            _config = _collectorEndpointManager.GetInfluxConfig();
            InitializeClient();
            _collectorEndpointManager.EndpointRenewed += async (s, e) => await HandleEndpointRenewalAsync(e);
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

    private void InitializeClient()
    {
        if (_config == null)
        {
            Log.Error("InfluxDB configuration is not set. Cannot initialize client.");
            throw new InvalidOperationException("InfluxDB configuration is not set.");
        }

        _client = new InfluxDBClient(_config.Endpoint, _config.Token);
        Log.Information("InfluxDB client initialized with endpoint: {Endpoint}", _config.Endpoint);
    }

    private async Task HandleEndpointRenewalAsync(InfluxEndpoint newConfig)
    {
        await _configLock.WaitAsync();
        try
        {
            _config = newConfig;
            _client?.Dispose();
            InitializeClient();

            Log.Information("InfluxDB client updated with new endpoint: {Endpoint}", _config.Endpoint);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Failed to update InfluxDB client after endpoint renewal");
            _isConfigured = false;
        }
        finally
        {
            _configLock.Release();
        }
    }

    #endregion

    #region Insertion Methods

    /// <summary>
    /// Inserts a collection of measurement data into InfluxDB.
    /// </summary>
    /// <param name="data">Data to insert into the db.</param>
    /// <param name="configuration">The collector configuration.</param>
    /// <returns>True if success, false otherwise.</returns>
    public async Task<bool> BulkInsertAsync(IEnumerable<IMeasurementData> data, CollectorConfiguration configuration)
    {
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

        var dataList = data.ToList();

        try
        {
            Log.Debug("Inserting {Count} measurements to InfluxDB at {Endpoint}", dataList.Count, _config!.Endpoint);
            return await TryBulkInsertWithRetryAsync(dataList, configuration);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error inserting measurements to InfluxDB at {Endpoint}", _config!.Endpoint);
            return false;
        }
    }

    private async Task<bool> TryBulkInsertWithRetryAsync(IEnumerable<IMeasurementData> data, CollectorConfiguration configuration)
    {
        try
        {
            return await BulkInsertInternalAsync(data, configuration);
        }
        catch (Exception ex) when (IsConnectionException(ex))
        {
            Log.Warning(ex, "Connection issue with InfluxDB at {Endpoint}, attempting to renew endpoint", _config!.Endpoint);
            return await TryReconnectAndRetryAsync(data, configuration);
        }
    }

    private async Task<bool> TryReconnectAndRetryAsync(IEnumerable<IMeasurementData> data, CollectorConfiguration configuration)
    {
        if (await _collectorEndpointManager.TryRenewEndpointAsync())
        {
            Log.Information("Endpoint renewed, retrying operation");
            try
            {
                return await BulkInsertInternalAsync(data, configuration);
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

    private async Task<bool> BulkInsertInternalAsync(IEnumerable<IMeasurementData> data, CollectorConfiguration configuration)
    {
        IEnumerable<PointData> groupedPoints = data
            .GroupBy(m => m.TimeStamp)
            .Select(group =>
            {
                var fields = group.ToDictionary(
                    m => m.SensorName,
                    m => m.GetRawValue());

                return PointData
                    .Measurement(configuration.DeviceName)
                    .SetTimestamp(DateTime.SpecifyKind(group.Key, DateTimeKind.Utc))
                    .SetFields(fields);
            });

        await _client!.WritePointsAsync(groupedPoints);
        return true;
    }

    #endregion

    #region Conversion & Helpers

    private bool IsConnectionException(Exception ex) =>
        ex is HttpRequestException ||
        ex is SocketException ||
        ex is TimeoutException ||
        ex.Message.Contains("connection", StringComparison.OrdinalIgnoreCase);

    /// <inheritdoc/>
    public void Dispose()
    {
        _client?.Dispose();
        _collectorEndpointManager.EndpointRenewed -= async (s, e) => await HandleEndpointRenewalAsync(e);
    }

    /// <inheritdoc/>
    public async Task<bool> IsConnectedAsync()
        => _config is not null && await _config.CheckEndPointValidityAsync();
    #endregion
}
