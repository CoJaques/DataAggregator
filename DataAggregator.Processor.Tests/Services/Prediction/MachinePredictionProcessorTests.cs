using DataAggregator.Collector.Shared.Models;
using DataAggregator.Processor.Configuration;
using DataAggregator.Processor.Services.DataStorage;
using DataAggregator.Processor.Services.Prediction;
using DataAggregator.Processor.Services.PreProcessing;
using DataAggregator.Processor.Services.Registration;
using DataAggregator.Shared.Configuration.TimeSeries;
using DataAggregator.Shared.Domain.DataType;
using DataAggregator.Shared.DTOs;
using Moq;

namespace DataAggregator.Processor.Tests.Services.Prediction;

/// <summary>
/// Tests for the <see cref="MachinePredictionProcessor"/> class.
/// </summary>
public class MachinePredictionProcessorTests
{
    private readonly Mock<IDataRepository> _mockInfluxRepository;
    private readonly Mock<IRegistrationServiceClient> _mockRegistrationClient;
    private readonly Mock<IOnnxPredictionEngine> _mockPredictionEngine;
    private readonly Mock<IPreprocessingStrategyFactory> _mockStrategyFactory;
    private readonly Mock<IPreprocessingStrategy> _mockPreprocessingStrategy;
    private readonly MachinePredictionProcessor _processor;

    /// <summary>
    /// Initializes a new instance of the <see cref="MachinePredictionProcessorTests"/> class.
    /// </summary>
    public MachinePredictionProcessorTests()
    {
        _mockInfluxRepository = new Mock<IDataRepository>();
        _mockRegistrationClient = new Mock<IRegistrationServiceClient>();
        _mockPredictionEngine = new Mock<IOnnxPredictionEngine>();
        _mockStrategyFactory = new Mock<IPreprocessingStrategyFactory>();
        _mockPreprocessingStrategy = new Mock<IPreprocessingStrategy>();

        _processor = new MachinePredictionProcessor(
            _mockInfluxRepository.Object,
            _mockRegistrationClient.Object,
            _mockPredictionEngine.Object,
            _mockStrategyFactory.Object);
    }

    #region ProcessAsync tests

    [Fact]
    public async Task ProcessAsync_ShouldReturnEarly_WhenCollectorInfoIsNull()
    {
        // Arrange
        MachinePredictionConfig config = CreateValidMachineConfig();
        _mockRegistrationClient.Setup(x => x.GetCollectorInfoAsync(config.MachineName))
            .ReturnsAsync((CollectorInfoDto?)null);

        // Act
        await _processor.ProcessAsync(config);

        // Assert
        _mockInfluxRepository.Verify(x => x.QueryMeasurementsAsync(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<List<SensorInfoDto>>()), Times.Never);
        _mockPredictionEngine.Verify(x => x.PredictAsync(It.IsAny<string>(), It.IsAny<Dictionary<string, float[]>>()), Times.Never);
    }

    [Fact]
    public async Task ProcessAsync_ShouldReturnEarly_WhenNoValidSensorsFound()
    {
        // Arrange
        MachinePredictionConfig config = CreateValidMachineConfig();
        CollectorInfoDto collectorInfo = CreateCollectorInfoWithSensors(new[] { "different_sensor" });

        _mockRegistrationClient.Setup(x => x.GetCollectorInfoAsync(config.MachineName))
            .ReturnsAsync(collectorInfo);

        // Act
        await _processor.ProcessAsync(config);

        // Assert
        _mockInfluxRepository.Verify(x => x.QueryMeasurementsAsync(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<List<SensorInfoDto>>()), Times.Never);
        _mockPredictionEngine.Verify(x => x.PredictAsync(It.IsAny<string>(), It.IsAny<Dictionary<string, float[]>>()), Times.Never);
    }

    [Fact]
    public async Task ProcessAsync_ShouldReturnEarly_WhenNoMeasurementsFound()
    {
        // Arrange
        MachinePredictionConfig config = CreateValidMachineConfig();
        CollectorInfoDto collectorInfo = CreateCollectorInfoWithSensors(config.InputSensors);

        _mockRegistrationClient.Setup(x => x.GetCollectorInfoAsync(config.MachineName))
            .ReturnsAsync(collectorInfo);
        _mockInfluxRepository.Setup(x => x.QueryMeasurementsAsync(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<List<SensorInfoDto>>()))
            .ReturnsAsync([]);

        // Act
        await _processor.ProcessAsync(config);

        // Assert
        _mockPredictionEngine.Verify(x => x.PredictAsync(It.IsAny<string>(), It.IsAny<Dictionary<string, float[]>>()), Times.Never);
        _mockInfluxRepository.Verify(x => x.WriteMeasurementAsync(It.IsAny<string>(), It.IsAny<IMeasurementData>()), Times.Never);
    }

