namespace DataAggregator.Shared;

/// <summary>
/// Record used as DTO for collector information.
/// </summary>
public record CollectorInfoDto(string DeviceName, string Location, string HealthCheckEndpoint, IEnumerable<SensorInfoDto> Sensors);
