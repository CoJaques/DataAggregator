using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using Serilog;

namespace DataAggregator.Processor.Services.Prediction;

/// <summary>
/// Implementation of ONNX prediction engine.
/// </summary>
public class OnnxPredictionEngine : IOnnxPredictionEngine, IDisposable
{
    #region Private fields

    private readonly Dictionary<string, InferenceSession> _modelCache = [];

    #endregion

    #region Public methods

    /// <inheritdoc/>
    public async Task<Dictionary<string, float[]>> PredictAsync(string modelPath, Dictionary<string, float[]> inputData)
    {
        try
        {
            InferenceSession session = LoadOrGetModel(modelPath);

            // Validate input data against model schema
            ValidateInputData(session, inputData);

            // Prepare input tensors
            var inputs = new List<NamedOnnxValue>();
            foreach (KeyValuePair<string, float[]> kvp in inputData)
            {
                string inputName = kvp.Key;
                float[] inputValues = kvp.Value;

                // Create tensor with shape [1, inputValues.Length] for single sample
                int[] inputShape = [1, inputValues.Length];
                var inputTensor = new DenseTensor<float>(inputValues, inputShape);

                inputs.Add(NamedOnnxValue.CreateFromTensor(inputName, inputTensor));
            }

            using IDisposableReadOnlyCollection<DisposableNamedOnnxValue> results = session.Run(inputs);
            
            // Create output dictionary with output names and values
            var outputData = new Dictionary<string, float[]>();
            foreach (var result in results)
            {
                string outputName = result.Name;
                Tensor<float> outputTensor = result.AsTensor<float>();
                float[] outputValues = [.. outputTensor];
                outputData[outputName] = outputValues;
            }

            Log.Debug(
                "Prediction completed for model {ModelPath} with {InputCount} inputs and {OutputCount} outputs",
                modelPath,
                inputData.Count,
                outputData.Count);

            return await Task.FromResult(outputData);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error during prediction with model {ModelPath}", modelPath);
            throw;
        }
    }

    /// <summary>
    /// Disposes the cached models.
    /// </summary>
    public void Dispose()
    {
        foreach (InferenceSession session in _modelCache.Values)
        {
            session?.Dispose();
        }

        _modelCache.Clear();
    }

    #endregion

    #region Private methods

    private InferenceSession LoadOrGetModel(string modelPath)
    {
        if (_modelCache.TryGetValue(modelPath, out InferenceSession? cachedSession))
        {
            return cachedSession;
        }

        if (!File.Exists(modelPath))
        {
            Log.Error("ONNX model file not found: {ModelPath}", modelPath);
            throw new FileNotFoundException($"ONNX model file not found: {modelPath}");
        }

        try
        {
            var session = new InferenceSession(modelPath);
            _modelCache[modelPath] = session;

            Log.Information("Loaded ONNX model: {ModelPath}", modelPath);
            return session;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Failed to load ONNX model: {ModelPath}", modelPath);
            throw;
        }
    }

    private void ValidateInputData(InferenceSession session, Dictionary<string, float[]> inputData)
    {
        IReadOnlyDictionary<string, NodeMetadata> modelInputs = session.InputMetadata;

        // Check if all required model inputs are provided
        foreach (KeyValuePair<string, NodeMetadata> modelInput in modelInputs)
        {
            if (!inputData.ContainsKey(modelInput.Key))
            {
                throw new ArgumentException(
                    $"Model requires input '{modelInput.Key}' but it was not provided. " +
                    $"Available inputs: [{string.Join(", ", inputData.Keys)}]");
            }
        }

        Log.Debug("Input validation passed for model with {InputCount} inputs", inputData.Count);
    }

    #endregion
}
