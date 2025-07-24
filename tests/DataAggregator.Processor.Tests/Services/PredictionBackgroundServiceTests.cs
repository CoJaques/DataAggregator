using DataAggregator.Processor.Configuration;
using DataAggregator.Processor.Services;
using DataAggregator.Processor.Services.Prediction;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Moq;

namespace DataAggregator.Processor.Tests.Services;

/// <summary>
/// Tests for the <see cref="PredictionBackgroundService"/> class.
/// </summary>
public class PredictionBackgroundServiceTests : IDisposable
{
    private readonly Mock<IOptions<PredictionServiceConfiguration>> _mockConfiguration;
    private readonly Mock<IMachinePredictionProcessor> _mockPredictionProcessor;
    private readonly Mock<IServiceProvider> _serviceProvider;
    private readonly Mock<IServiceScope> _mockScope;
    private readonly Mock<IServiceScopeFactory> _mockScopeFactory;
    private readonly CancellationTokenSource _cancellationTokenSource;
    private readonly PredictionBackgroundService _backgroundService;

    public PredictionBackgroundServiceTests()
    {
        _mockConfiguration = new Mock<IOptions<PredictionServiceConfiguration>>();
        _mockPredictionProcessor = new Mock<IMachinePredictionProcessor>();
        _cancellationTokenSource = new CancellationTokenSource();

        _serviceProvider = new Mock<IServiceProvider>();
        _mockScope = new Mock<IServiceScope>();
        _mockScopeFactory = new Mock<IServiceScopeFactory>();

        _serviceProvider
            .Setup(x => x.GetService(typeof(IMachinePredictionProcessor)))
            .Returns(_mockPredictionProcessor.Object);

        _serviceProvider
            .Setup(x => x.GetService(typeof(IServiceScopeFactory)))
            .Returns(_mockScopeFactory.Object);

        _mockScope
            .Setup(x => x.ServiceProvider)
            .Returns(_serviceProvider.Object);

        _mockScopeFactory
            .Setup(x => x.CreateScope())
            .Returns(_mockScope.Object);

        _backgroundService = new PredictionBackgroundService(
            _mockConfiguration.Object,
            _serviceProvider.Object);
    }

    #region ExecuteAsync tests

    [Fact]
    public async Task ExecuteAsync_ShouldStartSuccessfully_WhenValidConfigurationProvided()
    {
        // Arrange
        PredictionServiceConfiguration config = CreateValidConfiguration();
        _mockConfiguration.Setup(x => x.Value).Returns(config);

        // Act
        Task task = _backgroundService.StartAsync(_cancellationTokenSource.Token);
        await Task.Delay(100); // Give it time to start
        await _backgroundService.StopAsync(_cancellationTokenSource.Token);

        // Assert
        Assert.True(true); // If we reach here, no exception was thrown
    }

    [Fact]
    public async Task ExecuteAsync_ShouldScheduleEnabledMachines_WhenConfigurationContainsEnabledMachines()
    {
        // Arrange
        PredictionServiceConfiguration config = CreateValidConfiguration();
        _mockConfiguration.Setup(x => x.Value).Returns(config);

        // Act
        Task task = _backgroundService.StartAsync(_cancellationTokenSource.Token);
        await Task.Delay(100); // Give it time to start
        await _backgroundService.StopAsync(_cancellationTokenSource.Token);

        // Assert
        // The service should have started without throwing exceptions
        _mockPredictionProcessor.Verify(
            x => x.ProcessAsync(It.IsAny<MachinePredictionConfig>()),
            Times.Exactly(config.Machines.Count(m => m.Enabled)));
    }

