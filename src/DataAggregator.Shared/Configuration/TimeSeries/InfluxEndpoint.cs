namespace DataAggregator.Shared;

/// <summary>
/// Record which define the configuration for an InfluxDB endpoint.
/// </summary>
/// <param name="Name">The name of the endpoint.</param>
/// <param name="Endpoint">The endpoint.</param>
/// <param name="Token">The token for the endpoint.</param>
public record InfluxEndpoint(string Name, string Endpoint, string Token);
