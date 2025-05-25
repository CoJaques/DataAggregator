namespace DataAggregator.Shared;

/// <summary>
/// Record used as DTO for sensor information.
/// </summary>
public record SensorInfoDto(string SensorName, string Type, string Unit, IDictionary<string, string> Metadata);
