using DataAggregator.Processor.Services.PreProcessing.ActuatorMergingCurrentPreprocessing;
using Serilog;

namespace DataAggregator.Processor.Services.PreProcessing;

/// <summary>
/// Factory implementation for creating preprocessing strategies.
/// </summary>
public class PreprocessingStrategyFactory : IPreprocessingStrategyFactory
{
    /// <summary>
    /// Creates a preprocessing strategy based on the strategy name.
    /// </summary>
    /// <param name="strategyName">Name of the strategy to create.</param>
    /// <returns>Configured preprocessing strategy.</returns>
    public IPreprocessingStrategy CreateStrategy(string strategyName)
    {
        Log.Information("Creating preprocessing strategy: {StrategyName}", strategyName);

        return strategyName.ToLowerInvariant() switch
        {
            "actuatorcurrent" => new ActuatorCurrentFeatureExtractor(),
            _ => throw new ArgumentException($"Unknown preprocessing strategy: {strategyName}", nameof(strategyName)),
        };
    }
}
