using System.Net;
using System.Net.Http.Json;
using DataAggregator.Collector.Shared.Abstraction.Configuration;
using DataAggregator.Collector.Shared.Registration;
using DataAggregator.Shared;
using DataAggregator.Shared.Domain.DataType;
using Moq;
using Moq.Protected;

namespace DataAggregator.Collector.Tests;

public class RegistrationServiceTests
{
    private static CollectorConfiguration CreateConfig() => new()
    {
        DeviceName = "dev1",
        Location = "loc",
        HealthCheckEndpoint = "http://localhost/health",
        Sensors =
        [
            new() { Name = "s1", Type = "t1", Unit = "u1", DataType = SensorDataType.Integer, Metadata = [] }
        ]
    };

    [Fact]
    public async Task RegisterCollectorAsync_ShouldReturnSuccess_WhenResponseIsOk()
    {
        var responseObj = new DeviceRegistrationResponse(true, "endpoint", "token");
        var handler = new Mock<HttpMessageHandler>();
        handler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = JsonContent.Create(responseObj)
            });
        var httpClient = new HttpClient(handler.Object);
        var service = new RegistrationService(httpClient, "http://test/register");

        DeviceRegistrationResponse result = await service.RegisterCollectorAsync(CreateConfig());

        Assert.True(result.IsSuccess);
        Assert.Equal("endpoint", result.AssignedTimeSeriesEndpoint);
        Assert.Equal("token", result.DeviceToken);
    }

    [Fact]
    public async Task RegisterCollectorAsync_ShouldReturnFailure_WhenStatusCodeNotOk()
    {
        var handler = new Mock<HttpMessageHandler>();
        handler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.BadRequest));
        var httpClient = new HttpClient(handler.Object);
        var service = new RegistrationService(httpClient, "http://test/register");

        DeviceRegistrationResponse result = await service.RegisterCollectorAsync(CreateConfig());

        Assert.False(result.IsSuccess);
    }

    [Fact]
    public async Task RegisterCollectorAsync_ShouldReturnFailure_WhenDeserializationFails()
    {
        var handler = new Mock<HttpMessageHandler>();
        handler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("null")
            });
        var httpClient = new HttpClient(handler.Object);
        var service = new RegistrationService(httpClient, "http://test/register");

        DeviceRegistrationResponse result = await service.RegisterCollectorAsync(CreateConfig());

        Assert.False(result.IsSuccess);
    }

    [Fact]
    public async Task RegisterCollectorAsync_ShouldReturnFailure_OnException()
    {
        var handler = new Mock<HttpMessageHandler>();
        handler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ThrowsAsync(new Exception("fail"));
        var httpClient = new HttpClient(handler.Object);
        var service = new RegistrationService(httpClient, "http://test/register");

        DeviceRegistrationResponse result = await service.RegisterCollectorAsync(CreateConfig());

        Assert.False(result.IsSuccess);
    }
}
