namespace DataAggregator.Processor.Services.Prediction;

/// <summary>
/// Interface for ONNX prediction engine.
/// </summary>
public interface IOnnxPredictionEngine
{
    /// <summary>
    /// Performs prediction using an ONNX model.
    /// </summary>
    /// <param name="modelPath">The path to the ONNX model file.</param>
    /// <param name="inputData">The input data for prediction (single sample).</param>
    /// <returns>The prediction results as a float array.</returns>
    public Task<float[]> PredictAsync(string modelPath, float[] inputData);
}
