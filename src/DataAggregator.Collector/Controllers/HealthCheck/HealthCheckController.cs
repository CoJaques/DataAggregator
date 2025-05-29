using DataAggregator.Collector.DataCollector.Abstraction;
using DataAggregator.Collector.DataCollector.DataStorage;
using DataAggregator.Collector.DataCollector.LocalStorage;
using Microsoft.AspNetCore.Mvc;

namespace DataAggregator.Collector.Controllers.HealthCheck;

/// <summary>
/// Controller for exposing health check information about the collector.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="HealthCheckController"/> class.
/// </remarks>
/// <param name="collectorService">The collector service.</param>
/// <param name="dataBufferService">The data buffer service.</param>
/// <param name="dataRepository">The data repository.</param>
[ApiController]
[Route("api/[controller]")]
public class HealthCheckController(
    CollectorService collectorService,
    DataBufferService dataBufferService,
    IDataRepository dataRepository) : ControllerBase
{
    /// <summary>
    /// Gets the health status of the collector.
    /// </summary>
    /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(HealthStatus), 200)]
    [ProducesResponseType(500)]
    public ActionResult<HealthStatus> GetHealthStatus()
    {
        try
        {
            // TODO CJS -> Implement correct healthCheck.
            var healthStatus = new HealthStatus
            {
                Status = "Healthy", // Default status
                Message = "Collector is running normally",
                LastDataSent = DateTime.UtcNow, // This should be replaced with the actual last sent time
                BufferSize = dataBufferService.GetBufferSize(),
                DatabaseConnected = true, // This should be checked dynamically
                Timestamp = DateTime.UtcNow,
            };

            // Determine actual health status based on various checks
            if (dataBufferService.GetBufferSize() > 1000)
            {
                healthStatus.Status = "Degraded";
                healthStatus.Message = "Buffer size is high, possible connectivity issues";
            }

            // Check database connection
            try
            {
                // Placeholder for actual DB connection check
                // We could use a minimal bulk insert operation to test connectivity
                // Or implement a specific Ping method on the repository
                healthStatus.DatabaseConnected = true; // For now hardcoded
            }
            catch
            {
                healthStatus.Status = "Unhealthy";
                healthStatus.Message = "Unable to connect to database";
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