    [Fact]
    public async Task ExecuteAsync_ShouldHandleEmptyMachineList_WhenConfigurationContainsNoMachines()
    {
        // Arrange
        var config = new PredictionServiceConfiguration
        {
            Machines = [],
        };
        _mockConfiguration.Setup(x => x.Value).Returns(config);

        // Act
        Task task = _backgroundService.StartAsync(_cancellationTokenSource.Token);
        await Task.Delay(100); // Give it time to start
        await _backgroundService.StopAsync(_cancellationTokenSource.Token);

        // Assert
        // The service should have started without throwing exceptions
        Assert.True(true);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldThrowFileNotFoundException_WhenModelFileDoesNotExist()
    {
        // Arrange
        PredictionServiceConfiguration config = CreateValidConfiguration();
        config.Machines[0].ModelPath = "non_existent_model.onnx";
        _mockConfiguration.Setup(x => x.Value).Returns(config);

        // Act & Assert
        await Assert.ThrowsAsync<FileNotFoundException>(() => _backgroundService.StartAsync(_cancellationTokenSource.Token));
    }

    [Fact]
    public async Task ExecuteAsync_ShouldThrowInvalidOperationException_WhenMachineNameIsEmpty()
    {
        // Arrange
        PredictionServiceConfiguration config = CreateValidConfiguration();
        config.Machines[0].MachineName = string.Empty;
        _mockConfiguration.Setup(x => x.Value).Returns(config);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _backgroundService.StartAsync(_cancellationTokenSource.Token));
    }

    [Fact]
    public async Task ExecuteAsync_ShouldThrowInvalidOperationException_WhenNoInputSensorsConfigured()
    {
        // Arrange
        PredictionServiceConfiguration config = CreateValidConfiguration();
        config.Machines[0].InputSensors.Clear();
        _mockConfiguration.Setup(x => x.Value).Returns(config);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _backgroundService.StartAsync(_cancellationTokenSource.Token));
    }

    [Fact]
    public async Task ExecuteAsync_ShouldThrowInvalidOperationException_WhenPreprocessingStrategyIsEmpty()
    {
        // Arrange
        PredictionServiceConfiguration config = CreateValidConfiguration();
        config.Machines[0].PreprocessingStrategy = string.Empty;
        _mockConfiguration.Setup(x => x.Value).Returns(config);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _backgroundService.StartAsync(_cancellationTokenSource.Token));
    }

    [Fact]
    public async Task ExecuteAsync_ShouldStopGracefully_WhenCancellationRequested()
    {
        // Arrange
        PredictionServiceConfiguration config = CreateValidConfiguration();
        _mockConfiguration.Setup(x => x.Value).Returns(config);

        // Act
        Task task = _backgroundService.StartAsync(_cancellationTokenSource.Token);
        await Task.Delay(100); // Give it time to start
        _cancellationTokenSource.Cancel();
        await _backgroundService.StopAsync(_cancellationTokenSource.Token);

        // Assert
        // The service should have stopped without throwing exceptions
        Assert.True(true);
    }

    #endregion

    #region Helper methods

    private static PredictionServiceConfiguration CreateValidConfiguration()
    {
        // Create a temporary model file for testing
        string tempModelPath = Path.GetTempFileName() + ".onnx";
        File.WriteAllText(tempModelPath, "dummy model content");

        return new PredictionServiceConfiguration
        {
            Machines =
            [
                new MachinePredictionConfig
                {
                    MachineName = "test_machine_1",
                    ModelPath = tempModelPath,
                    InputSensors = ["sensor1", "sensor2"],
                    PreprocessingStrategy = "ActuatorMergingCurrent",
                    WindowSizeSeconds = 300,
                    CycleIntervalSeconds = 60,
                    Enabled = true,
                    Preprocessing = new PreprocessingConfig
                    {
                        EnableZScoreNormalization = true
                    },
                },
                new MachinePredictionConfig
                {
                    MachineName = "test_machine_2",
                    ModelPath = tempModelPath,
                    InputSensors = ["sensor3", "sensor4"],
                    PreprocessingStrategy = "ActuatorMergingCurrent",
                    WindowSizeSeconds = 600,
                    CycleIntervalSeconds = 120,
                    Enabled = true,
                    Preprocessing = new PreprocessingConfig
                    {
                        EnableZScoreNormalization = true
                    },
                }
            ],
        };
    }

    #endregion

    /// <inheritdoc/>
    public void Dispose()
    {
        _cancellationTokenSource?.Dispose();
        _backgroundService?.Dispose();
    }
}
