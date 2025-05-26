using DataAggregator.Registration.Domain;
using DataAggregator.Registration.Repositories;
using DataAggregator.Registration.Services;
using DataAggregator.Shared;
using Microsoft.Extensions.Options;
using Moq;

namespace DataAggregator.Registration.Tests;

public class DeviceRegistrationServiceTests
{
    private readonly Mock<IDeviceRepository> _deviceRepositoryMock;
    private readonly DeviceRegistrationService _service;

    public DeviceRegistrationServiceTests()
    {
        _deviceRepositoryMock = new Mock<IDeviceRepository>();

        var influxEndpoints = new List<InfluxEndpointConfiguration>
        {
            new("DefaultEndpoint", "http://localhost:8086", "default-token", true)
        };

        _service = new DeviceRegistrationService(_deviceRepositoryMock.Object, Options.Create(influxEndpoints));
    }

    [Fact]
    public async Task RegisterCollectorAsync_ShouldReturnSuccess_WhenDeviceIsNew()
    {
        // Arrange
        var request = new DeviceRegistrationRequest(new CollectorInfoDto("Device1", "Location1", "http://healthcheck", new List<SensorInfoDto>()));
        _deviceRepositoryMock.Setup(repo => repo.GetByNameAsync(request.Config.DeviceName)).ReturnsAsync((Device?)null);

        // Act
        var result = await _service.RegisterCollectorAsync(request);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("http://localhost:8086", result.AssignedTimeSeriesEndpoint);
        _deviceRepositoryMock.Verify(repo => repo.CreateAsync(It.IsAny<Device>()), Times.Once);
    }

    [Fact]
    public async Task RegisterCollectorAsync_ShouldReturnFailure_WhenDeviceAlreadyExists()
    {
        // Arrange
        var existingDevice = new Device { DeviceName = "Device1" };
        var request = new DeviceRegistrationRequest(new CollectorInfoDto("Device1", "Location1", "http://healthcheck", new List<SensorInfoDto>()));
        _deviceRepositoryMock.Setup(repo => repo.GetByNameAsync(request.Config.DeviceName)).ReturnsAsync(existingDevice);

        // Act
        var result = await _service.RegisterCollectorAsync(request);

        // Assert
        Assert.False(result.IsSuccess);
        _deviceRepositoryMock.Verify(repo => repo.CreateAsync(It.IsAny<Device>()), Times.Never);
    }

    [Fact]
    public async Task GetCollectorInfoAsync_ShouldReturnDeviceInfo_WhenDeviceExists()
    {
        // Arrange
        var device = new Device
        {
            DeviceName = "Device1",
            Location = "Location1",
            HealthCheckEndpoint = "http://healthcheck",
        };

        var sensors = new List<Sensor>
        {
            new() { SensorName = "Sensor1", SensorType = "Type1", Unit = "Unit1", Device = device },
        };

        device.Sensors = sensors;

        _deviceRepositoryMock.Setup(repo => repo.GetByNameAsync("Device1")).ReturnsAsync(device);

        // Act
        var result = await _service.GetCollectorInfoAsync("Device1");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Device1", result!.DeviceName);
        Assert.Single(result.Sensors);
    }

    [Fact]
    public async Task GetCollectorInfoAsync_ShouldReturnNull_WhenDeviceDoesNotExist()
    {
        // Arrange
        _deviceRepositoryMock.Setup(repo => repo.GetByNameAsync("Device1")).ReturnsAsync((Device?)null);

        // Act
        var result = await _service.GetCollectorInfoAsync("Device1");

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetAllCollectorInfoAsync_ShouldReturnAllDevices()
    {
        // Arrange
        var devices = new List<Device>
        {
            new Device { DeviceName = "Device1", Location = "Location1", HealthCheckEndpoint = "http://healthcheck", Sensors = new List<Sensor>() },
            new Device { DeviceName = "Device2", Location = "Location2", HealthCheckEndpoint = "http://healthcheck", Sensors = new List<Sensor>() }
        };
        _deviceRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(devices);

        // Act
        var result = await _service.GetAllCollectorInfoAsync();

        // Assert
        Assert.Equal(2, result.Count());
        Assert.Contains(result, d => d.DeviceName == "Device1");
        Assert.Contains(result, d => d.DeviceName == "Device2");
    }
}
