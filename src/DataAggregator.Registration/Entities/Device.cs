namespace DataAggregator.Registration.Entities;

/// <summary>
/// Class representing a registered device using to store in DB.
/// </summary>
public class Device
{
    /// <summary>
    /// Gets or sets the unique identifier of the device.
    /// </summary>
    public string DeviceId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the name of the device.
    /// </summary>
    public string DeviceName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the location of the device.
    /// </summary>
    public string Location { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the health check endpoint of the device.
    /// </summary>
    public string HealthCheckEndpoint { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the registration date of the device.
    /// </summary>
    public DateTime RegistrationDate { get; set; }

    /// <summary>
    /// Gets or sets the assigned time series endpoint for the device.
    /// </summary>
    public string AssignedTimeSeriesEndpoint { get; set; } = string.Empty;
}
