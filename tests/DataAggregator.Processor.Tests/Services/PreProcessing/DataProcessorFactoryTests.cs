using DataAggregator.Processor.Configuration;
using DataAggregator.Processor.Services.Processing.Factory;
using DataAggregator.Processor.Services.Processing.PreProcessing.ActuatorMergingCurrentPreprocessing;
using DataAggregator.Processor.Services.Prediction;
using DataAggregator.Processor.Services.Processing.PostProcessing.StateDeductionPostProcess;
using System.Text.Json;

namespace DataAggregator.Processor.Tests.Services.PreProcessing;

public class DataProcessorFactoryTests
{
    private readonly DataProcessorFactory _factory;

    public DataProcessorFactoryTests() => _factory = new DataProcessorFactory();

    [Fact]
    public void CreateProcessors_ShouldReturnCorrectProcessors_ForValidPipeline()
    {
        // Arrange
        string pipelineJson = @"[
            { ""Strategy"": ""actuatorcurrent"", ""EnableZScoreNormalization"": true, ""NormalizationParameters"": {} },
            { ""Strategy"": ""onnxprediction"", ""ModelPath"": ""model.onnx"" },
            { ""Strategy"": ""statedeductionpostprocessor"", ""Threshold"": 2 }
        ]";
        var pipelineElements = JsonDocument.Parse(pipelineJson).RootElement.EnumerateArray().ToList();

        // Act
        var processors = _factory.CreateProcessors(pipelineElements);

        // Assert
        Assert.Equal(3, processors.Count);
        Assert.IsType<ActuatorCurrentFeatureExtractor>(processors[0]);
        Assert.IsType<OnnxPredictionEngine>(processors[1]);
        Assert.IsType<StateDeductionPostProcessor>(processors[2]);
    }

    [Fact]
    public void CreateProcessors_ShouldThrowArgumentException_ForUnknownStrategy()
    {
        // Arrange
        string pipelineJson = @"[
            { ""Strategy"": ""unknownstrategy"" }
        ]";
        var pipelineElements = JsonDocument.Parse(pipelineJson).RootElement.EnumerateArray().ToList();

        // Act & Assert
        Assert.Throws<ArgumentException>(() => _factory.CreateProcessors(pipelineElements));
    }

    [Fact]
    public void CreateProcessors_ShouldReturnEmptyList_ForEmptyPipeline()
    {
        // Arrange
        string pipelineJson = "[]";
        var pipelineElements = JsonDocument.Parse(pipelineJson).RootElement.EnumerateArray().ToList();

        // Act
        var processors = _factory.CreateProcessors(pipelineElements);

        // Assert
        Assert.Empty(processors);
    }

    [Fact]
    public void CreateProcessors_ShouldThrowArgumentException_WhenStrategyMissing()
    {
        // Arrange
        string pipelineJson = @"[
            { ""EnableZScoreNormalization"": true }
        ]";
        var pipelineElements = JsonDocument.Parse(pipelineJson).RootElement.EnumerateArray().ToList();

        // Act & Assert
        Assert.Throws<ArgumentException>(() => _factory.CreateProcessors(pipelineElements));
    }
}
