using DataAggregator.Registration.DeviceManagement.Domain;
using DataAggregator.Registration.DeviceManagement.Persistence.Repositories;
using DataAggregator.Registration.DeviceManagement.Services;
using DataAggregator.Registration.InfluxService.Services;
using DataAggregator.Shared;
using DataAggregator.Shared.Configuration.TimeSeries;
using DataAggregator.Shared.Domain;
using DataAggregator.Shared.DTOs;
using Moq;

namespace DataAggregator.Registration.Tests;

/// <summary>
/// Tests for the <see cref="DeviceRegistrationService"/> class.
/// </summary>
public class DeviceRegistrationServiceTests
{
    private readonly Mock<IDeviceRepository> _deviceRepositoryMock;
    private readonly Mock<IInfluxEndpointProviderService> _influxEndpointProviderServiceMock;
    private readonly DeviceRegistrationService _service;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeviceRegistrationServiceTests"/> class.
    /// </summary>
    public DeviceRegistrationServiceTests()
    {
        _deviceRepositoryMock = new Mock<IDeviceRepository>();
        _influxEndpointProviderServiceMock = new Mock<IInfluxEndpointProviderService>();

        _service = new DeviceRegistrationService(_deviceRepositoryMock.Object, _influxEndpointProviderServiceMock.Object);
    }

    /// <summary>
    /// Tests the RegisterCollectorAsync method of the DeviceRegistrationService.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
    [Fact]
    public async Task RegisterCollectorAsync_ShouldReturnSuccess_WhenDeviceIsNew()
    {
        // Arrange
        var mockEndpoint = new InfluxEndpoint("MockEndpoint", "http://mock-endpoint", "mock-token");
        _influxEndpointProviderServiceMock.Setup(service => service.GetAvailableEndpointAsync()).ReturnsAsync(mockEndpoint);

        var request = new DeviceRegistrationRequest("Device1", "Location1", "http://healthcheck", []);
        _deviceRepositoryMock.Setup(repo => repo.GetByNameAsync(request.DeviceName)).ReturnsAsync((Collector?)null);

        // Act
        DeviceRegistrationResponse result = await _service.RegisterCollectorAsync(request);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("http://mock-endpoint", result.AssignedTimeSeriesEndpoint);
        _deviceRepositoryMock.Verify(repo => repo.CreateAsync(It.IsAny<Collector>()), Times.Once);
    }

    /// <summary>
    /// Tests the RegisterCollectorAsync method of the DeviceRegistrationService when the device already exists and the endpoint is healthy.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
    [Fact]
    public async Task RegisterCollectorAsync_ShouldReturnFailure_WhenDeviceAlreadyExistsAndEndpointIsHealthy()
    {
        // Arrange
        var mockEndpoint = new InfluxEndpoint("MockEndpoint", "http://mock-endpoint", "mock-token");
        var existingDevice = new Collector { DeviceName = "Device1", AssignedInfluxEndpoint = mockEndpoint };
        var request = new DeviceRegistrationRequest("Device1", "Location1", "http://healthcheck", []);
        _deviceRepositoryMock.Setup(repo => repo.GetByNameAsync(request.DeviceName)).ReturnsAsync(existingDevice);
        _influxEndpointProviderServiceMock.Setup(service => service.CheckEndPointValidityAsync(mockEndpoint)).ReturnsAsync(true);

        // Act
        DeviceRegistrationResponse result = await _service.RegisterCollectorAsync(request);

        // Assert
        Assert.True(result.IsSuccess);
        _deviceRepositoryMock.Verify(repo => repo.CreateAsync(It.IsAny<Collector>()), Times.Never);
    }

