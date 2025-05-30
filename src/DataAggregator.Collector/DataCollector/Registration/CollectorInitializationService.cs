using DataAggregator.Collector.DataCollector.Abstraction.Configuration;
using DataAggregator.Collector.DataCollector.DataStorage.Influx;
using DataAggregator.Shared;
using DataAggregator.Shared.DTOs;
using Serilog;
using System.Collections.Concurrent;

namespace DataAggregator.Collector.DataCollector.Registration;

/// <summary>
/// Service responsible for initializing and managing collector configurations and endpoints.
/// </summary>
public class CollectorInitializationService
{
    private readonly RegistrationService _registrationService;
    private readonly CollectorConfiguration _configuration;
    
    private bool _initialized;
    private InfluxDbConfig? _influxConfig;
    
    private readonly SemaphoreSlim _renewLock = new(1, 1);
    private readonly ConcurrentDictionary<string, DateTime> _renewAttempts = new();
    
    /// <summary>
    /// Event that is triggered when the endpoint is renewed.
    /// </summary>
    public event EventHandler<InfluxDbConfig>? EndpointRenewed;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="CollectorInitializationService"/> class.
    /// </summary>
    /// <param name="registrationService">The registration service used to register and get endpoints.</param>
    /// <param name="configuration">The collector configuration.</param>
    public CollectorInitializationService(RegistrationService registrationService, CollectorConfiguration configuration)
    {
        _registrationService = registrationService;
        _configuration = configuration;
    }
    
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
                throw new InvalidOperationException($"Failed to register collector {_configuration.DeviceName}");
            }
            
            _influxConfig = new InfluxDbConfig
            {
                Endpoint = response.AssignedTimeSeriesEndpoint,
                Token = response.DeviceToken
            };
            
            _initialized = true;
            
            Log.Information(
                "Collector {DeviceName} initialized with endpoint {Endpoint}", 
                _configuration.DeviceName, 
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
    public InfluxDbConfig GetInfluxConfig()
    {
        if (!_initialized || _influxConfig == null)
        {
            throw new InvalidOperationException("Collector not initialized. Call InitializeAsync first.");
        }
        
        return _influxConfig;
    }
    
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
            
            Log.Information("Attempting to renew InfluxDB endpoint for collector {DeviceName}", _configuration.DeviceName);
            
            DeviceRegistrationResponse response = await RegisterCollectorAsync();
            
            if (!response.IsSuccess)
            {
                Log.Error("Failed to renew endpoint for collector {DeviceName}", _configuration.DeviceName);
                return false;
            }
            
            var newConfig = new InfluxDbConfig
            {
                Endpoint = response.AssignedTimeSeriesEndpoint,
                Token = response.DeviceToken
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
                    _configuration.DeviceName, 
                    _influxConfig.Endpoint);
                
                // Notify subscribers
                OnEndpointRenewed(_influxConfig);
            }
            
            return true;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error while attempting to renew endpoint for collector {DeviceName}", _configuration.DeviceName);
            return false;
        }
        finally
        {
            _renewLock.Release();
        }
    }
    
    private async Task<DeviceRegistrationResponse> RegisterCollectorAsync()
    {
        Log.Debug("Registering collector {DeviceName} with central service", _configuration.DeviceName);
        return await _registrationService.RegisterCollectorAsync(_configuration);
    }
    
    private void OnEndpointRenewed(InfluxDbConfig config)
    {
        EndpointRenewed?.Invoke(this, config);
    }
}
