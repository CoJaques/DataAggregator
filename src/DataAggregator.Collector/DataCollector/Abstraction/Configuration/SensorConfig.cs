namespace DataAggregator.Collector.DataCollector.Abstraction.Configuration;

/// <summary>
/// Configuration for an individual sensor.
/// </summary>
public class SensorConfig
{
    /// <summary>
    /// Gets or sets the name of the sensor.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the type of the sensor.
    /// </summary>
    public string Type { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the unit of measurement.
    /// </summary>
    public string Unit { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the type of data this sensor produces.
    /// </summary>
    public SensorDataType DataType { get; set; } = SensorDataType.Double;

    /// <summary>
    /// Gets or sets additional metadata for the sensor.
    /// </summary>
    public Dictionary<string, string> Metadata { get; set; } = [];

    /// <summary>
    /// Gets the .NET type corresponding to the sensor's data type.
    /// </summary>
    /// <returns>The corresponding .NET Type.</returns>
    public Type GetClrType()
    {
        return DataType switch
        {
            SensorDataType.Boolean => typeof(bool),
            SensorDataType.Integer => typeof(int),
            SensorDataType.Double => typeof(double),
            SensorDataType.String => typeof(string),
            _ => typeof(object)
        };
    }
}
