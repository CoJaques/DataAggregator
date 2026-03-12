using System.Net;
using System.Net.Http.Json;
using DataAggregator.Collector.HttpCollector.Configuration;
using DataAggregator.Collector.HttpCollector.Models;
using DataAggregator.Collector.Shared.Abstraction.Configuration;
using DataAggregator.Collector.Shared.DataStorage;
using DataAggregator.Collector.Shared.Models;
using DataAggregator.Shared.Domain.DataType;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Moq;

namespace DataAggregator.Collector.Tests;

public class HttpCollectorIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public HttpCollectorIntegrationTests(WebApplicationFactory<Program> factory) => _factory = factory;

    [Fact]
    public async Task HttpCollector_Integration_Simulation()
    {
        // Arrange
        var dataRepoMock = new Mock<IDataRepository>();
        dataRepoMock.Setup(x => x.InitializeAsync()).Returns(Task.CompletedTask);
        dataRepoMock.Setup(x => x.IsConnectedAsync()).ReturnsAsync(true);

        var capturedData = new List<IMeasurementData>();
        dataRepoMock.Setup(x => x.BulkInsertAsync(It.IsAny<IEnumerable<IMeasurementData>>(), It.IsAny<CollectorConfiguration>()))
            .Callback<IEnumerable<IMeasurementData>, CollectorConfiguration>((data, config) => capturedData.AddRange(data))
            .ReturnsAsync(true);

        using var client = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                services.AddSingleton(dataRepoMock.Object);

                // Override the configuration directly to ensure sensors are defined
                var config = new HttpCollectorConfiguration
                {
                    DeviceName = "MockMachine",
                    FlushIntervalMilliseconds = 100,
                    Sensors = new List<SensorConfig>
                    {
                        new() { Name = "temp", DataType = SensorDataType.Double },
                        new() { Name = "vibration", DataType = SensorDataType.Double }
                    }
                };
                services.AddSingleton<IOptions<HttpCollectorConfiguration>>(Options.Create(config));
                services.AddSingleton<IOptions<CollectorConfiguration>>(Options.Create((CollectorConfiguration)config));
            });

            builder.UseSetting("CollectorType", "HTTP");
        }).CreateClient();

        // Simulate a machine sending data
        var machineData = new List<HttpMeasurementDto>
        {
            new() { SensorName = "temp", Value = 25.5, DataType = SensorDataType.Double, TimeStamp = DateTime.UtcNow },
            new() { SensorName = "vibration", Value = 0.12, DataType = SensorDataType.Double, TimeStamp = DateTime.UtcNow }
        };

        // Act
        var response = await client.PostAsJsonAsync("/api/HttpCollector/collect", machineData);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        await Task.Delay(500);

        // Verify data reached the repository mock
        Assert.NotEmpty(capturedData);
        Assert.Contains(capturedData, m => m.SensorName == "temp" && (double)m.GetRawValue() == 25.5);
        Assert.Contains(capturedData, m => m.SensorName == "vibration" && (double)m.GetRawValue() == 0.12);
    }
}