    [Fact]
    public async Task ProcessAsync_ShouldReturnEarly_WhenPreprocessingFails()
    {
        // Arrange
        MachinePredictionConfig config = CreateValidMachineConfig();
        CollectorInfoDto collectorInfo = CreateCollectorInfoWithSensors(config.InputSensors);
        List<IMeasurementData> measurements = CreateTestMeasurements();

        _mockRegistrationClient.Setup(x => x.GetCollectorInfoAsync(config.MachineName))
            .ReturnsAsync(collectorInfo);
        _mockInfluxRepository.Setup(x => x.QueryMeasurementsAsync(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<List<SensorInfoDto>>()))
            .ReturnsAsync(measurements);
        _mockStrategyFactory.Setup(x => x.CreateStrategy(config.PreprocessingStrategy))
            .Returns(_mockPreprocessingStrategy.Object);
        _mockPreprocessingStrategy.Setup(x => x.PreprocessAsync(It.IsAny<List<IMeasurementData>>(), It.IsAny<MachinePredictionConfig>()))
            .Returns(new Dictionary<string, float[]>());

        // Act
        await _processor.ProcessAsync(config);

        // Assert
        _mockPredictionEngine.Verify(x => x.PredictAsync(It.IsAny<string>(), It.IsAny<Dictionary<string, float[]>>()), Times.Never);
        _mockInfluxRepository.Verify(x => x.WriteMeasurementAsync(It.IsAny<string>(), It.IsAny<IMeasurementData>()), Times.Never);
    }

    [Fact]
    public async Task ProcessAsync_ShouldCompleteSuccessfully_WhenAllConditionsAreMet()
    {
        // Arrange
        MachinePredictionConfig config = CreateValidMachineConfig();
        CollectorInfoDto collectorInfo = CreateCollectorInfoWithSensors(config.InputSensors);
        List<IMeasurementData> measurements = CreateTestMeasurements();
        var preprocessedData = new Dictionary<string, float[]> 
        { 
            ["GlobalActivityRatio"] = [1.0f],
            ["GlobalChangeDensity"] = [2.0f],
            ["InterAxisMeanCorrelation"] = [3.0f],
            ["InterAxisMaxCorrelation"] = [4.0f],
            ["InterAxisCorrelationVariance"] = [5.0f],
            ["AxisSynchronization"] = [6.0f],
            ["AxisLoadBalance"] = [7.0f],
            ["TemporalStability"] = [8.0f],
            ["GlobalSkewness"] = [9.0f],
            ["GlobalKurtosis"] = [10.0f],
            ["GlobalTrendSlope"] = [11.0f],
            ["CoefficientOfVariation"] = [12.0f],
            ["NormalizedIqrMedian"] = [13.0f],
            ["NormalizedIqrMean"] = [14.0f]
        };
        float[] predictions = new float[] { 0.85f };

        _mockRegistrationClient.Setup(x => x.GetCollectorInfoAsync(config.MachineName))
            .ReturnsAsync(collectorInfo);
        _mockInfluxRepository.Setup(x => x.QueryMeasurementsAsync(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<List<SensorInfoDto>>()))
            .ReturnsAsync(measurements);
        _mockStrategyFactory.Setup(x => x.CreateStrategy(config.PreprocessingStrategy))
            .Returns(_mockPreprocessingStrategy.Object);
        _mockPreprocessingStrategy.Setup(x => x.PreprocessAsync(It.IsAny<List<IMeasurementData>>(), It.IsAny<MachinePredictionConfig>()))
            .Returns(preprocessedData);
        _mockPredictionEngine.Setup(x => x.PredictAsync(config.ModelPath, preprocessedData))
            .ReturnsAsync(predictions);

        // Act
        await _processor.ProcessAsync(config);

        // Assert
        _mockInfluxRepository.Verify(
            x => x.InitializeAsync(
            collectorInfo.AssignedInfluxEndpoint.Endpoint,
            collectorInfo.AssignedInfluxEndpoint.Token,
            "Dataggregator"), Times.Once);
        _mockInfluxRepository.Verify(
            x => x.QueryMeasurementsAsync(
            config.MachineName,
            It.IsAny<DateTime>(),
            It.IsAny<DateTime>(),
            It.IsAny<List<SensorInfoDto>>()), Times.Once);
        _mockPredictionEngine.Verify(x => x.PredictAsync(config.ModelPath, preprocessedData), Times.Once);
        _mockInfluxRepository.Verify(x => x.WriteMeasurementAsync(config.MachineName, It.IsAny<IMeasurementData>()), Times.Once);
    }

