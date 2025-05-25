using DataAggregator.Registration.Controllers;
using DataAggregator.Registration.Services;
using DataAggregator.Shared;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace DataAggregator.Registration.Tests;

public class DeviceRegistrationControllerTests
{
    private readonly Mock<IDeviceRegistrationService> _serviceMock;
    private readonly DeviceRegistrationController _controller;

    public DeviceRegistrationControllerTests()
    {
        _serviceMock = new Mock<IDeviceRegistrationService>();
        _controller = new DeviceRegistrationController(_serviceMock.Object);
    }

    [Fact]
    public async Task RegisterDevice_ShouldReturnOk_WhenRequestIsValid()
    {
        // Arrange
        var request = new DeviceRegistrationRequest(new CollectorInfoDto("Device1", "Location1", "http://healthcheck", new List<SensorInfoDto>()));
        var response = new DeviceRegistrationResponse(true, "http://localhost:8086", "GeneratedToken");
        _serviceMock.Setup(service => service.RegisterCollectorAsync(request)).ReturnsAsync(response);

        // Act
        var result = await _controller.RegisterDevice(request);

        // Assert
        Assert.IsType<OkObjectResult>(result);
        var okResult = result as OkObjectResult;
        Assert.NotNull(okResult);
        Assert.Equal(response, okResult!.Value);
    }

    [Fact]
    public async Task RegisterDevice_ShouldReturnBadRequest_WhenRequestIsInvalid()
    {
        // Act
        var result = await _controller.RegisterDevice(null!);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task GetDeviceInfo_ShouldReturnOk_WhenDeviceExists()
    {
        // Arrange
        var deviceInfo = new CollectorInfoDto("Device1", "Location1", "http://healthcheck", new List<SensorInfoDto>());
        _serviceMock.Setup(service => service.GetCollectorInfoAsync("Device1")).ReturnsAsync(deviceInfo);

        // Act
        var result = await _controller.GetDeviceInfo("Device1");

        // Assert
        Assert.IsType<OkObjectResult>(result);
        var okResult = result as OkObjectResult;
        Assert.NotNull(okResult);
        Assert.Equal(deviceInfo, okResult!.Value);
    }

    [Fact]
    public async Task GetDeviceInfo_ShouldReturnNotFound_WhenDeviceDoesNotExist()
    {
        // Arrange
        _serviceMock.Setup(service => service.GetCollectorInfoAsync("Device1")).ReturnsAsync((CollectorInfoDto?)null);

        // Act
        var result = await _controller.GetDeviceInfo("Device1");

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task GetAllDevices_ShouldReturnOk_WithListOfDevices()
    {
        // Arrange
        var devices = new List<CollectorInfoDto>
        {
            new("Device1", "Location1", "http://healthcheck", new List<SensorInfoDto>()),
            new("Device2", "Location2", "http://healthcheck", new List<SensorInfoDto>())
        };
        _serviceMock.Setup(service => service.GetAllCollectorInfoAsync()).ReturnsAsync(devices);

        // Act
        var result = await _controller.GetAllDevices();

        // Assert
        Assert.IsType<OkObjectResult>(result);
        var okResult = result as OkObjectResult;
        Assert.NotNull(okResult);
        Assert.Equal(devices, okResult!.Value);
    }
}
