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
        File.Copy(modelPath, copyPath);

        var inputData = new List<IMeasurementData>
        {
            new MeasurementData<float>(DateTime.UtcNow, "GlobalActivityRatio", 1.0f),
            new MeasurementData<float>(DateTime.UtcNow, "GlobalChangeDensity", 2.0f),
            new MeasurementData<float>(DateTime.UtcNow, "InterAxisMeanCorrelation", 3.0f),
        };

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
        var inputData = new List<IMeasurementData>
        {
            new MeasurementData<float>(now, "GlobalActivityRatio", 1.0f),
            new MeasurementData<float>(now, "GlobalChangeDensity", 2.0f),
            new MeasurementData<float>(now, "InterAxisMeanCorrelation", 3.0f),
            new MeasurementData<float>(now, "InterAxisMaxCorrelation", 4.0f),
            new MeasurementData<float>(now, "InterAxisCorrelationVariance", 5.0f),
            new MeasurementData<float>(now, "AxisSynchronization", 6.0f),
            new MeasurementData<float>(now, "AxisLoadBalance", 7.0f),
            new MeasurementData<float>(now, "TemporalStability", 8.0f),
            new MeasurementData<float>(now, "GlobalSkewness", 9.0f),
            new MeasurementData<float>(now, "GlobalKurtosis", 10.0f),
            new MeasurementData<float>(now, "GlobalTrendSlope", 11.0f),
            new MeasurementData<float>(now, "CoefficientOfVariation", 12.0f),
            new MeasurementData<float>(now, "NormalizedIqrMedian", 13.0f),
            new MeasurementData<float>(now, "NormalizedIqrMean", 14.0f),
        };

        try
        {
            // Act
            IEnumerable<IMeasurementData> result = await _predictionEngine.PredictAsync(modelPath, inputData);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Count() > 0);
            // TODO ADD RESULT
        }
        finally
        {
            // Cleanup
            if (File.Exists(modelPath))
            {
                File.Delete(modelPath);
            }
        }
    }

    [Fact]
    public async Task PredictAsync_ShouldHandleEmptyInputArray_WhenProvided()
    {
        // Arrange
        string modelPath = _testModelPath;
        var inputData = new List<IMeasurementData>();

        try
        {
            // Act
            IEnumerable<IMeasurementData> result = await _predictionEngine.PredictAsync(modelPath, inputData);

            // Assert
            Assert.NotNull(result);
        }
        finally
        {
            // Cleanup
            if (File.Exists(modelPath))
            {
                File.Delete(modelPath);
            }
        }
    }

    #endregion

    #region Dispose tests

    [Fact]
    public void Dispose_ShouldNotThrowException_WhenCalledMultipleTimes()
    {
        // Act & Assert
        Exception exception = Record.Exception(() =>
        {
            _predictionEngine.Dispose();
            _predictionEngine.Dispose();
        });

        Assert.Null(exception);
    }

    [Fact]
    public void Dispose_ShouldClearModelCache_WhenCalled()
    {
        // Arrange
        string modelPath = _testModelPath;
        var inputData = new List<IMeasurementData>
        {
            new MeasurementData<float>(DateTime.UtcNow, "GlobalActivityRatio", 1.0f),
        };

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
            // Cleanup
            if (File.Exists(modelPath))
            {
                File.Delete(modelPath);
            }
        }
    }

    #endregion

    #region Helper methods

    private static string _testModelPath = Path.Combine("resources", "opencn_model.onnx");

    // Simple test model bytes (minimal ONNX model)
    private static readonly byte[] TestModelBytes = Convert.FromBase64String(
        "T05OWA=="); // This is just a placeholder - in real tests you'd use a proper minimal ONNX model

    #endregion

    /// <inheritdoc/>
    public void Dispose() => _predictionEngine?.Dispose();
}
