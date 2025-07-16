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
    /// <param name="inputData">The input data for prediction as a dictionary mapping input names to values.</param>
    /// <returns>The prediction results as a dictionary mapping output names to values.</returns>
    public Task<Dictionary<string, float[]>> PredictAsync(string modelPath, Dictionary<string, float[]> inputData);

}
