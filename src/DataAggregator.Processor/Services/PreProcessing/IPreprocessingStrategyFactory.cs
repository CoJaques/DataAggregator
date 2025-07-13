namespace DataAggregator.Processor.Services.PreProcessing;

/// <summary>
/// Factory interface for creating preprocessing strategies based on strategy name.
/// </summary>
public interface IPreprocessingStrategyFactory
{
    /// <summary>
    /// Creates a preprocessing strategy based on the strategy name.
    /// </summary>
    /// <param name="strategyName">Name of the strategy to create.</param>
    /// <returns>Configured preprocessing strategy.</returns>
    public IPreprocessingStrategy CreateStrategy(string strategyName);
}
