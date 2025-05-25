using DataAggregator.Registration.Services;
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
    public Task<IActionResult> RegisterDevice([FromBody] DeviceRegistrationRequest request) => throw new NotImplementedException();

    /// <summary>
    /// Retrieves information about a specific device by its name.
    /// </summary>
    /// <param name="deviceName">The name of the device.</param>
    /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
    [HttpGet("{deviceName}")]
    public Task<IActionResult> GetDeviceInfo(string deviceName) => throw new NotImplementedException();

    /// <summary>
    /// Retrieves a list of all registered devices.
    /// </summary>
    /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
    [HttpGet]
    public Task<IActionResult> GetAllDevices() => throw new NotImplementedException();
}
