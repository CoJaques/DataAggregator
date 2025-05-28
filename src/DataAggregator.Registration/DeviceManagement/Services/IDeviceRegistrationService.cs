using DataAggregator.Shared;

namespace DataAggregator.Registration.DeviceManagement.Services;

/// <summary>
/// Service interface for device registration operations.
/// </summary>
public interface IDeviceRegistrationService
{
    /// <summary>
    /// Register a device with the provided configuration.
    /// </summary>
    /// <param name="request">The request parameters.</param>
    /// <returns>Response of the request.</returns>
    public Task<DeviceRegistrationResponse> RegisterCollectorAsync(DeviceRegistrationRequest request);

    /// <summary>
    /// Method to get device information by its ID.
    /// </summary>
    /// <param name="collectorName">The id of the device.</param>
    /// <returns>Information concerning the device if it exists.</returns>
    public Task<CollectorInfoDto?> GetCollectorInfoAsync(string collectorName);

    /// <summary>
    /// Method to check if a device is registered.
    /// </summary>
    /// <param name="collectorName">The id of the device.</param>
    /// <returns>True if it exist, false otherwise.</returns>
    public Task<bool> IsCollectorRegisteredAsync(string collectorName);

    /// <summary>
    /// Get all registered collector information asynchronously.
    /// </summary>
    /// <returns> A collection of collector information DTOs.</returns>
    public Task<IEnumerable<CollectorInfoDto>> GetAllCollectorInfoAsync();
}