    /// <summary>
    /// Tests the RegisterCollectorAsync method of the DeviceRegistrationService.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
    [Fact]
    public async Task RegisterCollectorAsync_ShouldUpdateEndpointHistory_WhenEndpointIsNonFunctional()
    {
        // Arrange
        var mockEndpoint = new InfluxEndpoint("MockEndpoint", "http://mock-endpoint", "mock-token");
        var newMockEndpoint = new InfluxEndpoint("NewMockEndpoint", "http://new-mock-endpoint", "new-mock-token");

        var existingDevice = new Collector
        {
            DeviceName = "Device1",
            AssignedInfluxEndpoint = mockEndpoint,
            RegistrationDate = DateTime.UtcNow.AddMonths(-1),
        };

        var request = new DeviceRegistrationRequest("Device1", "Location1", "http://healthcheck", []);

        _deviceRepositoryMock.Setup(repo => repo.GetByNameAsync(request.DeviceName)).ReturnsAsync(existingDevice);
        _deviceRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<Collector>())).Verifiable();
        _influxEndpointProviderServiceMock.Setup(service => service.CheckEndPointValidityAsync(mockEndpoint)).ReturnsAsync(false);
        _influxEndpointProviderServiceMock.Setup(service => service.GetAvailableEndpointAsync()).ReturnsAsync(newMockEndpoint);

        // Act
        DeviceRegistrationResponse result = await _service.RegisterCollectorAsync(request);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("http://new-mock-endpoint", result.AssignedTimeSeriesEndpoint);
        Assert.Equal("new-mock-token", result.DeviceToken);
        Assert.Single(existingDevice.EndpointHistories);
        Assert.Equal(mockEndpoint.Endpoint, existingDevice.EndpointHistories[0].Endpoint.Endpoint);
        _deviceRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<Collector>()), Times.Once);
    }

    /// <summary>
    /// Tests the GetCollectorInfoAsync method of the DeviceRegistrationService to ensure it returns device information.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
    [Fact]
    public async Task GetCollectorInfoAsync_ShouldReturnDeviceInfo_WhenDeviceExists()
    {
        var mockEndpoint = new InfluxEndpoint("Name", "Endpoint", "token");

        // Arrange
        var device = new Collector
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
        CollectorInfoDto? result = await _service.GetCollectorInfoAsync("Device1");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Device1", result!.DeviceName);
        Assert.Single(result.Sensors);
    }

    /// <summary>
    /// Tests the GetCollectorInfoAsync method of the DeviceRegistrationService to ensure it includes endpoint history.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
    [Fact]
    public async Task GetCollectorInfoAsync_ShouldIncludeEndpointHistory()
    {
        var mockEndpoint = new InfluxEndpoint("Name", "Endpoint", "token");

        // Arrange
        var device = new Collector
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
        CollectorInfoDto? result = await _service.GetCollectorInfoAsync("Device1");

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result!.EndpointHistories.Count);
    }

    /// <summary>
    /// Tests the GetCollectorInfoAsync method of the DeviceRegistrationService to ensure it returns null when the device does not exist.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
    [Fact]
    public async Task GetCollectorInfoAsync_ShouldReturnNull_WhenDeviceDoesNotExist()
    {
        // Arrange
        _deviceRepositoryMock.Setup(repo => repo.GetByNameAsync("Device1")).ReturnsAsync((Collector?)null);

        // Act
        CollectorInfoDto? result = await _service.GetCollectorInfoAsync("Device1");

        // Assert
        Assert.Null(result);
    }

    /// <summary>
    /// Tests the GetAllCollectorInfoAsync method of the DeviceRegistrationService to ensure it returns all registered devices.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
    [Fact]
    public async Task GetAllCollectorInfoAsync_ShouldReturnAllDevices()
    {
        var mockEnpoint = new InfluxEndpoint("default", "http://localhost:8086", "token");

        // Arrange
        var devices = new List<Collector>
        {
            new() { DeviceName = "Device1", AssignedInfluxEndpoint = mockEnpoint, Location = "Location1", HealthCheckEndpoint = "http://healthcheck", Sensors = [] },
            new() { DeviceName = "Device2", AssignedInfluxEndpoint = mockEnpoint, Location = "Location2", HealthCheckEndpoint = "http://healthcheck", Sensors = [] },
        };
        _deviceRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(devices);

        // Act
        IEnumerable<CollectorInfoDto> result = await _service.GetAllCollectorInfoAsync();

        // Assert
        Assert.Equal(2, result.Count());
        Assert.Contains(result, d => d.DeviceName == "Device1");
        Assert.Contains(result, d => d.DeviceName == "Device2");
    }
}
