namespace DataAggregator.Collector.DataCollector.Models;

/// <summary>
/// Interface for common measurement data operations regardless of the value type.
/// </summary>
public interface IMeasurementData
{
    /// <summary>
    /// Gets the timestamp when the measurement was taken.
    /// </summary>
    DateTime TimeStamp { get; }
    
    /// <summary>
    /// Gets the name of the sensor that produced this measurement.
    /// </summary>
    string SensorName { get; }
    
    /// <summary>
    /// Gets the type of the measurement value.
    /// </summary>
    Type ValueType { get; }
    
    /// <summary>
    /// Gets the raw value of the measurement as an object.
    /// </summary>
    /// <returns>The measurement value as an object.</returns>
    object GetRawValue();
}

/// <summary>
/// Represents a single measurement data point from a sensor.
/// </summary>
/// <typeparam name="T">The type of the measurement value.</typeparam>
public record MeasurementData<T>(DateTime TimeStamp, string SensorName, T Value) : IMeasurementData
{
    /// <inheritdoc/>
    public Type ValueType => typeof(T);
    
    /// <inheritdoc/>
    public object GetRawValue() => Value!;
}
