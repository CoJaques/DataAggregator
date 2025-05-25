namespace DataAggregator.Shared;

/// <summary>
/// Record representing a request to register a device.
/// </summary>
/// <param name="Config">Gets the configuration of the sensor to be registered.</param>
public record DeviceRegistrationRequest(CollectorInfoDto Config);
