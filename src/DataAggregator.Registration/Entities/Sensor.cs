using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAggregator.Registration.Entities;

/// <summary>
/// Class representing a sensor associated with a device using to store in DB.
/// </summary>
public class Sensor()
{
    /// <summary>
    /// Gets or sets the unique identifier of the sensor.
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string SensorId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the unique identifier of the associated device.
    /// </summary>
    [Required]
    public string DeviceId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the name of the sensor.
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string SensorName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the type of the sensor.
    /// </summary>
    [MaxLength(50)]
    public string SensorType { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the unit of measurement for the sensor.
    /// </summary>
    [MaxLength(50)]
    public string Unit { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets additional metadata for the sensor.
    /// </summary>
    public Dictionary<string, string> Metadata { get; set; } = [];

    /// <summary>
    /// Gets or sets the associated device.
    /// </summary>
    [ForeignKey(nameof(DeviceId))]
    public Device? Device { get; set; }
}
