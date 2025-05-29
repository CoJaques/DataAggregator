namespace DataAggregator.Collector.Controllers.HealthCheck;

/// <summary>
/// DTO representing the health status of a collector.
/// </summary>
public record HealthStatus(
    string Status,
    string Message,
    DateTime LastDataSent,
    int BufferSize,
    bool DatabaseConnected,
    DateTime Timestamp)
{
}
