using DataAggregator.Processor.Services.Processing.Abstraction;
using DataAggregator.Processor.Services.Processing.Onnx;
using DataAggregator.Processor.Services.Processing.PostProcessing.StateDeductionPostProcess;
using DataAggregator.Processor.Services.Processing.PreProcessing.ActuatorMergingCurrentPreprocessing;

namespace DataAggregator.Processor.Services.Processing.Factory;

/// <summary>
/// Defines a factory for creating data processors based on a pipeline description.
/// </summary>
public class DataProcessorFactory : IDataProcessorFactory
{
    /// <inheritdoc/>
    public List<IDataProcessor> CreateProcessors(IEnumerable<ProcessorDescription> pipeline)
    {
        var processors = new List<IDataProcessor>();
        foreach (ProcessorDescription desc in pipeline)
        {
            switch (desc.Name.ToLowerInvariant())
            {
                case "actuatorcurrent":
                    if (desc.Configuration is PreprocessingConfig preConfig)
                        processors.Add(new ActuatorCurrentFeatureExtractor(preConfig));
                    else
                        throw new ArgumentException("Invalid config type for actuatorcurrent");
                    break;
                case "onnxprediction":
                    if (desc.Configuration is OnnxPredictionConfig onnxConfig)
                        processors.Add(new OnnxPredictionEngine(onnxConfig));
                    else
                        throw new ArgumentException("Invalid config type for onnxprediction");
                    break;
                case "statedeductionpostprocessor":
                    if (desc.Configuration is StateDeductionPostProcessorConfig stateConfig)
                        processors.Add(new StateDeductionPostProcessor(stateConfig));
                    else
                        throw new ArgumentException("Invalid config type for statedeductionpostprocessor");
                    break;
                default:
                    throw new ArgumentException($"Unknown processor name: {desc.Name}");
            }
        }

        return processors;
    }
}
