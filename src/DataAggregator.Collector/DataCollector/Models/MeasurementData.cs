namespace DataAggregator.Collector.DataCollector.Models;

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
