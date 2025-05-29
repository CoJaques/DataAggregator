namespace DataAggregator.Collector.Configuration;

/// <summary>
/// Record representing the configuration for a collector device.
/// </summary>
/// <param name="DeviceName">The device name.</param>
/// <param name="Location">The location of the device.</param>
/// <param name="HealthCheckEndpoint">An endpoint where healtcheck status can be perceived.</param>
public record CollectorConfiguration(string DeviceName, string Location, string HealthCheckEndpoint);
