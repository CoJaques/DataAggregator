using DataAggregator.Shared.Configuration.TimeSeries;

namespace DataAggregator.Shared.Domain;

/// <summary>
/// Represents the history of a time series endpoint assigned to a device.
/// </summary>
public record EndpointHistory(InfluxEndpoint Endpoint, DateTime StartDate, DateTime EndDate);
