using DataAggregator.Collector.Shared.Models;
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

    /// <inheritdoc />
    public async Task<IEnumerable<IMeasurementData>> PredictAsync(
    string modelPath,
    IEnumerable<IMeasurementData> inputData)
    {
        try
        {
            InferenceSession session = LoadOrGetModel(modelPath);
            ValidateInputData(session, inputData);

            var inputs = new List<NamedOnnxValue>();

            foreach (IMeasurementData data in inputData)
            {
                inputs.Add(CreateNamedOnnxValue(data));
            }

            using IDisposableReadOnlyCollection<DisposableNamedOnnxValue> results = session.Run(inputs);

            var outputMeasurements = new List<IMeasurementData>();
            DateTime now = DateTime.UtcNow;

            foreach (DisposableNamedOnnxValue? result in results)
            {
                string name = result.Name;
                Tensor<float> tensor = result.AsTensor<float>();
                float[] values = [.. tensor];

                for (int i = 0; i < values.Length; i++)
                {
                    outputMeasurements.Add(
                        new MeasurementData<float>(now, $"{name}_{i}", values[i]));
                }
            }

            Log.Debug("Prediction completed for model {ModelPath} with {OutputCount} outputs", modelPath, outputMeasurements.Count);
            return await Task.FromResult(outputMeasurements);
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

    private void ValidateInputData(InferenceSession session, IEnumerable<IMeasurementData> inputData)
    {
        IReadOnlyDictionary<string, NodeMetadata> modelInputs = session.InputMetadata;

        // Check if all required model inputs are provided
        foreach (KeyValuePair<string, NodeMetadata> modelInput in modelInputs)
        {
            if (!inputData.Any(x => modelInput.Key == x.SensorName))
            {
                throw new ArgumentException(
                    $"Model requires input '{modelInput.Key}' but it was not provided. ");
            }
        }

        Log.Debug("Input validation passed for model with {InputCount} inputs", inputData.Count());
    }

    private NamedOnnxValue CreateNamedOnnxValue(IMeasurementData data)
    {
        string name = data.SensorName;
        object rawValue = data.GetRawValue();

        return rawValue switch
        {
            float[] fArray => NamedOnnxValue.CreateFromTensor(name, new DenseTensor<float>(fArray, [1, fArray.Length])),
            float f => NamedOnnxValue.CreateFromTensor(name, new DenseTensor<float>(new[] { f }, [1, 1])),

            int[] iArray => NamedOnnxValue.CreateFromTensor(name, new DenseTensor<int>(iArray, [1, iArray.Length])),
            int i => NamedOnnxValue.CreateFromTensor(name, new DenseTensor<int>(new[] { i }, [1, 1])),

            double[] dArray => NamedOnnxValue.CreateFromTensor(name, new DenseTensor<double>(dArray, [1, dArray.Length])),
            double d => NamedOnnxValue.CreateFromTensor(name, new DenseTensor<double>(new[] { d }, [1, 1])),

            _ => throw new NotSupportedException($"Unsupported data type {data.ValueType} for sensor {name}"),
        };
    }

    #endregion
}
