using DataAggregator.Registration.Repositories;
using DataAggregator.Shared;
using Microsoft.Extensions.Options;

namespace DataAggregator.Registration.Services;

/// <summary>
/// Class for handling device registration operations.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="DeviceRegistrationService"/> class.
/// </remarks>
/// <param name="deviceRepository">The device repository.</param>
/// <param name="influxEndpoints">The list of InfluxDB endpoint configurations.</param>
public class DeviceRegistrationService(IDeviceRepository deviceRepository, IOptions<List<InfluxEndpointConfiguration>> influxEndpoints) : IDeviceRegistrationService
{
    private readonly List<InfluxEndpointConfiguration> _influxEndpoints = influxEndpoints.Value;

    /// <inheritdoc/>
    public Task<DeviceRegistrationResponse> RegisterCollectorAsync(DeviceRegistrationRequest request) => throw new NotImplementedException();

    /// <inheritdoc/>
    public Task<CollectorInfoDto?> GetCollectorInfoAsync(string collectorName) => throw new NotImplementedException();

    /// <inheritdoc/>
    public Task<bool> IsCollectorRegisteredAsync(string collectorName) => throw new NotImplementedException();

    /// <inheritdoc/>
    public Task<IEnumerable<CollectorInfoDto>> GetAllCollectorInfoAsync() => throw new NotImplementedException();

    /// <summary>
    /// Gets the available InfluxDB endpoint configuration.
    /// This method can be enhanced to define a specific logic for selecting the endpoint.
    /// </summary>
    /// <returns>The default InfluxDB endpoint configuration.</returns>
    public Task<InfluxEndpointConfiguration> GetAvailableInfluxEndpoint()
    {
        InfluxEndpointConfiguration? defaultEndpoint = _influxEndpoints.FirstOrDefault(e => e.IsDefault);
        return defaultEndpoint == null
            ? throw new InvalidOperationException("No default InfluxDB endpoint is configured.")
            : Task.FromResult(defaultEndpoint);
    }
}