    [Fact]
    public async Task ProcessAsync_ShouldReinitializeInfluxConnection_WhenEndpointChanges()
    {
        // Arrange
        MachinePredictionConfig config = CreateValidMachineConfig();
        CollectorInfoDto collectorInfo1 = CreateCollectorInfoWithSensors(config.InputSensors, "endpoint1");
        CollectorInfoDto collectorInfo2 = CreateCollectorInfoWithSensors(config.InputSensors, "endpoint2");
        List<IMeasurementData> measurements = CreateTestMeasurements();
        var preprocessedData = new Dictionary<string, float[]> 
        { 
            ["GlobalActivityRatio"] = [1.0f],
            ["GlobalChangeDensity"] = [2.0f],
            ["InterAxisMeanCorrelation"] = [3.0f],
            ["InterAxisMaxCorrelation"] = [4.0f],
            ["InterAxisCorrelationVariance"] = [5.0f],
            ["AxisSynchronization"] = [6.0f],
            ["AxisLoadBalance"] = [7.0f],
            ["TemporalStability"] = [8.0f],
            ["GlobalSkewness"] = [9.0f],
            ["GlobalKurtosis"] = [10.0f],
            ["GlobalTrendSlope"] = [11.0f],
            ["CoefficientOfVariation"] = [12.0f],
            ["NormalizedIqrMedian"] = [13.0f],
            ["NormalizedIqrMean"] = [14.0f]
        };
        float[] predictions = new float[] { 0.85f };

        _mockRegistrationClient.SetupSequence(x => x.GetCollectorInfoAsync(config.MachineName))
            .ReturnsAsync(collectorInfo1)
            .ReturnsAsync(collectorInfo2);
        _mockInfluxRepository.Setup(x => x.QueryMeasurementsAsync(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<List<SensorInfoDto>>()))
            .ReturnsAsync(measurements);
        _mockStrategyFactory.Setup(x => x.CreateStrategy(config.PreprocessingStrategy))
            .Returns(_mockPreprocessingStrategy.Object);
        _mockPreprocessingStrategy.Setup(x => x.PreprocessAsync(It.IsAny<List<IMeasurementData>>(), It.IsAny<MachinePredictionConfig>()))
            .Returns(preprocessedData);
        _mockPredictionEngine.Setup(x => x.PredictAsync(config.ModelPath, preprocessedData))
            .ReturnsAsync(predictions);

        // Act
        await _processor.ProcessAsync(config); // First call with endpoint1
        await _processor.ProcessAsync(config); // Second call with endpoint2

        // Assert
        _mockInfluxRepository.Verify(
            x => x.InitializeAsync(
            collectorInfo1.AssignedInfluxEndpoint.Endpoint,
            collectorInfo1.AssignedInfluxEndpoint.Token,
            "Dataggregator"), Times.Once);
        _mockInfluxRepository.Verify(
            x => x.InitializeAsync(
            collectorInfo2.AssignedInfluxEndpoint.Endpoint,
            collectorInfo2.AssignedInfluxEndpoint.Token,
            "Dataggregator"), Times.Once);
    }

