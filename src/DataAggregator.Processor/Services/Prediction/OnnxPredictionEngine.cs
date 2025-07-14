using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using Serilog;

namespace DataAggregator.Processor.Services.Prediction;

/// <summary>
/// Implementation of ONNX prediction engine.
/// </summary>
public class OnnxPredictionEngine : IOnnxPredictionEngine, IDisposable
{
    private readonly Dictionary<string, InferenceSession> _modelCache = [];

    /// <inheritdoc/>
    public async Task<float[]> PredictAsync(string modelPath, float[] inputData)
    {
        try
        {
            InferenceSession session = LoadOrGetModel(modelPath);

            // Prepare input tensor
            int[] inputShape = [1, inputData.Length];
            var inputTensor = new DenseTensor<float>(inputData, inputShape);

            var inputs = new List<NamedOnnxValue>
        {
            NamedOnnxValue.CreateFromTensor("input", inputTensor),
        };

            // Run inference in background thread
            return await Task.Run(() =>
            {
                using IDisposableReadOnlyCollection<DisposableNamedOnnxValue> results = session.Run(inputs);
                Tensor<float> outputTensor = results[0].AsTensor<float>();

                float[] predictions = [.. outputTensor];

                Log.Debug("Prediction completed for model {ModelPath} with {InputFeatures} input features", modelPath, inputData.Length);

                return predictions;
            });
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error during prediction with model {ModelPath}", modelPath);
            throw;
        }
    }

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
}
