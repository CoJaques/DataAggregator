using System.Text.Json.Serialization;
using DataAggregator.Shared.Domain.DataType;

namespace DataAggregator.Collector.HttpCollector.Models;

/// <summary>
/// Data transfer object for measurements received via HTTP.
/// </summary>
public class HttpMeasurementDto
{
    /// <summary>
    /// Gets or sets the name of the sensor.
    /// </summary>
    public string SensorName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the timestamp of the measurement.
    /// If not provided, the current UTC time will be used.
    /// </summary>
    public DateTime? TimeStamp { get; set; }

    /// <summary>
    /// Gets or sets the value of the measurement.
    /// Can be any type supported by the aggregator.
    /// </summary>
    public object Value { get; set; } = default!;

    /// <summary>
    /// Gets or sets the data type of the measurement.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public SensorDataType DataType { get; set; }
}
