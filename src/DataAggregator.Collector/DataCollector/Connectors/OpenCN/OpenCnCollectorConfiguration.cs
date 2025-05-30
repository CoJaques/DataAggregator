using DataAggregator.Collector.DataCollector.Abstraction.Configuration;
using DataAggregator.Collector.DataCollector.Connectors.CapnProto;

namespace DataAggregator.Collector.DataCollector.Connectors.OpenCN;

/// <summary>
/// Configuration specific for OpenCN collector.
/// </summary>
public class OpenCnCollectorConfiguration : CollectorConfiguration
{
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
    /// Gets or sets the list of sensors configured for this OpenCN collector.
    /// </summary>
    public new List<OpenCnSensorConfig> Sensors { get; set; } = [];
}
