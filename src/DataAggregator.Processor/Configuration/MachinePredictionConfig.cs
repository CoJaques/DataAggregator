using DataAggregator.Processor.Services.Processing.Factory;

namespace DataAggregator.Processor.Configuration;

/// <summary>
/// Configuration for a specific machine prediction.
/// </summary>
public class MachinePredictionConfig
{
    /// <summary>
    /// Gets or sets the machine name.
    /// </summary>
    public string MachineName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a value indicating whether prediction is enabled for this machine.
    /// </summary>
    public bool Enabled { get; set; } = true;

    /// <summary>
    /// Gets or sets the list of input sensor names.
    /// </summary>
    public List<string> InputSensors { get; set; } = [];

    /// <summary>
    /// Gets or sets the window size in seconds for data collection.
    /// </summary>
    public int WindowSizeSeconds { get; set; } = 60;

    /// <summary>
    /// Gets or sets the cycle interval in seconds for this machine.
    /// </summary>
    public int CycleIntervalSeconds { get; set; } = 1;

    /// <summary>
    /// Gets or sets the processing pipeline for this machine.
    /// </summary>
    public List<ProcessorDescription> ProcessingPipeline { get; set; } = new();
}
