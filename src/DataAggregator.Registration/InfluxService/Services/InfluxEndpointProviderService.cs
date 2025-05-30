using DataAggregator.Registration.InfluxService.Configuration;
using DataAggregator.Shared.Configuration.TimeSeries;
using Microsoft.Extensions.Options;

namespace DataAggregator.Registration.InfluxService.Services;

/// <summary>
/// Service for managing InfluxDB endpoints and their health checks.
/// Implement simple endpoint selection logic based first healthy endpoint found.
/// </summary>
public class InfluxEndpointProviderService(IOptions<InfluxEndpointsConfiguration> options) : IInfluxEndpointProviderService
{
    private readonly InfluxEndpointsConfiguration _configuration = options.Value;

    /// <inheritdoc/>
    public async Task<InfluxEndpoint> GetAvailableEndpointAsync()
    {
        foreach (InfluxEndpoint endpoint in _configuration.Endpoints)
        {
            if (await CheckEndPointValidityAsync(endpoint))
            {
                return endpoint;
            }
        }

        throw new InvalidOperationException("No healthy InfluxDB endpoint is available.");
    }

    /// <inheritdoc/>
    public async Task<bool> CheckEndPointValidityAsync(InfluxEndpoint endpoint)
        => await endpoint.CheckEndPointValidityAsync();
}
