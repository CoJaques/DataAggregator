using System.Text.Json;
using DataAggregator.Processor.Services.Processing.Abstraction;

namespace DataAggregator.Processor.Services.Processing.Factory;

/// <summary>
/// Factory interface for creating processing strategies based on strategy name.
/// </summary>
public interface IDataProcessorFactory
{
    /// <summary>
    /// Create multiple data processor which correspond to the pipeline configuration.
    /// </summary>
    /// <param name="pipelineConfig">The pipeline configuration.</param>
    /// <returns>Configured pipeline strategy.</returns>
    public List<IDataProcessor> CreateProcessors(IEnumerable<JsonElement> pipelineConfig);
}
