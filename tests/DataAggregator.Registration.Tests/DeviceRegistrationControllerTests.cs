using DataAggregator.Registration.Controllers;
using DataAggregator.Registration.DeviceManagement.Services;
using DataAggregator.Shared;
using DataAggregator.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace DataAggregator.Registration.Tests;

/// <summary>
/// Tests for the <see cref="DeviceRegistrationController"/> class.
/// </summary>
public class DeviceRegistrationControllerTests
{
    private readonly Mock<IDeviceRegistrationService> _serviceMock;
    private readonly DeviceRegistrationController _controller;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeviceRegistrationControllerTests"/> class.
    /// </summary>
    public DeviceRegistrationControllerTests()
    {
        _serviceMock = new Mock<IDeviceRegistrationService>();
        _controller = new DeviceRegistrationController(_serviceMock.Object);
    }

    /// <summary>
    /// Tests the RegisterDevice method of the DeviceRegistrationController.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
    [Fact]
    public async Task RegisterDevice_ShouldReturnOk_WhenRequestIsValid()
    {
        // Arrange
        var request = new DeviceRegistrationRequest("Device1", "Location1", "http://healthcheck", []);
        var response = new DeviceRegistrationResponse(true, "http://localhost:8086", "GeneratedToken");
        _serviceMock.Setup(service => service.RegisterCollectorAsync(request)).ReturnsAsync(response);

        // Act
        IActionResult result = await _controller.RegisterDevice(request);

        // Assert
        Assert.IsType<OkObjectResult>(result);
        var okResult = result as OkObjectResult;
        Assert.NotNull(okResult);
        Assert.Equal(response, okResult!.Value);
    }

    /// <summary>
    /// Tests the RegisterDevice method of the DeviceRegistrationController when the request is invalid.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
    [Fact]
    public async Task RegisterDevice_ShouldReturnBadRequest_WhenRequestIsInvalid()
    {
        var request = new DeviceRegistrationRequest(" ", "Location1", string.Empty, []);

        // Act
        IActionResult result = await _controller.RegisterDevice(request);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    /// <summary>
    /// Tests the GetDeviceInfo method of the DeviceRegistrationController.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
    [Fact]
    public async Task GetDeviceInfo_ShouldReturnOk_WhenDeviceExists()
    {
        // Arrange
        var deviceInfo = new CollectorInfoDto("Device1", "Location1", "http://healthcheck", new("name", "endpoint", "token"), [], []);
        _serviceMock.Setup(service => service.GetCollectorInfoAsync("Device1")).ReturnsAsync(deviceInfo);

        // Act
        IActionResult result = await _controller.GetDeviceInfo("Device1");

        // Assert
        Assert.IsType<OkObjectResult>(result);
        var okResult = result as OkObjectResult;
        Assert.NotNull(okResult);
        Assert.Equal(deviceInfo, okResult!.Value);
    }

    /// <summary>
    /// Tests the GetDeviceInfo method of the DeviceRegistrationController when the device does not exist.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
    [Fact]
    public async Task GetDeviceInfo_ShouldReturnNotFound_WhenDeviceDoesNotExist()
    {
        // Arrange
        _serviceMock.Setup(service => service.GetCollectorInfoAsync("Device1")).ReturnsAsync((CollectorInfoDto?)null);

        // Act
        IActionResult result = await _controller.GetDeviceInfo("Device1");

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
    }

    /// <summary>
    /// Tests the GetAllDevices method of the DeviceRegistrationController.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
    [Fact]
    public async Task GetAllDevices_ShouldReturnOk_WithListOfDevices()
    {
        // Arrange
        var devices = new List<CollectorInfoDto>
        {
            new("Device1", "Location1", "http://healthcheck", new(string.Empty, string.Empty, string.Empty), [], []),
            new("Device2", "Location2", "http://healthcheck", new(string.Empty, string.Empty, string.Empty), [], []),
        };
        _serviceMock.Setup(service => service.GetAllCollectorInfoAsync()).ReturnsAsync(devices);

        // Act
        IActionResult result = await _controller.GetAllDevices();

        // Assert
        Assert.IsType<OkObjectResult>(result);
        var okResult = result as OkObjectResult;
        Assert.NotNull(okResult);
        Assert.Equal(devices, okResult!.Value);
    }
}
