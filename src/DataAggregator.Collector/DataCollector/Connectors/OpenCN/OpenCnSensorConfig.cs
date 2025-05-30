using DataAggregator.Collector.DataCollector.Abstraction.Configuration;

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

    /// <summary>
    /// Gets or sets the type of the measurement this sensor is configured to collect.
    /// </summary>
    public Type DataType { get; set; }
}
