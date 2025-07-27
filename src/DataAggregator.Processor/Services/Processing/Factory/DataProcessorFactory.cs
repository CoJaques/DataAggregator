using System.Text.Json;
using DataAggregator.Processor.Services.Prediction;
using DataAggregator.Processor.Services.Processing.Abstraction;
using DataAggregator.Processor.Services.Processing.Onnx;
using DataAggregator.Processor.Services.Processing.PostProcessing.StateDeductionPostProcess;
using DataAggregator.Processor.Services.Processing.PreProcessing.ActuatorMergingCurrentPreprocessing;

namespace DataAggregator.Processor.Services.Processing.Factory;

/// <summary>
/// Factory implementation for creating processor strategies.
/// </summary>
public class DataProcessorFactory : IDataProcessorFactory
{
    /// <inheritdoc/>
    public List<IDataProcessor> CreateProcessors(IEnumerable<JsonElement> pipelineConfig)
    {
        var processors = new List<IDataProcessor>();
        foreach (JsonElement element in pipelineConfig)
        {
            if (!element.TryGetProperty("Strategy", out JsonElement strategyProp))
                throw new ArgumentException("Each processor config must have a 'Strategy' property.");

            string? strategy = strategyProp.GetString()?.ToLowerInvariant();

            switch (strategy)
            {
                case "actuatorcurrent":
                    PreprocessingConfig? preConfig = element.Deserialize<PreprocessingConfig>();
                    if (preConfig == null)
                        throw new ArgumentException("Preprocessing configuration is required for ActuatorCurrentFeatureExtractor.");
                    processors.Add(new ActuatorCurrentFeatureExtractor(preConfig));
                    break;
                case "onnxprediction":
                    OnnxPredictionConfig? onnxConfig = element.Deserialize<OnnxPredictionConfig>();
                    if (onnxConfig == null)
                        throw new ArgumentException("ONNX prediction configuration is required.");
                    processors.Add(new OnnxPredictionEngine(onnxConfig));
                    break;
                case "statedeductionpostprocessor":
                    StateDeductionPostProcessorConfig? postConfig = element.Deserialize<StateDeductionPostProcessorConfig>();
                    if (postConfig == null)
                        throw new ArgumentException("Post-processing configuration is required for MyCustomPostProcessor.");
                    processors.Add(new StateDeductionPostProcessor(postConfig));
                    break;
                default:
                    throw new ArgumentException($"Unknown processor strategy: {strategy}");
            }
        }

        return processors;
    }
}
