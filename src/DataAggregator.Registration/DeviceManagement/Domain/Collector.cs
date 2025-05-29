using DataAggregator.Registration.Domain;
using DataAggregator.Shared;

namespace DataAggregator.Registration.DeviceManagement.Domain;

/// <summary>
/// Class representing a registered device using to store in DB.
/// </summary>
public class Collector
{
    /// <summary>
    /// Gets or sets the unique identifier of the device.
    /// </summary>
    public Guid Id { get; set; }

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
    public required InfluxEndpoint AssignedInfluxEndpoint { get; set; }

    /// <summary>
    /// Gets or sets the collection of sensors associated with the device.
    /// </summary>
    public List<Sensor> Sensors { get; set; } = [];

    /// <summary>
    /// Gets or sets the history of assigned time series endpoints.
    /// </summary>
    public List<EndpointHistory> EndpointHistories { get; set; } = [];
}
