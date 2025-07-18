using DataAggregator.Collector.Shared.Models;
using DataAggregator.Processor.Configuration;
using DataAggregator.Processor.Services.PreProcessing.ActuatorMergingCurrentPreprocessing;

namespace DataAggregator.Processor.Tests.Services.PreProcessing;

/// <summary>
/// Tests for the <see cref="ActuatorCurrentFeatureExtractor"/> class.
/// </summary>
public class ActuatorCurrentFeatureExtractorTests
{
    private readonly ActuatorCurrentFeatureExtractor _featureExtractor;

    /// <summary>
    /// Initializes a new instance of the <see cref="ActuatorCurrentFeatureExtractorTests"/> class.
    /// </summary>
    public ActuatorCurrentFeatureExtractorTests()
        => _featureExtractor = new ActuatorCurrentFeatureExtractor();

    #region PreprocessAsync tests

    [Fact]
    public void PreprocessAsync_ShouldReturnFourteenFeatures_WhenValidDataProvided()
    {
        // Arrange
        List<IMeasurementData> measurements = CreateTestMeasurements();
        MachinePredictionConfig config = CreateValidConfig();

        // Act
        var result = _featureExtractor.PreprocessAsync(measurements, config);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(15, result.Count());
    }

    [Fact]
    public void PreprocessAsync_ShouldReturnFourteenFeatures_WhenEmptyMeasurementsProvided()
    {
        // Arrange
        var measurements = new List<IMeasurementData>();
        MachinePredictionConfig config = CreateValidConfig();

        // Act
        var result = _featureExtractor.PreprocessAsync(measurements, config);
        result = result.Where(f => f.SensorName != "Label");

        // Assert
        Assert.NotNull(result);
        Assert.Equal(14, result.Count());
        Assert.All(result, feature => Assert.Equal(0.0f, (float)feature.GetRawValue()));
    }

    [Fact]
    public void PreprocessAsync_ShouldReturnFourteenFeatures_WhenEmptySensorsListProvided()
    {
        // Arrange
        List<IMeasurementData> measurements = CreateTestMeasurements();
        MachinePredictionConfig config = CreateValidConfig();
        config.InputSensors.Clear();

        // Act
        var result = _featureExtractor.PreprocessAsync(measurements, config);
        result = result.Where(f => f.SensorName != "Label");

        // Assert
        Assert.NotNull(result);
        Assert.Equal(14, result.Count());
        Assert.All(result, feature => Assert.Equal(0.0f, (float)feature.GetRawValue()));
    }

    [Fact]
    public void PreprocessAsync_ShouldReturnValidFeatures_WhenValidDataProvided()
    {
        // Arrange
        List<IMeasurementData> measurements = CreateTestMeasurements();
        MachinePredictionConfig config = CreateValidConfig();

        // Act
        var result = _featureExtractor.PreprocessAsync(measurements, config);
        result = result.Where(f => f.SensorName != "Label");

        // Assert
        Assert.NotNull(result);
        Assert.Equal(14, result.Count());

        // Check that features are within reasonable bounds
        Assert.All(result, feature => Assert.False(float.IsNaN((float)feature.GetRawValue())));
        Assert.All(result, feature => Assert.False(float.IsInfinity((float)feature.GetRawValue())));
    }

    [Fact]
    public void PreprocessAsync_ShouldReturnZeroFeatures_WhenNoValidValuesFound()
    {
        // Arrange
        var measurements = new List<IMeasurementData>
        {
            new MeasurementData<float>(DateTime.UtcNow, "sensor1", float.NaN),
            new MeasurementData<float>(DateTime.UtcNow, "sensor2", float.PositiveInfinity),
            new MeasurementData<float>(DateTime.UtcNow, "sensor1", float.NegativeInfinity),
        };
        MachinePredictionConfig config = CreateValidConfig();

        // Act
        var result = _featureExtractor.PreprocessAsync(measurements, config);
        result = result.Where(f => f.SensorName != "Label");

        // Assert
        Assert.NotNull(result);
        Assert.Equal(14, result.Count());
        Assert.All(result, feature => Assert.Equal(0.0f, (float)feature.GetRawValue()));
    }

    [Fact]
    public void PreprocessAsync_ShouldHandleSingleValue_WhenOnlyOneValidMeasurementProvided()
    {
        // Arrange
        var measurements = new List<IMeasurementData>
        {
            new MeasurementData<float>(DateTime.UtcNow, "sensor1", 10.5f),
        };
        MachinePredictionConfig config = CreateValidConfig();

        // Act
        var result = _featureExtractor.PreprocessAsync(measurements, config);
        result = result.Where(f => f.SensorName != "Label");

        // Assert
        Assert.NotNull(result);
        Assert.Equal(14, result.Count());
        Assert.All(result, feature => Assert.False(float.IsNaN((float)feature.GetRawValue())));
    }

    [Fact]
    public void PreprocessAsync_ShouldHandleLargeDataset_WhenManyMeasurementsProvided()
    {
        // Arrange
        var measurements = new List<IMeasurementData>();
        var random = new Random(42);

        for (int i = 0; i < 1000; i++)
        {
            measurements.Add(new MeasurementData<float>(
                DateTime.UtcNow.AddSeconds(i),
                "sensor1",
                (float)random.NextDouble() * 100));
            measurements.Add(new MeasurementData<float>(
                DateTime.UtcNow.AddSeconds(i),
                "sensor2",
                (float)random.NextDouble() * 100));
        }

        MachinePredictionConfig config = CreateValidConfig();

        // Act
        var result = _featureExtractor.PreprocessAsync(measurements, config);
        result = result.Where(f => f.SensorName != "Label");

        // Assert
        Assert.NotNull(result);
        Assert.Equal(14, result.Count());
        Assert.All(result, feature => Assert.False(float.IsNaN((float)feature.GetRawValue())));
        Assert.All(result, feature => Assert.False(float.IsInfinity((float)feature.GetRawValue())));
    }

    #endregion

    #region Helper methods

    private static List<IMeasurementData> CreateTestMeasurements() => [
            new MeasurementData<float>(DateTime.UtcNow, "sensor1", 10.5f),
            new MeasurementData<float>(DateTime.UtcNow, "sensor2", 20.3f),
            new MeasurementData<float>(DateTime.UtcNow, "sensor1", 11.2f),
            new MeasurementData<float>(DateTime.UtcNow, "sensor2", 21.8f),
            new MeasurementData<float>(DateTime.UtcNow, "sensor1", 12.1f),
            new MeasurementData<float>(DateTime.UtcNow, "sensor2", 22.5f)
        ];

    private static MachinePredictionConfig CreateValidConfig() => new()
    {
        MachineName = "test_machine",
        ModelPath = "test_model.onnx",
        InputSensors = ["sensor1", "sensor2"],
        PreprocessingStrategy = "ActuatorMergingCurrent",
        WindowSizeSeconds = 1,
        CycleIntervalSeconds = 1,
        Enabled = true,
        Preprocessing = new PreprocessingConfig
        {
            EnableZScoreNormalization = true,
        },
    };

    #endregion
}
