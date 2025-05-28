using DataAggregator.Shared;

namespace DataAggregator.Registration.InfluxService.Services;

/// <summary>
/// Interface for managing InfluxDB endpoints and their health checks.
/// </summary>
public interface IInfluxEndpointProviderService
{
    /// <summary>
    /// Retrieves an available InfluxDB endpoint that is healthy.
    /// Can define the logic to select the endpoint based on health checks or other criteria.
    /// </summary>
    /// <returns>A valid InfluxEndpoint.</returns>
    public Task<InfluxEndpoint> GetAvailableEndpointAsync();

    /// <summary>
    /// Check the health of a specific InfluxDB endpoint.
    /// </summary>
    /// <param name="endpoint">The endpoints to check.</param>
    /// <returns>Return true if the endpoint is reachable and healthy, false otherwise.</returns>
    public Task<bool> CheckEndPointValidityAsync(InfluxEndpoint endpoint);
}
