using DataAggregator.Collector.Shared.Abstraction.Configuration;

namespace DataAggregator.Collector.DataCollector.Connectors.OpenCN;

/// <summary>
/// Class which defines the configuration for OpenCN sensors.
/// </summary>
public class OpenCnSensorConfig : SensorConfig
{
    /// <summary>
    /// Gets or sets the pin name associated with this sensor.
    /// </summary>
    public string PinName { get; set; } = string.Empty;
}
