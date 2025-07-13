using Microsoft.AspNetCore.Mvc;

namespace DataAggregator.Processor.Controllers;

/// <summary>
/// Controller for health check endpoints.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class HealthCheckController : ControllerBase
{
    /// <summary>
    /// Gets the health status of the prediction service.
    /// </summary>
    /// <returns>The health status.</returns>
    [HttpGet]
    public IActionResult Get()
        => Ok(new { Status = "Healthy", Service = "DataAggregator.Processor", Timestamp = DateTime.UtcNow });
}
