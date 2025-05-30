using DataAggregator.Collector.DataCollector.Abstraction.Configuration;

namespace DataAggregator.Collector.DataCollector.Models;

/// <summary>
/// Represents a single measurement data point from a sensor.
/// </summary>
/// <typeparam name="T">The type of the measurement value.</typeparam>
public record MeasurementData<T>(DateTime TimeStamp, SensorConfig Sensor, T Value)
{
}
