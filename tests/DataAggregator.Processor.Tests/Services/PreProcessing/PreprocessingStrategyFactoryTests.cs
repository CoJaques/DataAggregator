using DataAggregator.Processor.Services.PreProcessing;
using DataAggregator.Processor.Services.PreProcessing.ActuatorMergingCurrentPreprocessing;

namespace DataAggregator.Processor.Tests.Services.PreProcessing;

/// <summary>
/// Tests for the <see cref="PreprocessingStrategyFactory"/> class.
/// </summary>
public class PreprocessingStrategyFactoryTests
{
    private readonly PreprocessingStrategyFactory _factory;

    /// <summary>
    /// Initializes a new instance of the <see cref="PreprocessingStrategyFactoryTests"/> class.
    /// </summary>
    public PreprocessingStrategyFactoryTests() => _factory = new PreprocessingStrategyFactory();

    #region CreateStrategy tests

    [Fact]
    public void CreateStrategy_ShouldReturnGoodStrategy()
    {
        string strategyName = "actuatorcurrent";

        IPreprocessingStrategy strategy = _factory.CreateStrategy(strategyName);

        Assert.NotNull(strategy);
        Assert.IsType<ActuatorCurrentFeatureExtractor>(strategy);
    }

    [Fact]
    public void CreateStrategy_ShouldThrowArgumentException_WhenStrategyNameIsEmpty()
    {
        string strategyName = string.Empty;

        ArgumentException exception = Assert.Throws<ArgumentException>(() => _factory.CreateStrategy(strategyName));
    }

    [Fact]
    public void CreateStrategy_ShouldThrowArgumentException_WhenStrategyNameIsUnknown()
    {
        string strategyName = "UnknownStrategy";

        ArgumentException exception = Assert.Throws<ArgumentException>(() => _factory.CreateStrategy(strategyName));
    }

    #endregion
}
