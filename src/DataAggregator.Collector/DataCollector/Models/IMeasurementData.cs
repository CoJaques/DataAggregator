namespace DataAggregator.Collector.DataCollector.Models;

// TODO CJS -> Check if it's better to have a SensorConfig here and work with its datatype

/// <summary>
/// Interface for common measurement data operations regardless of the value type.
/// </summary>
public interface IMeasurementData
{
    /// <summary>
    /// Gets the timestamp when the measurement was taken.
    /// </summary>
    public DateTime TimeStamp { get; }

    /// <summary>
    /// Gets the name of the sensor that produced this measurement.
    /// </summary>
    public string SensorName { get; }

    /// <summary>
    /// Gets the type of the measurement value.
    /// </summary>
    public Type ValueType { get; }

    /// <summary>
    /// Gets the raw value of the measurement as an object.
    /// </summary>
    /// <returns>The measurement value as an object.</returns>
    public object GetRawValue();
}
