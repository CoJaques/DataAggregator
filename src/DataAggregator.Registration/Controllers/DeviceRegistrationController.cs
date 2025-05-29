using DataAggregator.Registration.DeviceManagement.Services;
using DataAggregator.Shared;
using Microsoft.AspNetCore.Mvc;

namespace DataAggregator.Registration.Controllers;

/// <summary>
/// Controller for handling device registration-related API requests.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="DeviceRegistrationController"/> class.
/// </remarks>
/// <param name="deviceRegistrationService">The service for handling device registration operations.</param>
[ApiController]
[Route("api/[controller]")]
public class DeviceRegistrationController(IDeviceRegistrationService deviceRegistrationService) : ControllerBase
{
    private readonly IDeviceRegistrationService _deviceRegistrationService = deviceRegistrationService;

    /// <summary>
    /// Registers a new device.
    /// </summary>
    /// <param name="request">The device registration request containing configuration details.</param>
    /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
    [HttpPost("register")]
    [ProducesResponseType(typeof(DeviceRegistrationResponse), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> RegisterDevice([FromBody] DeviceRegistrationRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.DeviceName) || string.IsNullOrWhiteSpace(request.HealthCheckEndpoint))
        {
            return BadRequest("Invalid request payload.");
        }

        try
        {
            DeviceRegistrationResponse response = await _deviceRegistrationService.RegisterCollectorAsync(request);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An unexpected error occurred: {ex.Message}");
        }
    }

    /// <summary>
    /// Retrieves information about a specific device by its name.
    /// </summary>
    /// <param name="deviceName">The name of the device.</param>
    /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
    [HttpGet("{deviceName}")]
    [ProducesResponseType(typeof(CollectorInfoDto), 200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> GetDeviceInfo(string deviceName)
    {
        if (string.IsNullOrWhiteSpace(deviceName))
        {
            return BadRequest("Device name cannot be null or empty.");
        }

        try
        {
            CollectorInfoDto? deviceInfo = await _deviceRegistrationService.GetCollectorInfoAsync(deviceName);
            return deviceInfo == null ? NotFound($"Device with name '{deviceName}' not found.") : Ok(deviceInfo);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An unexpected error occurred: {ex.Message}");
        }
    }

    /// <summary>
    /// Retrieves a list of all registered devices.
    /// </summary>
    /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<CollectorInfoDto>), 200)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> GetAllDevices()
    {
        try
        {
            IEnumerable<CollectorInfoDto> devices = await _deviceRegistrationService.GetAllCollectorInfoAsync();
            return Ok(devices);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An unexpected error occurred: {ex.Message}");
        }
    }
}
