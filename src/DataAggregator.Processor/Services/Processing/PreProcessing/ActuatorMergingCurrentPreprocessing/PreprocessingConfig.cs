namespace DataAggregator.Processor.Services.Processing.PreProcessing.ActuatorMergingCurrentPreprocessing;

/// <summary>
/// Configuration for preprocessing operations including Z-score normalization.
/// </summary>
public class PreprocessingConfig
{
    /// <summary>
    /// Gets or sets a value indicating whether Z-score normalization is enabled.
    /// </summary>
    public bool EnableZScoreNormalization { get; set; } = true;

    /// <summary>
    /// Gets or sets the normalization parameters for each feature (name, [mean, standard deviation]).
    /// </summary>
    public Dictionary<string, float[]> NormalizationParameters { get; set; } = new();
}
