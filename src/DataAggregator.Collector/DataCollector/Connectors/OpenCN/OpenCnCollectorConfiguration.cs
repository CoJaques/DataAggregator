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
    /// Gets the configuration string for OpenCN.
    /// </summary>
    public string CfgString => GetConfigString();

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

    /// <summary>
    /// Gets or sets the list of sensors configured for this OpenCN collector.
    /// </summary>
    public new List<OpenCnSensorConfig> Sensors { get; set; } = [];

    private string GetConfigString()
        => string.Concat(
            Sensors.Select(
                sensor
                => sensor.DataType switch
                {
                    SensorDataType.Boolean => 'b',
                    SensorDataType.Integer => 'i',
                    SensorDataType.Double => 'f',
                    SensorDataType.String => 's',
                    _ => throw new ArgumentOutOfRangeException(
                             nameof(sensor.DataType),
                             $"Unsupported sensor data type: {sensor.DataType}"),
                }));
}
