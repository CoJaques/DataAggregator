namespace DataAggregator.Collector.DataCollector.DataStorage.Influx;

/// <summary>
/// Configuration for InfluxDB connection.
/// </summary>
public class InfluxDbConfig
{
    /// <summary>
    /// Gets or sets the InfluxDB endpoint URL.
    /// </summary>
    public string Endpoint { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the authentication token for InfluxDB.
    /// </summary>
    public string Token { get; set; } = string.Empty;
}
