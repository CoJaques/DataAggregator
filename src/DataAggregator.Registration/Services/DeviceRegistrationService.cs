using DataAggregator.Registration.Repositories;
using DataAggregator.Shared;

namespace DataAggregator.Registration.Services;

/// <summary>
/// Class for handling device registration operations.
/// </summary>
/// <param name="deviceRepository">The device repository.</param>
public class DeviceRegistrationService(IDeviceRepository deviceRepository) : IDeviceRegistrationService
{
    /// <inheritdoc/>
    public Task<DeviceRegistrationResponse> RegisterCollectorAsync(DeviceRegistrationRequest request) => throw new NotImplementedException();

    /// <inheritdoc/>
    public Task<CollectorInfoDto?> GetCollectorInfoAsync(string collectorName) => throw new NotImplementedException();

    /// <inheritdoc/>
    public Task<bool> IsCollectorRegisteredAsync(string collectorName) => throw new NotImplementedException();

    /// <inheritdoc/>
    public Task<IEnumerable<CollectorInfoDto>> GetAllCollectorInfoAsync() => throw new NotImplementedException();

    private Task<InfluxEndpointConfiguration> GetAvailableInfluxEndpoint() => throw new NotImplementedException();
}
