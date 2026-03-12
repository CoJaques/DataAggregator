using System.Text.Json;
using DataAggregator.Collector.HttpCollector.Configuration;
using DataAggregator.Collector.HttpCollector.Connector;
using DataAggregator.Collector.HttpCollector.Models;
using DataAggregator.Collector.Shared.Models;
using DataAggregator.Shared.Domain.DataType;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Serilog;

namespace DataAggregator.Collector.HttpCollector.Controllers;

/// <summary>
/// Controller for receiving data via HTTP Push.
/// This controller is the entry point for "Push" machines.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="HttpCollectorController"/> class.
/// </remarks>
/// <param name="connector">The HTTP connector singleton bridge.</param>
/// <param name="options">The HTTP collector configuration.</param>
[ApiController]
[Route("api/[controller]")]
public class HttpCollectorController(
    HttpConnector? connector = null,
    IOptions<HttpCollectorConfiguration>? options = null) : ControllerBase
{
    #region Private Fields
    private readonly HttpConnector? _connector = connector;
    private readonly HttpCollectorConfiguration? _config = options?.Value;
    #endregion

    #region Public Methods

    /// <summary>
    /// Receives measurement data via HTTP POST.
    /// </summary>
    /// <param name="measurements">The list of measurements to collect.</param>
    /// <returns>A result indicating success or failure.</returns>
    [HttpPost("collect")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    [ProducesResponseType(503)] // Added 503 for when collector is off
    public IActionResult CollectData([FromBody] IEnumerable<HttpMeasurementDto> measurements)
    {
        // 1. Check if the collector is enabled and the bridge is present
        if (_connector == null || _config == null)
        {
            Log.Warning("HTTP Collector called but service is not enabled in configuration.");
            return BadRequest("HTTP collector is not enabled on this instance.");
        }

        // 2. Validate measurements list
        if (measurements == null || !measurements.Any())
        {
            return BadRequest("No measurements provided in the request body.");
        }

        try
        {
            // 3. Convert DTOs to internal model with validation
            var internalMeasurements = ProcessAndValidateMeasurements(measurements);

            if (internalMeasurements.Count == 0)
            {
                Log.Debug("No valid measurements after validation for {DeviceName}.", _config.DeviceName);
                return Ok(); // Could also be BadRequest if we want to be strict
            }

            // 4. Try to push data to the bridge (Channel)
            bool accepted = _connector.TryEnqueueData(internalMeasurements);

            if (!accepted)
            {
                Log.Warning("HTTP data rejected for {DeviceName}. The collector is currently disconnected or shutting down.", _config.DeviceName);
                return StatusCode(503, "Service is currently not accepting data. Please try again later.");
            }

            return Ok();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Unexpected error processing incoming HTTP data for {DeviceName}.", _config.DeviceName);
            return StatusCode(500, "Internal error processing measurements.");
        }
    }

    #endregion

    #region Private Methods

    private List<IMeasurementData> ProcessAndValidateMeasurements(IEnumerable<HttpMeasurementDto> measurements)
    {
        var result = new List<IMeasurementData>();

        foreach (HttpMeasurementDto m in measurements)
        {
            // Simple validation against configured sensors
            if (_config!.Sensors != null && _config.Sensors.Count > 0)
            {
                if (!_config.Sensors.Any(s => s.Name.Equals(m.SensorName, StringComparison.OrdinalIgnoreCase)))
                {
                    Log.Warning("Rejected data for unknown sensor: {SensorName} for device {DeviceName}.", m.SensorName, _config.DeviceName);
                    continue;
                }
            }

            DateTime timestamp = m.TimeStamp ?? DateTime.UtcNow;
            IMeasurementData? internalM = m.DataType switch
            {
                SensorDataType.Boolean => new MeasurementData<bool>(timestamp, m.SensorName, ConvertTo<bool>(m.Value)),
                SensorDataType.Integer => new MeasurementData<int>(timestamp, m.SensorName, ConvertTo<int>(m.Value)),
                SensorDataType.Double => new MeasurementData<double>(timestamp, m.SensorName, ConvertTo<double>(m.Value)),
                SensorDataType.Float => new MeasurementData<double>(timestamp, m.SensorName, ConvertTo<double>(m.Value)),
                SensorDataType.String => new MeasurementData<string>(timestamp, m.SensorName, m.Value.ToString() ?? string.Empty),
                _ => throw new NotSupportedException($"Data type {m.DataType} is not supported.")
            };

            if (internalM != null)
            {
                result.Add(internalM);
            }
        }

        return result;
    }

    private static T ConvertTo<T>(object value)
    {
        if (value is JsonElement element)
        {
            return element.Deserialize<T>()!;
        }

        return (T)Convert.ChangeType(value, typeof(T));
    }

    #endregion
}
