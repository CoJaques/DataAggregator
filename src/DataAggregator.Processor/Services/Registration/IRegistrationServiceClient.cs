using DataAggregator.Shared.DTOs;

namespace DataAggregator.Processor.Services.Registration;

/// <summary>
/// Interface for the registration service client.
/// </summary>
public interface IRegistrationServiceClient
{
    /// <summary>
    /// Gets collector information from the registration service.
    /// </summary>
    /// <param name="deviceName">The name of the device.</param>
    /// <returns>The collector information.</returns>
    public Task<CollectorInfoDto?> GetCollectorInfoAsync(string deviceName);
}
