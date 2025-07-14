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
    /// Gets or sets the path to the ONNX model file.
    /// </summary>
    public string ModelPath { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the preprocessing strategy name.
    /// </summary>
    public string PreprocessingStrategy { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the list of input sensor names.
    /// </summary>
    public List<string> InputSensors { get; set; } = [];

    /// <summary>
    /// Gets or sets the name of the prediction sensor.
    /// </summary>
    public string PredictionSensorName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the window size in seconds for data collection.
    /// </summary>
    public int WindowSizeSeconds { get; set; } = 60;

    /// <summary>
    /// Gets or sets the cycle interval in seconds for this machine.
    /// </summary>
    public int CycleIntervalSeconds { get; set; } = 1;

    /// <summary>
    /// Gets or sets the preprocessing configuration for Z-score normalization.
    /// </summary>
    public PreprocessingConfig Preprocessing { get; set; } = new();
}
