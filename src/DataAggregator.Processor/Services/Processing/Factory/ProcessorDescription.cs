using System.Text.Json.Serialization;
using DataAggregator.Processor.Services.Processing.Abstraction;

namespace DataAggregator.Processor.Services.Processing.Factory;

/// <summary>
/// Describes a processor with its name and configuration.
/// </summary>
[JsonConverter(typeof(ProcessorDescriptionJsonConverter))]
public class ProcessorDescription
{
    /// <summary>
    /// Gets or sets the name of the processor.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the configuration for the processor.
    /// </summary>
    public IProcessorConfiguration? Configuration { get; set; }
}
