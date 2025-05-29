namespace DataAggregator.Shared.DTOs;

/// <summary>
/// Record used as DTO for sensor information.
/// </summary>
public record SensorInfoDto(string SensorName, string Type, string Unit, Dictionary<string, string> Metadata);
