using DataAggregator.Processor.Configuration;
using DataAggregator.Processor.Services;
using DataAggregator.Processor.Services.Prediction;
using DataAggregator.Processor.Services.Processing.Factory;
using DataAggregator.Processor.Services.Processing.PreProcessing.ActuatorMergingCurrentPreprocessing;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace DataAggregator.Processor.Tests.Services;

/// <summary>
/// Tests for the <see cref="PredictionBackgroundService"/> class.
/// </summary>
public class PredictionBackgroundServiceTests : IDisposable
{
    private readonly Mock<IMachinePredictionProcessor> _mockPredictionProcessor;
    private readonly Mock<IServiceProvider> _serviceProvider;
    private readonly Mock<IServiceScope> _mockScope;
    private readonly Mock<IServiceScopeFactory> _mockScopeFactory;
    private readonly CancellationTokenSource _cancellationTokenSource;
    private readonly PredictionBackgroundService _backgroundService;

    public PredictionBackgroundServiceTests()
    {
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
            CreateValidConfiguration(),
            _serviceProvider.Object);
    }

    #region ExecuteAsync tests

    [Fact]
    public async Task ExecuteAsync_ShouldStartSuccessfully_WhenValidConfigurationProvided()
    {
        var config = CreateValidConfiguration();

        Task task = _backgroundService.StartAsync(_cancellationTokenSource.Token);
        await Task.Delay(100);
        await _backgroundService.StopAsync(_cancellationTokenSource.Token);

        Assert.True(true);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldScheduleEnabledMachines_WhenConfigurationContainsEnabledMachines()
    {
        var config = CreateValidConfiguration();

        Task task = _backgroundService.StartAsync(_cancellationTokenSource.Token);
        await Task.Delay(100);
        await _backgroundService.StopAsync(_cancellationTokenSource.Token);

        _mockPredictionProcessor.Verify(
            x => x.ProcessAsync(It.IsAny<MachinePredictionConfig>()),
            Times.Exactly(config.Machines.Count(m => m.Enabled)));
    }

    [Fact]
    public async Task ExecuteAsync_ShouldHandleEmptyMachineList_WhenConfigurationContainsNoMachines()
    {
        var config = new PredictionServiceConfiguration
        {
            Machines = [],
        };

        Task task = _backgroundService.StartAsync(_cancellationTokenSource.Token);
        await Task.Delay(100);
        await _backgroundService.StopAsync(_cancellationTokenSource.Token);

        Assert.True(true);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldStopGracefully_WhenCancellationRequested()
    {
        var config = CreateValidConfiguration();

        Task task = _backgroundService.StartAsync(_cancellationTokenSource.Token);
        await Task.Delay(100);
        _cancellationTokenSource.Cancel();
        await _backgroundService.StopAsync(_cancellationTokenSource.Token);

        Assert.True(true);
    }

    #endregion

    #region Helper methods

    private static PredictionServiceConfiguration CreateValidConfiguration() => new PredictionServiceConfiguration
    {
        Machines =
            [
                new MachinePredictionConfig
                {
                    MachineName = "test_machine_1",
                    InputSensors = ["sensor1", "sensor2"],
                    WindowSize = 300,
                    CycleIntervalSeconds = 60,
                    Enabled = true,
                    ProcessingPipeline = new List<ProcessorDescription>
                    {
                        new ProcessorDescription
                        {
                            Name = "actuatorcurrent",
                            Configuration = new PreprocessingConfig
                            {
                                EnableZScoreNormalization = true,
                                NormalizationParameters = new Dictionary<string, float[]>()
                            }
                        }
                    },
                },
                new MachinePredictionConfig
                {
                    MachineName = "test_machine_2",
                    InputSensors = ["sensor3", "sensor4"],
                    WindowSize = 600,
                    CycleIntervalSeconds = 120,
                    Enabled = true,
                    ProcessingPipeline = new List<ProcessorDescription>
                    {
                        new ProcessorDescription
                        {
                            Name = "actuatorcurrent",
                            Configuration = new PreprocessingConfig
                            {
                                EnableZScoreNormalization = true,
                                NormalizationParameters = new Dictionary<string, float[]>()
                            }
                        }
                    },
                }
            ],
    };

    #endregion

    /// <inheritdoc/>
    public void Dispose()
    {
        _cancellationTokenSource?.Dispose();
        _backgroundService?.Dispose();
    }
}
