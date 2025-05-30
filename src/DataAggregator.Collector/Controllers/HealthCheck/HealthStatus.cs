namespace DataAggregator.Collector.Controllers.HealthCheck;

/// <summary>
/// DTO representing the health status of a collector.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="HealthStatus"/> class.
/// </remarks>
/// <param name="status">The collector status.</param>
/// <param name="message">A descriptive message.</param>
/// <param name="lastDataSent">Timestamp of last data sent.</param>
/// <param name="bufferSize">Current buffer size.</param>
/// <param name="databaseConnected">Database connection status.</param>
/// <param name="timestamp">Time of this health check.</param>
public class HealthStatus(
    string status,
    string message,
    DateTime lastDataSent,
    int bufferSize,
    bool databaseConnected,
    DateTime timestamp)
{
    /// <summary>
    /// Gets or sets  the collector status (e.g. Healthy, Degraded, Down).
    /// </summary>
    public string Status { get; set; } = status;

    /// <summary>
    /// Gets or sets  a descriptive message about the current health.
    /// </summary>
    public string Message { get; set; } = message;

    /// <summary>
    /// Gets or sets  the timestamp of the last successfully sent data.
    /// </summary>
    public DateTime LastDataSent { get; set; } = lastDataSent;

    /// <summary>
    /// Gets or sets  the current size of the data buffer.
    /// </summary>
    public int BufferSize { get; set; } = bufferSize;

    /// <summary>
    /// Gets or sets a value indicating whether the database is connected.
    /// </summary>
    public bool DatabaseConnected { get; set; } = databaseConnected;

    /// <summary>
    /// Gets or sets the timestamp at which the health status was recorded.
    /// </summary>
    public DateTime Timestamp { get; set; } = timestamp;
}
