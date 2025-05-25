using System.ComponentModel.DataAnnotations;

namespace DataAggregator.Registration.Entities;

/// <summary>
/// Class representing a registered device using to store in DB.
/// </summary>
public class Device
{
    /// <summary>
    /// Gets or sets the unique identifier of the device.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the device.
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string DeviceName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the location of the device.
    /// </summary>
    [MaxLength(200)]
    public string Location { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the health check endpoint of the device.
    /// </summary>
    [MaxLength(200)]
    public string HealthCheckEndpoint { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the registration date of the device.
    /// </summary>
    [Required]
    public DateTime RegistrationDate { get; set; }

    /// <summary>
    /// Gets or sets the assigned time series endpoint for the device.
    /// </summary>
    [MaxLength(200)]
    public string AssignedTimeSeriesEndpoint { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the collection of sensors associated with the device.
    /// </summary>
    public ICollection<Sensor> Sensors { get; set; } = [];
}
