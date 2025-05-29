namespace DataAggregator.Collector.DataCollector.Models;

/// <summary>
/// Represents a single measurement data point from a sensor.
/// </summary>
public class MeasurementData
{
    /// <summary>
    /// Gets or sets the timestamp when the measurement was taken.
    /// </summary>
    public DateTime Timestamp { get; set; }

    /// <summary>
    /// Gets or sets the sensor identifier that produced this measurement.
    /// </summary>
    public string SensorName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the value of the measurement.
    /// </summary>
    public double Value { get; set; }
}
