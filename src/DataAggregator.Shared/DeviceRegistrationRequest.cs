using DataAggregator.Shared.DTOs;

namespace DataAggregator.Shared;

/// <summary>
/// Record representing a request to register a device.
/// </summary>
public record DeviceRegistrationRequest(
    string DeviceName,
    string Location,
    string HealthCheckEndpoint,
    List<SensorInfoDto> Sensors);
