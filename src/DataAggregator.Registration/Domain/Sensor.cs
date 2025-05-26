using System.Diagnostics.CodeAnalysis;

namespace DataAggregator.Registration.Domain;

/// <summary>
/// Class representing a sensor associated with a device using to store in DB.
/// </summary>
public class Sensor()
{
    /// <summary>
    /// Gets or sets the unique identifier of the sensor.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the sensor.
    /// </summary>
    public string SensorName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the type of the sensor.
    /// </summary>
    public string SensorType { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the unit of measurement for the sensor.
    /// </summary>
    public string Unit { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets additional metadata for the sensor.
    /// </summary>
    public Dictionary<string, string> Metadata { get; set; } = [];

    /// <summary>
    /// Gets or sets the associated device.
    /// </summary>
    [DisallowNull]
    public required Device? Device { get; set; }
}
