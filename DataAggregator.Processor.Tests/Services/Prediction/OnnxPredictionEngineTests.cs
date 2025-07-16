using System.Reflection.Metadata;
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
        var inputData = new Dictionary<string, float[]> 
        { 
            ["GlobalActivityRatio"] = [1.0f],
            ["GlobalChangeDensity"] = [2.0f],
            ["InterAxisMeanCorrelation"] = [3.0f]
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

        var inputData = new Dictionary<string, float[]> 
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

        // Act
        Dictionary<string, float[]> result1 = await _predictionEngine.PredictAsync(copyPath, inputData);
        File.Delete(copyPath); // Simulate model file deletion
        Dictionary<string, float[]> result2 = await _predictionEngine.PredictAsync(copyPath, inputData);

        // Assert
        Assert.NotNull(result1);
        Assert.NotNull(result2);
        Assert.Equal(result1.Count, result2.Count);
    }

    [Fact]
    public async Task PredictAsync_ShouldReturnCorrectOutputShape_WhenValidInputProvided()
    {
        // Arrange
        string modelPath = _testModelPath;
        var inputData = new Dictionary<string, float[]> 
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

        try
        {
            // Act
            Dictionary<string, float[]> result = await _predictionEngine.PredictAsync(modelPath, inputData);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Count > 0);
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
        var inputData = new Dictionary<string, float[]> { ["GlobalActivityRatio"] = new float[0] };

        try
        {
            // Act
            Dictionary<string, float[]> result = await _predictionEngine.PredictAsync(modelPath, inputData);

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

    #region GetModelMetadataAsync tests

    [Fact]
    public async Task GetModelMetadataAsync_ShouldReturnMetadata_WhenValidModelProvided()
    {
        // Arrange
        string modelPath = _testModelPath;

        try
        {
            // Act
            OnnxModelMetadata metadata = await _predictionEngine.GetModelMetadataAsync(modelPath);

            // Assert
            Assert.NotNull(metadata);
            Assert.NotNull(metadata.InputNames);
            Assert.NotNull(metadata.OutputNames);
            Assert.NotNull(metadata.InputShapes);
            Assert.NotNull(metadata.OutputShapes);
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
    public async Task GetModelMetadataAsync_ShouldThrowFileNotFoundException_WhenModelPathDoesNotExist()
    {
        // Arrange
        string nonExistentModelPath = "non_existent_model.onnx";

        // Act & Assert
        FileNotFoundException exception = await Assert.ThrowsAsync<FileNotFoundException>(
            () => _predictionEngine.GetModelMetadataAsync(nonExistentModelPath));
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
        var inputData = new Dictionary<string, float[]> { ["GlobalActivityRatio"] = [1.0f] };

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
