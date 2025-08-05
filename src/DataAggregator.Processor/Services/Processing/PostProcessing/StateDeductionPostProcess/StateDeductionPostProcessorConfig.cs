using DataAggregator.Processor.Services.Processing.Abstraction;

namespace DataAggregator.Processor.Services.Processing.PostProcessing.StateDeductionPostProcess;

/// <summary>
/// Configuration for <see cref="StateDeductionPostProcessor"/>.
/// </summary>
public class StateDeductionPostProcessorConfig : IProcessorConfiguration
{
    /// <summary>
    /// Gets or sets the threshold for minimum number of consecutive cycles required to deduce a state change.
    /// </summary>
    public int Threshold { get; set; }
}