    [Fact]
    public async Task ProcessAsync_ShouldNotReinitializeInfluxConnection_WhenEndpointIsSame()
    {
        // Arrange
        MachinePredictionConfig config = CreateValidMachineConfig();
        CollectorInfoDto collectorInfo = CreateCollectorInfoWithSensors(config.InputSensors);
        List<IMeasurementData> measurements = CreateTestMeasurements();
        var preprocessedData = new Dictionary<string, float[]> 
        { 
            ["GlobalActivityRatio"] = [1.0f],
            ["GlobalChangeDensity"] = [2.0f],
            ["InterAxisMeanCorrelation"] = [3.0f],
            ["InterAxisMaxCorrelation"] = [4.0f],
            ["InterAxisCorrelationVariance"] = [5.0f],
            ["AxisSynchronization"] = [6.0f],
            ["AxisLoadBalance"] = [7.0f],
            ["TemporalStability"] = [8.0f],
            ["GlobalSkewness"] = [9.0f],
            ["GlobalKurtosis"] = [10.0f],
            ["GlobalTrendSlope"] = [11.0f],
            ["CoefficientOfVariation"] = [12.0f],
            ["NormalizedIqrMedian"] = [13.0f],
            ["NormalizedIqrMean"] = [14.0f]
        };
        float[] predictions = new float[] { 0.85f };

        _mockRegistrationClient.Setup(x => x.GetCollectorInfoAsync(config.MachineName))
            .ReturnsAsync(collectorInfo);
        _mockInfluxRepository.Setup(x => x.QueryMeasurementsAsync(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<List<SensorInfoDto>>()))
            .ReturnsAsync(measurements);
        _mockStrategyFactory.Setup(x => x.CreateStrategy(config.PreprocessingStrategy))
            .Returns(_mockPreprocessingStrategy.Object);
        _mockPreprocessingStrategy.Setup(x => x.PreprocessAsync(It.IsAny<List<IMeasurementData>>(), It.IsAny<MachinePredictionConfig>()))
            .Returns(preprocessedData);
        _mockPredictionEngine.Setup(x => x.PredictAsync(config.ModelPath, preprocessedData))
            .ReturnsAsync(predictions);

        // Act
        await _processor.ProcessAsync(config);
        await _processor.ProcessAsync(config);

        // Assert
        _mockInfluxRepository.Verify(
            x => x.InitializeAsync(
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task ProcessAsync_ShouldThrowException_WhenRegistrationClientThrows()
    {
        // Arrange
        MachinePredictionConfig config = CreateValidMachineConfig();
        _mockRegistrationClient.Setup(x => x.GetCollectorInfoAsync(config.MachineName))
            .ThrowsAsync(new InvalidOperationException("Registration service error"));

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _processor.ProcessAsync(config));
    }

    [Fact]
    public async Task ProcessAsync_ShouldThrowException_WhenPredictionEngineThrows()
    {
        // Arrange
        MachinePredictionConfig config = CreateValidMachineConfig();
        CollectorInfoDto collectorInfo = CreateCollectorInfoWithSensors(config.InputSensors);
        List<IMeasurementData> measurements = CreateTestMeasurements();
        var preprocessedData = new Dictionary<string, float[]> 
        { 
            ["GlobalActivityRatio"] = [1.0f],
            ["GlobalChangeDensity"] = [2.0f],
            ["InterAxisMeanCorrelation"] = [3.0f],
            ["InterAxisMaxCorrelation"] = [4.0f],
            ["InterAxisCorrelationVariance"] = [5.0f],
            ["AxisSynchronization"] = [6.0f],
            ["AxisLoadBalance"] = [7.0f],
            ["TemporalStability"] = [8.0f],
            ["GlobalSkewness"] = [9.0f],
            ["GlobalKurtosis"] = [10.0f],
            ["GlobalTrendSlope"] = [11.0f],
            ["CoefficientOfVariation"] = [12.0f],
            ["NormalizedIqrMedian"] = [13.0f],
            ["NormalizedIqrMean"] = [14.0f]
        };

        _mockRegistrationClient.Setup(x => x.GetCollectorInfoAsync(config.MachineName))
            .ReturnsAsync(collectorInfo);
        _mockInfluxRepository.Setup(x => x.QueryMeasurementsAsync(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<List<SensorInfoDto>>()))
            .ReturnsAsync(measurements);
        _mockStrategyFactory.Setup(x => x.CreateStrategy(config.PreprocessingStrategy))
            .Returns(_mockPreprocessingStrategy.Object);
        _mockPreprocessingStrategy.Setup(x => x.PreprocessAsync(It.IsAny<List<IMeasurementData>>(), It.IsAny<MachinePredictionConfig>()))
            .Returns(preprocessedData);
        _mockPredictionEngine.Setup(x => x.PredictAsync(config.ModelPath, preprocessedData))
            .ThrowsAsync(new InvalidOperationException("Prediction error"));

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _processor.ProcessAsync(config));
    }

    #endregion

    #region Helper methods

    private static MachinePredictionConfig CreateValidMachineConfig() => new()
    {
        MachineName = "test_machine",
        ModelPath = "test_model.onnx",
        InputSensors = ["sensor1", "sensor2"],
        PredictionSensorName = "prediction_sensor",
        PreprocessingStrategy = "test_strategy",
        WindowSizeSeconds = 300,
        CycleIntervalSeconds = 60,
        Enabled = true,
    };

    private static CollectorInfoDto CreateCollectorInfoWithSensors(IEnumerable<string> sensorNames, string endpoint = "http://localhost:8086") => new(
            "test_device",
            "test_location",
            "http://localhost:5000/health",
            new InfluxEndpoint("TestEndpoint", endpoint, "test_token"),
            sensorNames.Select(name => new SensorInfoDto(name, "Type", "Unit", [], SensorDataType.Float)).ToList(),
            []);

    private static List<IMeasurementData> CreateTestMeasurements()
        =>
        [
            new MeasurementData<float>(DateTime.UtcNow, "sensor1", 10.5f),
            new MeasurementData<float>(DateTime.UtcNow, "sensor2", 20.3f)
        ];

    #endregion
}
