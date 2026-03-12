using DataAggregator.Collector.HttpCollector.Configuration;
using DataAggregator.Collector.HttpCollector.Connector;
using DataAggregator.Collector.HttpCollector.Controllers;
using DataAggregator.Collector.HttpCollector.Models;
using DataAggregator.Collector.Shared.Models;
using DataAggregator.Shared.Domain.DataType;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace DataAggregator.Collector.Tests;

public class HttpCollectorTests
{
    private readonly HttpConnector _connector;
    private readonly HttpCollectorController _controller;
    private readonly HttpCollectorConfiguration _configuration;

    public HttpCollectorTests()
    {
        _configuration = new HttpCollectorConfiguration { DeviceName = "TestHttpDevice" };
        var options = Options.Create(_configuration);
        _connector = new HttpConnector(_configuration);
        _connector.ConnectAsync().Wait();
        _controller = new HttpCollectorController(_connector, options);
    }

    [Fact]
    public async Task HttpConnector_ShouldEnqueueAndFetchData()
    {
        // Arrange
        var timestamp = DateTime.UtcNow;
        var measurements = new List<IMeasurementData>
        {
            new MeasurementData<double>(timestamp, "sensor1", 12.3)
        };

        // Act
        _connector.TryEnqueueData(measurements);
        var fetchedData = await _connector.FetchDataAsync();

        // Assert
        Assert.Single(fetchedData);
        var first = fetchedData.First();
        Assert.Equal("sensor1", first.SensorName);
        Assert.Equal(12.3, (double)first.GetRawValue());
        Assert.Equal(timestamp, first.TimeStamp);
    }

    [Fact]
    public async Task HttpConnector_ShouldNotAcceptDataOnDisconnect()
    {
        // Arrange
        var measurements = new[] { new MeasurementData<double>(DateTime.UtcNow, "s1", 1.0) };

        // Act
        await _connector.DisconnectAsync();
        bool accepted = _connector.TryEnqueueData(measurements);
        var data = await _connector.FetchDataAsync();

        // Assert
        Assert.False(accepted);
        Assert.Empty(data);
    }

    [Fact]
    public async Task HttpCollectorController_ShouldHandleMultipleDataTypes()
    {
        // Arrange
        var timestamp = DateTime.UtcNow;
        var dtos = new List<HttpMeasurementDto>
        {
            new() { SensorName = "bool", TimeStamp = timestamp, Value = true, DataType = SensorDataType.Boolean },
            new() { SensorName = "int", TimeStamp = timestamp, Value = 42, DataType = SensorDataType.Integer },
            new() { SensorName = "double", TimeStamp = timestamp, Value = 3.14, DataType = SensorDataType.Double },
            new() { SensorName = "string", TimeStamp = timestamp, Value = "hello", DataType = SensorDataType.String }
        };

        // Act
        var result = _controller.CollectData(dtos);

        // Assert
        Assert.IsType<OkResult>(result);
        var fetched = (await _connector.FetchDataAsync()).ToList();
        Assert.Equal(4, fetched.Count);

        Assert.Equal(true, (bool)fetched.First(m => m.SensorName == "bool").GetRawValue());
        Assert.Equal(42, (int)fetched.First(m => m.SensorName == "int").GetRawValue());
        Assert.Equal(3.14, (double)fetched.First(m => m.SensorName == "double").GetRawValue());
        Assert.Equal("hello", (string)fetched.First(m => m.SensorName == "string").GetRawValue());
    }

    [Fact]
    public void HttpCollectorController_ShouldReturnBadRequest_WhenMeasurementsNull()
    {
        // Act
        var result = _controller.CollectData(null!);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public void HttpCollectorController_ShouldReturnBadRequest_WhenConnectorIsNull()
    {
        // Arrange
        var controller = new HttpCollectorController(null, null);

        // Act
        var result = controller.CollectData(new List<HttpMeasurementDto>());

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }
}
