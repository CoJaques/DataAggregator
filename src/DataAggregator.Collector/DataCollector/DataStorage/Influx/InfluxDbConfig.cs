namespace DataAggregator.Collector.DataCollector.DataStorage.Influx;

/// <summary>
/// Configuration for InfluxDB connection.
/// </summary>
public record InfluxDbConfig(string Endpoint, string Token);
