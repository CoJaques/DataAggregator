namespace DataAggregator.Collector;

/// <summary>
/// Record representing the configuration for a sensor.
/// </summary>
/// <param name="Name">Gets the name of the sensor.</param>
/// <param name="Type">Gets the type of the sensor.</param>
/// <param name="Unit">Gets the unit of measurement for the sensor.</param>
/// <param name="Metadata">Gets or sets additional metadata for the sensor.</param>
public record SensorConfiguration(string Name, string Type, string Unit, Dictionary<string, string> Metadata);
