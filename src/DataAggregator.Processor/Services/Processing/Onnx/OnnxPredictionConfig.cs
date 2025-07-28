using DataAggregator.Processor.Services.Processing.Abstraction;

namespace DataAggregator.Processor.Services.Processing.Onnx;

/// <summary>
/// Configuration for ONNX prediction.
/// </summary>
public class OnnxPredictionConfig : IProcessorConfiguration
{
    /// <summary>
    /// Gets or sets the path of the model.
    /// </summary>
    public string ModelPath { get; set; } = string.Empty;
}
