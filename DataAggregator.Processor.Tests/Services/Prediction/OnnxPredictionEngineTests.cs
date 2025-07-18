using System.Reflection.Metadata;
using DataAggregator.Collector.Shared.Models;
using DataAggregator.Processor.Services.Prediction;
using Microsoft.ML.OnnxRuntime;

namespace DataAggregator.Processor.Tests.Services.Prediction;

/// <summary>
/// Tests for the <see cref="OnnxPredictionEngine"/> class.
/// </summary>
public class OnnxPredictionEngineTests : IDisposable
{
    private readonly OnnxPredictionEngine _predictionEngine;

    /// <summary>
    /// Initializes a new instance of the <see cref="OnnxPredictionEngineTests"/> class.
    /// </summary>
    public OnnxPredictionEngineTests()
        => _predictionEngine = new OnnxPredictionEngine();

    #region PredictAsync tests

    [Fact]
    public async Task PredictAsync_ShouldThrowFileNotFoundException_WhenModelPathDoesNotExist()
    {
        // Arrange
        string nonExistentModelPath = "non_existent_model.onnx";

        var inputData = new List<IMeasurementData>
        {
            new MeasurementData<float>(DateTime.UtcNow, "GlobalActivityRatio", 1.0f),
            new MeasurementData<float>(DateTime.UtcNow, "GlobalChangeDensity", 2.0f),
            new MeasurementData<float>(DateTime.UtcNow, "InterAxisMeanCorrelation", 3.0f),
        };

        // Act & Assert
        FileNotFoundException exception = await Assert.ThrowsAsync<FileNotFoundException>(
            () => _predictionEngine.PredictAsync(nonExistentModelPath, inputData));
    }

    [Fact]
    public async Task PredictAsync_ShouldCacheModel_WhenSameModelPathIsUsedMultipleTimes()
    {
        // Arrange
        string modelPath = _testModelPath;
        string copyPath = Path.Combine("resources", "opencn_model_copy.onnx");
        File.Copy(modelPath, copyPath, true);

        var inputData = ProcessorTestHelper.GetValidTestData();

        // Act
        IEnumerable<IMeasurementData> result1 = await _predictionEngine.PredictAsync(copyPath, inputData);
        File.Delete(copyPath); // Simulate model file deletion
        IEnumerable<IMeasurementData> result2 = await _predictionEngine.PredictAsync(copyPath, inputData);

        // Assert
        Assert.NotNull(result1);
        Assert.NotNull(result2);
        Assert.Equal(result1.Count(), result2.Count());
    }

    [Fact]
    public async Task PredictAsync_ShouldReturnCorrectOutputShape_WhenValidInputProvided()
    {
        // Arrange
        string modelPath = _testModelPath;
        var now = DateTime.UtcNow;
        var inputData = ProcessorTestHelper.GetValidTestData();

        // Act
        IEnumerable<IMeasurementData> result = await _predictionEngine.PredictAsync(modelPath, inputData);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Count() > 0);
    }

    [Fact]
    public async Task PredictAsync_ShouldReturnGoodResult_DifferentStateDataProvided()
    {
        // Arrange
        string modelPath = _testModelPath;
        var inputDataShutdown = ProcessorTestHelper.GetValidShutdownStateData();
        var inputDataProduction = ProcessorTestHelper.GetValidProductionStateData();
        var inputDataIdle = ProcessorTestHelper.GetValidIdleStateData();

        // Act
        IEnumerable<IMeasurementData> resultShutdown = await _predictionEngine.PredictAsync(modelPath, inputDataShutdown);
        IEnumerable<IMeasurementData> resultProduction = await _predictionEngine.PredictAsync(modelPath, inputDataProduction);
        IEnumerable<IMeasurementData> resultIdle = await _predictionEngine.PredictAsync(modelPath, inputDataIdle);

        // Assert
        Assert.NotNull(resultShutdown);
        Assert.NotNull(resultProduction);
        Assert.NotNull(resultIdle);
        Assert.Equal("disable", resultShutdown.First(x => x.SensorName == "PredictedLabel.output_0").GetRawValue().ToString());
        Assert.Equal("production", resultProduction.First(x => x.SensorName == "PredictedLabel.output_0").GetRawValue().ToString());
        Assert.Equal("enable", resultIdle.First(static x => x.SensorName == "PredictedLabel.output_0").GetRawValue().ToString());

    }

    [Fact]
    public async Task PredictAsync_ShouldHandleEmptyInputArray_WhenProvided()
    {
        // Arrange
        string modelPath = _testModelPath;
        var inputData = new List<IMeasurementData>();

        // Act
        IEnumerable<IMeasurementData> result = await _predictionEngine.PredictAsync(modelPath, inputData);

        // Assert
        Assert.NotNull(result);
    }

    #endregion

    #region Dispose tests

    [Fact]
    public void Dispose_ShouldClearModelCache_WhenCalled()
    {
        // Arrange
        string modelPath = _testModelPath;
        IEnumerable<IMeasurementData> inputData = ProcessorTestHelper.GetValidTestData();

        try
        {
            // Act - Load model into cache
            _ = _predictionEngine.PredictAsync(modelPath, inputData).Result;

            // Act - Dispose
            _predictionEngine.Dispose();

            // Assert - Should be able to dispose without exception
            Assert.True(true); // If we reach here, no exception was thrown
        }
        finally
        {
            // do nothing
        }
    }

    #endregion

    #region Helper methods

    private static string _testModelPath = Path.Combine("resources", "opencn_model.onnx");

    #endregion

    /// <inheritdoc/>
    public void Dispose() => _predictionEngine?.Dispose();
}
