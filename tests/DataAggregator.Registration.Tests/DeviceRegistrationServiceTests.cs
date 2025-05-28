using DataAggregator.Registration.DeviceManagement.Domain;
using DataAggregator.Registration.DeviceManagement.Persistence.Repositories;
using DataAggregator.Registration.DeviceManagement.Services;
using DataAggregator.Registration.Domain;
using DataAggregator.Registration.InfluxService.Services;
using DataAggregator.Shared;
using Moq;

namespace DataAggregator.Registration.Tests;

public class DeviceRegistrationServiceTests
{
    private readonly Mock<IDeviceRepository> _deviceRepositoryMock;
    private readonly Mock<IInfluxEndpointProviderService> _influxEndpointProviderServiceMock;
    private readonly DeviceRegistrationService _service;

    public DeviceRegistrationServiceTests()
    {
        _deviceRepositoryMock = new Mock<IDeviceRepository>();
        _influxEndpointProviderServiceMock = new Mock<IInfluxEndpointProviderService>();

        _service = new DeviceRegistrationService(_deviceRepositoryMock.Object, _influxEndpointProviderServiceMock.Object);
    }

    [Fact]
    public async Task RegisterCollectorAsync_ShouldReturnSuccess_WhenDeviceIsNew()
    {
        // Arrange
        var mockEndpoint = new InfluxEndpoint("MockEndpoint", "http://mock-endpoint", "mock-token");
        _influxEndpointProviderServiceMock.Setup(service => service.GetAvailableEndpointAsync()).ReturnsAsync(mockEndpoint);

        var request = new DeviceRegistrationRequest(new CollectorInfoDto("Device1", "Location1", "http://healthcheck", [], []));
        _deviceRepositoryMock.Setup(repo => repo.GetByNameAsync(request.Config.DeviceName)).ReturnsAsync((Device?)null);

        // Act
        var result = await _service.RegisterCollectorAsync(request);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("http://mock-endpoint", result.AssignedTimeSeriesEndpoint);
        _deviceRepositoryMock.Verify(repo => repo.CreateAsync(It.IsAny<Device>()), Times.Once);
    }

    [Fact]
    public async Task RegisterCollectorAsync_ShouldReturnFailure_WhenDeviceAlreadyExists()
    {
        // Arrange
        var existingDevice = new Device { DeviceName = "Device1", AssignedInfluxEndpoint = new("", "", "") };
        var request = new DeviceRegistrationRequest(new CollectorInfoDto("Device1", "Location1", "http://healthcheck", [], []));
        _deviceRepositoryMock.Setup(repo => repo.GetByNameAsync(request.Config.DeviceName)).ReturnsAsync(existingDevice);

        // Act
        var result = await _service.RegisterCollectorAsync(request);

        // Assert
        Assert.False(result.IsSuccess);
        _deviceRepositoryMock.Verify(repo => repo.CreateAsync(It.IsAny<Device>()), Times.Never);
    }

    [Fact]
    public async Task RegisterCollectorAsync_ShouldUpdateEndpointHistory_WhenEndpointIsNonFunctional()
    {
        var mockEndpoint = new InfluxEndpoint("Name", "Endpoint", "token");

        // Arrange
        var existingDevice = new Device
        {
            DeviceName = "Device1",
            AssignedInfluxEndpoint = mockEndpoint,
            RegistrationDate = DateTime.UtcNow.AddMonths(-1),
        };

        var request = new DeviceRegistrationRequest(new CollectorInfoDto("Device1", "Location1", "http://healthcheck", new List<SensorInfoDto>(), new List<EndpointHistory>()));

        _deviceRepositoryMock.Setup(repo => repo.GetByNameAsync(request.Config.DeviceName)).ReturnsAsync(existingDevice);
        _deviceRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<Device>())).Verifiable();

        // Act
        var result = await _service.RegisterCollectorAsync(request);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotEqual("http://old-endpoint", result.AssignedTimeSeriesEndpoint);
        Assert.Equal("default-token", result.DeviceToken);
        _deviceRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<Device>()), Times.Once);
    }

    [Fact]
    public async Task GetCollectorInfoAsync_ShouldReturnDeviceInfo_WhenDeviceExists()
    {
        var mockEndpoint = new InfluxEndpoint("Name", "Endpoint", "token");

        // Arrange
        var device = new Device
        {
            DeviceName = "Device1",
            Location = "Location1",
            HealthCheckEndpoint = "http://healthcheck",
            AssignedInfluxEndpoint = mockEndpoint,
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
    public async Task GetCollectorInfoAsync_ShouldIncludeEndpointHistory()
    {
        var mockEndpoint = new InfluxEndpoint("Name", "Endpoint", "token");

        // Arrange
        var device = new Device
        {
            DeviceName = "Device1",
            Location = "Location1",
            HealthCheckEndpoint = "http://healthcheck",
            Sensors = [],
            AssignedInfluxEndpoint = mockEndpoint,
            EndpointHistories =
            [
                new EndpointHistory(mockEndpoint, DateTime.UtcNow.AddMonths(-1), DateTime.UtcNow.AddDays(-1)),
                new EndpointHistory(mockEndpoint, DateTime.UtcNow.AddDays(-1), DateTime.UtcNow.AddDays(1)),
            ],
        };

        _deviceRepositoryMock.Setup(repo => repo.GetByNameAsync("Device1")).ReturnsAsync(device);

        // Act
        var result = await _service.GetCollectorInfoAsync("Device1");

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result!.EndpointHistories.Count);
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
        InfluxEndpoint mockEnpoint = new InfluxEndpoint("default", "http://localhost:8086", "token");

        // Arrange
        var devices = new List<Device>
        {
            new Device { DeviceName = "Device1", AssignedInfluxEndpoint = mockEnpoint, Location = "Location1", HealthCheckEndpoint = "http://healthcheck", Sensors = new List<Sensor>() },
            new Device { DeviceName = "Device2", AssignedInfluxEndpoint = mockEnpoint, Location = "Location2", HealthCheckEndpoint = "http://healthcheck", Sensors = new List<Sensor>() },
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
