using DataAggregator.Processor.Configuration;
using DataAggregator.Processor.Services.Processing.Factory;
using DataAggregator.Processor.Services.Processing.PreProcessing.ActuatorMergingCurrentPreprocessing;
using DataAggregator.Processor.Services.Prediction;
using DataAggregator.Processor.Services.Processing.PostProcessing.StateDeductionPostProcess;
using System.Collections.Generic;
using DataAggregator.Processor.Services.Processing.Onnx;

namespace DataAggregator.Processor.Tests.Services.PreProcessing;

public class DataProcessorFactoryTests
{
    private readonly DataProcessorFactory _factory;

    public DataProcessorFactoryTests() => _factory = new DataProcessorFactory();

    [Fact]
    public void CreateProcessors_ShouldReturnCorrectProcessors_ForValidPipeline()
    {
        // Arrange
        var pipeline = new List<ProcessorDescription>
        {
            new ProcessorDescription
            {
                Name = "actuatorcurrent",
                Configuration = new PreprocessingConfig
                {
                    EnableZScoreNormalization = true,
                    NormalizationParameters = new Dictionary<string, float[]>()
                }
            },
            new ProcessorDescription
            {
                Name = "onnxprediction",
                Configuration = new OnnxPredictionConfig
                {
                    ModelPath = "model.onnx"
                }
            },
            new ProcessorDescription
            {
                Name = "statedeductionpostprocessor",
                Configuration = new StateDeductionPostProcessorConfig
                {
                    Threshold = 2
                }
            }
        };

        // Act
        var processors = _factory.CreateProcessors(pipeline);

        // Assert
        Assert.Equal(3, processors.Count);
        Assert.IsType<ActuatorCurrentFeatureExtractor>(processors[0]);
        Assert.IsType<OnnxPredictionEngine>(processors[1]);
        Assert.IsType<StateDeductionPostProcessor>(processors[2]);
    }

    [Fact]
    public void CreateProcessors_ShouldThrowArgumentException_ForUnknownName()
    {
        // Arrange
        var pipeline = new List<ProcessorDescription>
        {
            new ProcessorDescription { Name = "unknownstrategy", Configuration = null }
        };

        // Act & Assert
        Assert.Throws<ArgumentException>(() => _factory.CreateProcessors(pipeline));
    }

    [Fact]
    public void CreateProcessors_ShouldReturnEmptyList_ForEmptyPipeline()
    {
        // Arrange
        var pipeline = new List<ProcessorDescription>();

        // Act
        var processors = _factory.CreateProcessors(pipeline);

        // Assert
        Assert.Empty(processors);
    }

    [Fact]
    public void CreateProcessors_ShouldThrowArgumentException_WhenNameMissing()
    {
        // Arrange
        var pipeline = new List<ProcessorDescription>
        {
            new ProcessorDescription { Name = "", Configuration = new PreprocessingConfig() }
        };

        // Act & Assert
        Assert.Throws<ArgumentException>(() => _factory.CreateProcessors(pipeline));
    }
}
