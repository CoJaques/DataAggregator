using DataAggregator.Collector.Shared.Models;
using DataAggregator.Processor.Services.Processing.PreProcessing.ActuatorMergingCurrentPreprocessing;

namespace DataAggregator.Processor.Tests.Services.PreProcessing;

public class ActuatorCurrentFeatureExtractorTests
{
    private readonly ActuatorCurrentFeatureExtractor _featureExtractor;

    public ActuatorCurrentFeatureExtractorTests()
        => _featureExtractor = new ActuatorCurrentFeatureExtractor(CreateValidPreprocessingConfig());

    [Fact]
    public async Task ProcessAsync_ShouldReturnFifteenFeatures_WhenValidDataProvided()
    {
        var measurements = CreateTestMeasurements();
        var result = await _featureExtractor.ProcessAsync(measurements);
        Assert.NotNull(result);
        Assert.Equal(15, result.Count());
    }

    [Fact]
    public async Task ProcessAsync_ShouldReturnFourteenFeatures_WhenEmptyMeasurementsProvided()
    {
        var measurements = new List<IMeasurementData>();
        var result = await _featureExtractor.ProcessAsync(measurements);
        result = result.Where(f => f.SensorName != "Label");
        Assert.NotNull(result);
        Assert.Equal(14, result.Count());
        Assert.All(result, feature => Assert.Equal(0.0f, (float)feature.GetRawValue()));
    }

 
    [Fact]
    public async Task ProcessAsync_ShouldReturnValidFeatures_WhenValidDataProvided()
    {
        var measurements = CreateTestMeasurements();
        var result = await _featureExtractor.ProcessAsync(measurements);
        result = result.Where(f => f.SensorName != "Label");
        Assert.NotNull(result);
        Assert.Equal(14, result.Count());
        Assert.All(result, feature => Assert.False(float.IsNaN((float)feature.GetRawValue())));
        Assert.All(result, feature => Assert.False(float.IsInfinity((float)feature.GetRawValue())));
    }

    [Fact]
    public async Task ProcessAsync_ShouldReturnZeroFeatures_WhenNoValidValuesFound()
    {
        var measurements = new List<IMeasurementData>
        {
            new MeasurementData<float>(DateTime.UtcNow, "sensor1", float.NaN),
            new MeasurementData<float>(DateTime.UtcNow, "sensor2", float.PositiveInfinity),
            new MeasurementData<float>(DateTime.UtcNow, "sensor1", float.NegativeInfinity),
        };
        var result = await _featureExtractor.ProcessAsync(measurements);
        result = result.Where(f => f.SensorName != "Label");
        Assert.NotNull(result);
        Assert.Equal(14, result.Count());
        Assert.All(result, feature => Assert.Equal(0.0f, (float)feature.GetRawValue()));
    }

    [Fact]
    public async Task ProcessAsync_ShouldHandleSingleValue_WhenOnlyOneValidMeasurementProvided()
    {
        var measurements = new List<IMeasurementData>
        {
            new MeasurementData<float>(DateTime.UtcNow, "sensor1", 10.5f),
        };
        var result = await _featureExtractor.ProcessAsync(measurements);
        result = result.Where(f => f.SensorName != "Label");
        Assert.NotNull(result);
        Assert.Equal(14, result.Count());
        Assert.All(result, feature => Assert.False(float.IsNaN((float)feature.GetRawValue())));
    }

    [Fact]
    public async Task ProcessAsync_ShouldHandleLargeDataset_WhenManyMeasurementsProvided()
    {
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
        var result = await _featureExtractor.ProcessAsync(measurements);
        result = result.Where(f => f.SensorName != "Label");
        Assert.NotNull(result);
        Assert.Equal(14, result.Count());
        Assert.All(result, feature => Assert.False(float.IsNaN((float)feature.GetRawValue())));
        Assert.All(result, feature => Assert.False(float.IsInfinity((float)feature.GetRawValue())));
    }

    private static List<IMeasurementData> CreateTestMeasurements() => [
            new MeasurementData<float>(DateTime.UtcNow, "sensor1", 10.5f),
            new MeasurementData<float>(DateTime.UtcNow, "sensor2", 20.3f),
            new MeasurementData<float>(DateTime.UtcNow, "sensor1", 11.2f),
            new MeasurementData<float>(DateTime.UtcNow, "sensor2", 21.8f),
            new MeasurementData<float>(DateTime.UtcNow, "sensor1", 12.1f),
            new MeasurementData<float>(DateTime.UtcNow, "sensor2", 22.5f)
        ];

    private static PreprocessingConfig CreateValidPreprocessingConfig() => new()
    {
        EnableZScoreNormalization = true,
        NormalizationParameters = new Dictionary<string, float[]>()
    };
}
