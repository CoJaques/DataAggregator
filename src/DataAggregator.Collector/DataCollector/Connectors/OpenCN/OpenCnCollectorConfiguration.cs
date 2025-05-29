using DataAggregator.Collector.DataCollector.Abstraction.Configuration;
using DataAggregator.Collector.DataCollector.Connectors.CapnProto;
using DataAggregator.Collector.DataCollector.DataStorage.Influx;

namespace DataAggregator.Collector.DataCollector.Connectors.OpenCN;

/// <summary>
/// Configuration specific for OpenCN collector.
/// </summary>
public class OpenCnCollectorConfiguration : CollectorConfiguration
{
    /// <summary>
    /// Gets or sets the list of pins used by OpenCN.
    /// </summary>
    public List<string> PinsList { get; set; } = [];

    /// <summary>
    /// Gets or sets the configuration string for OpenCN.
    /// </summary>
    public string CfgString { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the sampling rate in Hz.
    /// </summary>
    public int SamplingRate { get; set; } = 500;

    /// <summary>
    /// Gets or sets the CapnProto configuration.
    /// </summary>
    public CapnProtoConfig CapnProto { get; set; } = new();

    /// <summary>
    /// Gets or sets the InfluxDB configuration.
    /// </summary>
    public InfluxDbConfig InfluxDB { get; set; } = new();
}
