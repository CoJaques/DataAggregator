using DataAggregator.Shared.DTOs;

namespace DataAggregator.Registration.DeviceManagement.Domain.Extension;

/// <summary>
/// Static class extension methods for converting between <see cref="Sensor"/> and <see cref="SensorInfoDto"/>.
/// </summary>
public static class SensorExtension
{
    /// <summary>
    /// Converts a <see cref="Sensor"/> to a <see cref="SensorInfoDto"/>.
    /// </summary>
    /// <param name="sensor">The sensor to convert to Dto.</param>
    /// <returns>A new SensorInfoDto.</returns>
    public static SensorInfoDto ToDto(this Sensor sensor)
        => new(
            sensor.SensorName,
            sensor.SensorType,
            sensor.Unit,
            sensor.Metadata);
}
