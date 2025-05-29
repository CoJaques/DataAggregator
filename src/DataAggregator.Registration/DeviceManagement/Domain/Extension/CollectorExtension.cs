using DataAggregator.Shared;

namespace DataAggregator.Registration.DeviceManagement.Domain.Extension;

/// <summary>
/// Static class extension methods for converting between <see cref="Collector"/> and <see cref="CollectorInfoDto"/>.
/// </summary>
public static class CollectorExtension
{
    /// <summary>
    /// Convert <see cref="Collector"/> to <see cref="CollectorInfoDto"/>.
    /// </summary>
    /// <param name="collector">The device to convert.</param>
    /// <returns>A new collectorInfoDto.</returns>
    public static CollectorInfoDto ToDto(this Collector collector)
        => new(
            collector.DeviceName,
            collector.Location,
            collector.HealthCheckEndpoint,
            collector.AssignedInfluxEndpoint,
            [.. collector.Sensors.Select(sensor => sensor.ToDto())],
            collector.EndpointHistories);
}
