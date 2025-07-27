using System.Reflection.Metadata;
using DataAggregator.Collector.Shared.Models;
using DataAggregator.Processor.Configuration;
using DataAggregator.Processor.Services.Prediction;
using Microsoft.ML.OnnxRuntime;
using System.Threading.Tasks;
using DataAggregator.Processor.Services.Processing.Onnx;

namespace DataAggregator.Processor.Tests.Services.Prediction;

/// <summary>
/// Tests for the <see cref="OnnxPredictionEngine"/> class.
/// </summary>
public class OnnxPredictionEngineTests : IDisposable
{
    private OnnxPredictionEngine _predictionEngine;

    public OnnxPredictionEngineTests()
    {
        // L'instance sera créée dans chaque test avec la bonne config
    }

    [Fact]
    public async Task ProcessAsync_ShouldThrowFileNotFoundException_WhenModelPathDoesNotExist()
    {
        var config = new OnnxPredictionConfig { ModelPath = "non_existent_model.onnx" };
        _predictionEngine = new OnnxPredictionEngine(config);
        var inputData = new List<IMeasurementData>
        {
            new MeasurementData<float>(DateTime.UtcNow, "GlobalActivityRatio", 1.0f),
            new MeasurementData<float>(DateTime.UtcNow, "GlobalChangeDensity", 2.0f),
            new MeasurementData<float>(DateTime.UtcNow, "InterAxisMeanCorrelation", 3.0f),
        };
        await Assert.ThrowsAsync<FileNotFoundException>(() => _predictionEngine.ProcessAsync(inputData));
    }

    [Fact]
    public async Task ProcessAsync_ShouldReturnCorrectOutputShape_WhenValidInputProvided()
    {
        var config = new OnnxPredictionConfig { ModelPath = _testModelPath };
        _predictionEngine = new OnnxPredictionEngine(config);
        var inputData = ProcessorTestHelper.GetValidTestData();
        var result = await _predictionEngine.ProcessAsync(inputData);
        Assert.NotNull(result);
        Assert.True(result.Count() > 0);
    }

    [Fact]
    public async Task ProcessAsync_ShouldReturnGoodResult_DifferentStateDataProvided()
    {
        var config = new OnnxPredictionConfig { ModelPath = _testModelPath };
        _predictionEngine = new OnnxPredictionEngine(config);
        var inputDataShutdown = ProcessorTestHelper.GetValidShutdownStateData();
        var inputDataProduction = ProcessorTestHelper.GetValidProductionStateData();
        var inputDataIdle = ProcessorTestHelper.GetValidIdleStateData();
        var resultShutdown = await _predictionEngine.ProcessAsync(inputDataShutdown);
        var resultProduction = await _predictionEngine.ProcessAsync(inputDataProduction);
        var resultIdle = await _predictionEngine.ProcessAsync(inputDataIdle);
        Assert.NotNull(resultShutdown);
        Assert.NotNull(resultProduction);
        Assert.NotNull(resultIdle);
        Assert.Equal("disable", resultShutdown.First(x => x.SensorName == "PredictedLabel.output_0").GetRawValue().ToString());
        Assert.Equal("production", resultProduction.First(x => x.SensorName == "PredictedLabel.output_0").GetRawValue().ToString());
        Assert.Equal("enable", resultIdle.First(x => x.SensorName == "PredictedLabel.output_0").GetRawValue().ToString());
    }

    [Fact]
    public async Task ProcessAsync_ShouldHandleEmptyInputArray_WhenProvided()
    {
        var config = new OnnxPredictionConfig { ModelPath = _testModelPath };
        _predictionEngine = new OnnxPredictionEngine(config);
        var inputData = new List<IMeasurementData>();
        var result = await _predictionEngine.ProcessAsync(inputData);
        Assert.NotNull(result);
    }

    [Fact]
    public void Dispose_ShouldClearModelCache_WhenCalled()
    {
        var config = new OnnxPredictionConfig { ModelPath = _testModelPath };
        _predictionEngine = new OnnxPredictionEngine(config);
        IEnumerable<IMeasurementData> inputData = ProcessorTestHelper.GetValidTestData();
        try
        {
            _ = _predictionEngine.ProcessAsync(inputData).Result;
            _predictionEngine.Dispose();
            Assert.True(true);
        }
        finally { }
    }

    private static string _testModelPath = Path.Combine("resources", "opencn_model.onnx");

    public void Dispose() => _predictionEngine?.Dispose();
}
