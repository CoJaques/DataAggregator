using DataAggregator.Processor.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace DataAggregator.Processor.Tests.Controllers;

/// <summary>
/// Tests for the <see cref="HealthCheckController"/> class.
/// </summary>
public class HealthCheckControllerTests
{
    private readonly HealthCheckController _controller;

    /// <summary>
    /// Initializes a new instance of the <see cref="HealthCheckControllerTests"/> class.
    /// </summary>
    public HealthCheckControllerTests() => _controller = new HealthCheckController();

    [Fact]
    public void Get_ShouldReturnOkResult_WhenCalled()
    {
        // Act
        IActionResult result = _controller.Get();

        // Assert
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public void Get_ShouldReturnCorrectStatus_WhenCalled()
    {
        // Act
        IActionResult result = _controller.Get();
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = okResult.Value as dynamic;

        // Assert
        Assert.NotNull(response);
        Assert.Equal("Healthy", response.Status);
        Assert.Equal("DataAggregator.Processor", response.Service);
        Assert.NotNull(response.Timestamp);
    }

    [Fact]
    public void Get_ShouldReturnCurrentTimestamp_WhenCalled()
    {
        // Arrange
        DateTime beforeCall = DateTime.UtcNow;

        // Act
        IActionResult result = _controller.Get();
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = okResult.Value as dynamic;

        DateTime afterCall = DateTime.UtcNow;

        // Assert
        Assert.NotNull(response);
        var timestamp = (DateTime)response.Timestamp;
        Assert.True(timestamp >= beforeCall && timestamp <= afterCall);
    }

    [Fact]
    public void Get_ShouldReturnValidJsonStructure_WhenCalled()
    {
        // Act
        IActionResult result = _controller.Get();
        var okResult = Assert.IsType<OkObjectResult>(result);

        // Assert
        Assert.Equal(200, okResult.StatusCode);
        Assert.NotNull(okResult.Value);
    }
} 