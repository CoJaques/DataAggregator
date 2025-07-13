using DataAggregator.Shared;

namespace DataAggregator.Processor.Services.Registration;

/// <summary>
/// Interface for the registration service client.
/// </summary>
public interface IRegistrationServiceClient
{
    /// <summary>
    /// Gets device information from the registration service.
    /// </summary>
    /// <param name="deviceName">The name of the device.</param>
    /// <returns>The device registration response.</returns>
    public Task<DeviceRegistrationResponse> GetDeviceInfoAsync(string deviceName);
}
