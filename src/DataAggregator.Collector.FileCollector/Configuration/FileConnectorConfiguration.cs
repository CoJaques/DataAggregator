using DataAggregator.Collector.Shared.Abstraction.Configuration;

namespace DataAggregator.Collector.FileCollector.Configuration;

/// <summary>
/// Specifies the configuration for the File Connector.
/// </summary>
public class FileConnectorConfiguration : CollectorConfiguration
{
    /// <summary>
    /// Gets or sets the list of file paths to read data from.
    /// </summary>
    public List<string> Files { get; set; } = [];

    /// <summary>
    /// Gets or sets the sampling rate (in Hz) for data provision.
    /// </summary>
    public double SamplingRate { get; set; } = 100;
}
