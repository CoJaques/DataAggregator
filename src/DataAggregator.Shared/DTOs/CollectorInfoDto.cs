using DataAggregator.Shared.Configuration.TimeSeries;
using DataAggregator.Shared.Domain;

namespace DataAggregator.Shared.DTOs;

/// <summary>
/// DTO representing information about a collector.
/// </summary>
public record CollectorInfoDto(
    string DeviceName,
    string Location,
    string HealthCheckEndpoint,
    InfluxEndpoint AssignedInfluxEndpoint,
    List<SensorInfoDto> Sensors,
    List<EndpointHistory> EndpointHistories);
