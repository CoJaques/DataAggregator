using DataAggregator.Shared.Configuration.TimeSeries;

namespace DataAggregator.Registration.InfluxService.Configuration;

/// <summary>
/// Configuration class for InfluxDB endpoints.
/// </summary>
public class InfluxEndpointsConfiguration
{
    /// <summary>
    /// Gets or sets the list of InfluxDB endpoint configurations.
    /// </summary>
    public List<InfluxEndpoint> Endpoints { get; set; } = [];
}
