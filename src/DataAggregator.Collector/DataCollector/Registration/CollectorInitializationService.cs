using System.Collections.Concurrent;
using DataAggregator.Collector.DataCollector.Abstraction.Configuration;
using DataAggregator.Collector.DataCollector.DataStorage.Influx;
using DataAggregator.Shared;
using Serilog;

namespace DataAggregator.Collector.DataCollector.Registration;

/// <summary>
/// Service responsible for initializing and managing collector configurations and endpoints.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="CollectorInitializationService"/> class.
/// </remarks>
/// <param name="registrationService">The registration service used to register and get endpoints.</param>
/// <param name="configuration">The collector configuration.</param>
public class CollectorInitializationService(
    RegistrationService registrationService,
    CollectorConfiguration configuration)
{
    #region Private Fields
    private readonly SemaphoreSlim _renewLock = new(1, 1);
    private readonly ConcurrentDictionary<string, DateTime> _renewAttempts = new();

    private bool _initialized;
    private InfluxDbConfig? _influxConfig;
    #endregion

    #region Events

    /// <summary>
    /// Event that is triggered when the endpoint is renewed.
    /// </summary>
    public event EventHandler<InfluxDbConfig>? EndpointRenewed;
    #endregion

    #region Public Methods

    /// <summary>
    /// Initializes the collector service by registering with the central service.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    /// <exception cref="InvalidOperationException">Thrown when registration fails.</exception>
    public async Task InitializeAsync()
    {
        if (_initialized && _influxConfig != null)
        {
            return;
        }

        await _renewLock.WaitAsync();
        try
        {
            if (_initialized && _influxConfig != null)
            {
                return;
            }

            DeviceRegistrationResponse response = await RegisterCollectorAsync();

            if (!response.IsSuccess)
            {
                throw new InvalidOperationException($"Failed to register collector {configuration.DeviceName}, probably" +
                    $"no endpoint available");
            }

            _influxConfig = new InfluxDbConfig
            {
                Endpoint = response.AssignedTimeSeriesEndpoint,
                Token = response.DeviceToken,
            };

            _initialized = true;

            Log.Information(
                "Collector {DeviceName} initialized with endpoint {Endpoint}",
                configuration.DeviceName,
                _influxConfig.Endpoint);
        }
        finally
        {
            _renewLock.Release();
        }
    }

    /// <summary>
    /// Gets the current InfluxDB configuration.
    /// </summary>
    /// <returns>The InfluxDB configuration.</returns>
    /// <exception cref="InvalidOperationException">Thrown when collector is not initialized.</exception>
    public InfluxDbConfig GetInfluxConfig() => !_initialized || _influxConfig == null
            ? throw new InvalidOperationException("Collector not initialized. Call InitializeAsync first.")
            : _influxConfig;

    /// <summary>
    /// Attempts to renew the endpoint if it's no longer valid.
    /// </summary>
    /// <returns>A <see cref="Task"/> with a boolean indicating if renewal was successful.</returns>
    public async Task<bool> TryRenewEndpointAsync()
    {
        // Rate limiting - don't try to renew more than once per minute
        string key = "renew";
        if (_renewAttempts.TryGetValue(key, out DateTime lastAttempt) &&
            (DateTime.UtcNow - lastAttempt).TotalMinutes < 1)
        {
            Log.Debug("Skipping endpoint renewal attempt - last attempt was less than 1 minute ago");
            return false;
        }

        await _renewLock.WaitAsync();
        try
        {
            _renewAttempts[key] = DateTime.UtcNow;

            Log.Information("Attempting to renew InfluxDB endpoint for collector {DeviceName}", configuration.DeviceName);

            DeviceRegistrationResponse response = await RegisterCollectorAsync();

            if (!response.IsSuccess)
            {
                Log.Error("Failed to renew endpoint for collector {DeviceName}", configuration.DeviceName);
                return false;
            }

            var newConfig = new InfluxDbConfig
            {
                Endpoint = response.AssignedTimeSeriesEndpoint,
                Token = response.DeviceToken,
            };

            // Check if endpoint actually changed
            bool changed = _influxConfig == null ||
                          _influxConfig.Endpoint != newConfig.Endpoint ||
                          _influxConfig.Token != newConfig.Token;

            _influxConfig = newConfig;

            if (changed)
            {
                Log.Information(
                    "Endpoint for collector {DeviceName} renewed: {Endpoint}",
                    configuration.DeviceName,
                    _influxConfig.Endpoint);

                // Notify subscribers
                OnEndpointRenewed(_influxConfig);
            }

            return true;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error while attempting to renew endpoint for collector {DeviceName}", configuration.DeviceName);
            return false;
        }
        finally
        {
            _renewLock.Release();
        }
    }
    #endregion

    #region Private Methods

    private async Task<DeviceRegistrationResponse> RegisterCollectorAsync()
    {
        Log.Debug("Registering collector {DeviceName} with central service", configuration.DeviceName);
        return await registrationService.RegisterCollectorAsync(configuration);
    }

    private void OnEndpointRenewed(InfluxDbConfig config)
        => EndpointRenewed?.Invoke(this, config);
    #endregion
}
