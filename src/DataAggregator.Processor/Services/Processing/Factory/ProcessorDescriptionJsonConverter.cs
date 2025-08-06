using System.Text.Json;
using System.Text.Json.Serialization;
using DataAggregator.Processor.Services.Processing.Abstraction;
using DataAggregator.Processor.Services.Processing.Onnx;
using DataAggregator.Processor.Services.Processing.PostProcessing.StateDeductionPostProcess;
using DataAggregator.Processor.Services.Processing.PreProcessing.ActuatorMergingCurrentPreprocessing;

namespace DataAggregator.Processor.Services.Processing.Factory;

/// <summary>
/// Coverts <see cref="ProcessorDescription"/> to and from JSON.
/// </summary>
public class ProcessorDescriptionJsonConverter : JsonConverter<ProcessorDescription>
{
    /// <inheritdoc/>
    public override ProcessorDescription Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using var jsonDoc = JsonDocument.ParseValue(ref reader);
        JsonElement root = jsonDoc.RootElement;
        string name = root.GetProperty("Name").GetString() ?? string.Empty;
        JsonElement configElement = root.GetProperty("Configuration");
        IProcessorConfiguration? config = name.ToLowerInvariant() switch
        {
            "actuatorcurrent" => configElement.Deserialize<PreprocessingConfig>(options),
            "onnxprediction" => configElement.Deserialize<OnnxPredictionConfig>(options),
            "statedeductionpostprocessor" => configElement.Deserialize<StateDeductionPostProcessorConfig>(options),
            _ => null,
        };
        return new ProcessorDescription { Name = name, Configuration = config };
    }

    /// <inheritdoc/>
    public override void Write(Utf8JsonWriter writer, ProcessorDescription value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteString("Name", value.Name);
        writer.WritePropertyName("Configuration");
        if (value.Configuration != null)
            JsonSerializer.Serialize(writer, value.Configuration, value.Configuration.GetType(), options);
        else
            writer.WriteNullValue();
        writer.WriteEndObject();
    }
}
