using DataAggregator.Collector.Shared.Abstraction.Configuration;

namespace DataAggregator.Collector.HttpCollector.Configuration;

/// <summary>
/// Configuration for the HTTP Push collector.
/// </summary>
public class HttpCollectorConfiguration : CollectorConfiguration
{
    /// <summary>
    /// Gets or sets the relative endpoint for receiving data.
    /// Default is "api/collect".
    /// </summary>
    public string Endpoint { get; set; } = "api/collect";
}
