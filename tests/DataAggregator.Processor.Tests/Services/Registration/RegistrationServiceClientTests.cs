using System.Net;
using System.Text.Json;
using DataAggregator.Processor.Services.Registration;
using DataAggregator.Shared.Configuration.TimeSeries;
using DataAggregator.Shared.Domain.DataType;
using DataAggregator.Shared.DTOs;
using Moq;
using Moq.Protected;

namespace DataAggregator.Processor.Tests.Services.Registration;

/// <summary>
/// Tests for the <see cref="RegistrationServiceClient"/> class.
/// </summary>
public class RegistrationServiceClientTests
{
    private readonly Mock<HttpMessageHandler> _mockHttpHandler;
    private readonly RegistrationServiceClient _client;

    /// <summary>
    /// Initializes a new instance of the <see cref="RegistrationServiceClientTests"/> class.
    /// </summary>
    public RegistrationServiceClientTests()
    {
        _mockHttpHandler = new Mock<HttpMessageHandler>();
        var httpClient = new HttpClient(_mockHttpHandler.Object)
        {
            BaseAddress = new Uri("http://localhost:5000"),
        };
        _client = new RegistrationServiceClient(httpClient);
    }

    #region GetCollectorInfoAsync tests

    [Fact]
    public async Task GetCollectorInfoAsync_ShouldReturnCollectorInfo_WhenValidResponseReceived()
    {
        // Arrange
        CollectorInfoDto expectedCollectorInfo = CreateTestCollectorInfo();
        string responseContent = JsonSerializer.Serialize(expectedCollectorInfo);

        _mockHttpHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(responseContent),
            });

        // Act
        CollectorInfoDto? result = await _client.GetCollectorInfoAsync("test_device");

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedCollectorInfo.DeviceName, result.DeviceName);
        Assert.Equal(expectedCollectorInfo.AssignedInfluxEndpoint.Endpoint, result.AssignedInfluxEndpoint.Endpoint);
        Assert.Equal(expectedCollectorInfo.Sensors.Count, result.Sensors.Count);
    }

    [Fact]
    public async Task GetCollectorInfoAsync_ShouldReturnNull_WhenNotFoundResponseReceived()
    {
        // Arrange
        _mockHttpHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.NotFound));

        // Act
        CollectorInfoDto? result = await _client.GetCollectorInfoAsync("non_existent_device");

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetCollectorInfoAsync_ShouldThrowArgumentException_WhenDeviceNameIsEmpty() =>

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _client.GetCollectorInfoAsync(string.Empty));

    [Fact]
    public async Task GetCollectorInfoAsync_ShouldHandleEmptyResponseContent_WhenReceived()
    {
        // Arrange
        _mockHttpHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(string.Empty),
            });

        // Act
        CollectorInfoDto? result = await _client.GetCollectorInfoAsync("test_device");

        // Assert
        Assert.Null(result);
    }

    #endregion

    #region Helper methods

    private static CollectorInfoDto CreateTestCollectorInfo() => new(
            "test_device",
            "test_location",
            "http://localhost:5000/health",
            new InfluxEndpoint("TestEndpoint", "http://localhost:8086", "test_token"),
            [
                new("temperature", "Type", "Unit", [], SensorDataType.Float),
                new("pressure", "Type", "Unit", [], SensorDataType.Float)
            ],
            []);

    #endregion
}
