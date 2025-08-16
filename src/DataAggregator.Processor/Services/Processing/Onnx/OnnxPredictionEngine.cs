using DataAggregator.Collector.Shared.Models;
using DataAggregator.Processor.Services.Processing.Abstraction;
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using Serilog;

namespace DataAggregator.Processor.Services.Processing.Onnx;

/// <summary>
/// Implementation of ONNX prediction engine.
/// </summary>
public class OnnxPredictionEngine(OnnxPredictionConfig config) : IDataProcessor, IDisposable
{
    #region Private fields

    private static readonly Dictionary<string, InferenceSession> _modelCache = [];

    #endregion

    #region Public methods

    /// <inheritdoc />
    public async Task<IEnumerable<IMeasurementData>> ProcessAsync(
    IEnumerable<IMeasurementData> input)
    {
        try
        {
            // Load the model or get the existing session
            string executablePath = AppContext.BaseDirectory;
            string modelPath = Path.Combine(executablePath, config.ModelPath);
            InferenceSession session = LoadOrGetModel(modelPath);

            if (!input.Any())
            {
                Log.Information($"No inputs data provided to model for model {config.ModelPath}");
                return Array.Empty<IMeasurementData>();
            }

            ValidateInputData(session, input);

            // Prepare the inputs for the inference session
            var inputs = new List<NamedOnnxValue>();
            foreach (IMeasurementData data in input)
            {
                inputs.Add(CreateNamedOnnxValue(data));
            }

            // Run the model to get results
            using IDisposableReadOnlyCollection<DisposableNamedOnnxValue> results = session.Run(inputs);

            var inputsNamesAsOutput = session.InputMetadata.Keys
                .Select(name => name + ".output")
                .ToList();

            // Filter out results containing input names or the terms "unused" or "__Features__"
            var filteredResults = results.Where(x =>
                !inputsNamesAsOutput.Any(input => input.Equals(x.Name, StringComparison.InvariantCultureIgnoreCase)) && // Remove input names
                !x.Name.Contains("unused", StringComparison.InvariantCultureIgnoreCase) && // Remove "unused" term
                !x.Name.Contains("__Features__", StringComparison.InvariantCultureIgnoreCase)) // Remove "__Features__" term
            .ToList();

            // Prepare the list to store output measurements
            var outputMeasurements = new List<IMeasurementData>();
            DateTime processedDataTime = input.First().TimeStamp;

            // Process each filtered result to convert it into measurement data
            foreach (DisposableNamedOnnxValue? result in filteredResults)
            {
                // Process the result and add it to the output list
                IEnumerable<IMeasurementData> measurementData = ProcessResultToMeasurementData(result, processedDataTime);
                outputMeasurements.AddRange(measurementData);
            }

            Log.Debug("Prediction completed for model {ModelPath} with {OutputCount} outputs", config.ModelPath, outputMeasurements.Count);
            return await Task.FromResult(outputMeasurements);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error during prediction with model {ModelPath}", config.ModelPath);
            throw;
        }
    }

    /// <summary>
    /// Processes the ONNX result into a list of IMeasurementData objects.
    /// </summary>
    /// <param name="result">The result from the ONNX model run.</param>
    /// <param name="timestamp">The timestamp for the measurement data.</param>
    /// <returns>A list of IMeasurementData containing the processed result.</returns>
    private List<IMeasurementData> ProcessResultToMeasurementData(DisposableNamedOnnxValue result, DateTime timestamp)
    {
        var measurementDataList = new List<IMeasurementData>();

        // Check if the result is of type float (tensor of floats)
        if (result.AsTensor<float>() != null)
        {
            float[] values = [.. result.AsTensor<float>()];

            // Add each float value as MeasurementData
            measurementDataList.AddRange(values.Select((value, index) =>
                new MeasurementData<float>(timestamp, $"{result.Name}_{index}", value)));
        }

        // Check if the result is of type string (tensor of strings)
        if (result.AsTensor<string>() != null)
        {
            string[] values = [.. result.AsTensor<string>()];

            // Add each string value as MeasurementData
            measurementDataList.AddRange(values.Select((value, index) =>
                new MeasurementData<string>(timestamp, $"{result.Name}_{index}", value)));
        }

        return measurementDataList;
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

        // Check if all inputData sensors exist in the model inputs
        foreach (IMeasurementData input in inputData)
        {
            if (!modelInputs.ContainsKey(input.SensorName))
            {
                throw new ArgumentException(
                    $"Model does not have an input for '{input.SensorName}' which was provided in input data.");
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

            string[] sArray => NamedOnnxValue.CreateFromTensor(name, new DenseTensor<string>(sArray, [1, sArray.Length])),
            string s => NamedOnnxValue.CreateFromTensor(name, new DenseTensor<string>(new[] { s }, [1, 1])),

            _ => throw new NotSupportedException($"Unsupported data type {data.ValueType} for sensor {name}"),
        };
    }

    #endregion
}
