using DataAggregator.Collector.DataCollector.Abstraction;
using DataAggregator.Collector.DataCollector.DataStorage;
using DataAggregator.Collector.DataCollector.LocalStorage;
using DataAggregator.Collector.DataCollector.Registration;
using Microsoft.AspNetCore.Mvc;

namespace DataAggregator.Collector.Controllers.HealthCheck;

/// <summary>
/// Controller for exposing health check information about the collector.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class HealthCheckController : ControllerBase
{
    private readonly CollectorService _collectorService;
    private readonly DataBufferService _dataBufferService;
    private readonly IDataRepository _dataRepository;
    private readonly CollectorInitializationService _initializationService;

    /// <summary>
    /// Initializes a new instance of the <see cref="HealthCheckController"/> class.
    /// </summary>
    /// <param name="collectorService">The collector service.</param>
    /// <param name="dataBufferService">The data buffer service.</param>
    /// <param name="dataRepository">The data repository.</param>
    /// <param name="initializationService">The collector initialization service.</param>
    public HealthCheckController(
        CollectorService collectorService,
        DataBufferService dataBufferService,
        IDataRepository dataRepository,
        CollectorInitializationService initializationService)
    {
        _collectorService = collectorService;
        _dataBufferService = dataBufferService;
        _dataRepository = dataRepository;
        _initializationService = initializationService;
    }

    /// <summary>
    /// Gets the health status of the collector.
    /// </summary>
    /// <returns>A response containing the health status.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(HealthStatus), 200)]
    [ProducesResponseType(500)]
    public ActionResult<HealthStatus> GetHealthStatus()
    {
        try
        {
            var healthStatus = new HealthStatus
            {
                Status = "Healthy", // Default status
                Message = "Collector is running normally",
                LastDataSent = _collectorService.LastDataSent,
                BufferSize = _dataBufferService.GetBufferSize(),
                DatabaseConnected = true, // Default value
                Timestamp = DateTime.UtcNow
            };

            // Determine actual health status based on various checks
            if (_dataBufferService.GetBufferSize() > 1000)
            {
                healthStatus.Status = "Degraded";
                healthStatus.Message = "Buffer size is high, possible connectivity issues";
            }

            // Check if we haven't sent data for a long time
            if (_collectorService.LastDataSent == DateTime.MinValue || 
                (DateTime.UtcNow - _collectorService.LastDataSent).TotalMinutes > 5)
            {
                healthStatus.Status = "Degraded";
                healthStatus.Message = "No data sent in the last 5 minutes";
            }

            // Check if the endpoint was configured
            try
            {
                var config = _initializationService.GetInfluxConfig();
                healthStatus.Endpoint = config.Endpoint;
            }
            catch
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
