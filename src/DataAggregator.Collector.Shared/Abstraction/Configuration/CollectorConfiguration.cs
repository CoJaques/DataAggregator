namespace DataAggregator.Collector.Shared.Abstraction.Configuration;

/// <summary>
/// Base configuration for a collector.
/// </summary>
public class CollectorConfiguration
{
    /// <summary>
    /// Gets or sets the device name.
    /// </summary>
    public string DeviceName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the location of the device.
    /// </summary>
    public string Location { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the health check endpoint URL.
    /// </summary>
    public string HealthCheckEndpoint { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the list of sensors configured for this collector.
    /// </summary>
    public List<SensorConfig> Sensors { get; set; } = [];

    /// <summary>
    /// Gets or sets the interval in milliseconds for flushing data to the time series database.
    /// </summary>
    public int FlushIntervalMilliseconds { get; set; } = 1000;
}
