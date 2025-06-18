using DataAggregator.Collector.Shared.Abstraction;
using Microsoft.AspNetCore.Mvc;

namespace DataAggregator.Collector.Controllers.HealthCheck;

/// <summary>
/// Controller for exposing health check information about the collector.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="HealthCheckController"/> class.
/// </remarks>
/// <param name="collectorService">The collector service.</param>
[ApiController]
[Route("api/[controller]")]
public class HealthCheckController(
    CollectorService collectorService) : ControllerBase
{
    /// <summary>
    /// Gets the health status of the collector.
    /// </summary>
    /// <returns>A response containing the health status.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(HealthStatus), 200)]
    [ProducesResponseType(500)]
    public async Task<ActionResult<HealthStatus>> GetHealthStatus()
    {
        try
        {
            var healthStatus = new HealthStatus(
                "Healthy", // Default status
                "Collector is running normally",
                collectorService.LastDataSent,
                collectorService.BufferSize,
                true, // Default value
                DateTime.UtcNow);

            // Determine actual health status based on various checks
            if (collectorService.BufferSize > 1000)
            {
                healthStatus.Status = "Degraded";
                healthStatus.Message = "Buffer size is high, possible connectivity issues";
            }

            // Check if we haven't sent data for a long time
            if (collectorService.LastDataSent == DateTime.MinValue ||
                (DateTime.UtcNow - collectorService.LastDataSent).TotalMinutes > 5)
            {
                healthStatus.Status = "Degraded";
                healthStatus.Message = "No data sent in the last 5 minutes";
            }

            // Check if the endpoint was configured
            if (!await collectorService.IsRepositoryConnected())
            {
                healthStatus.Status = "Unhealthy";
                healthStatus.Message = "No valid endpoint configuration";
                healthStatus.DatabaseConnected = false;
            }

            return Ok(healthStatus);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred while retrieving health status: {ex.Message}");
        }
    }
}
