using DataAggregator.Collector.DataCollector.Abstraction;
using DataAggregator.Collector.DataCollector.DataStorage;
using DataAggregator.Collector.DataCollector.LocalStorage;
using DataAggregator.Collector.DataCollector.Models;
using DataAggregator.Collector.DataCollector.Registration;
using DataAggregator.Shared.Domain.DataType;
using Serilog;

namespace DataAggregator.Collector.DataCollector.Connectors.OpenCN;

// TODO CJS -> To clean

/// <summary>
/// Collector service implementation for OpenCN.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="OpenCnCollectorService"/> class.
/// </remarks>
/// <param name="dataSourceConnector">The data source connector.</param>
/// <param name="dataRepository">The data repository.</param>
/// <param name="initializationService">The collector initialization service.</param>
/// <param name="dataBufferService">The data buffer service.</param>
/// <param name="configuration">The OpenCN collector configuration.</param>
public class OpenCnCollectorService(
    IDataSourceConnector dataSourceConnector,
    IDataRepository dataRepository,
    CollectorInitializationService initializationService,
    DataBufferService dataBufferService,
    OpenCnCollectorConfiguration configuration) : CollectorService(dataSourceConnector, dataRepository, initializationService, dataBufferService, configuration)
{
    /// <summary>
    /// Validates OpenCN data to ensure it meets specific criteria.
    /// </summary>
    /// <param name="data">The measurement data to validate.</param>
    /// <returns>True if the data is valid; otherwise, false.</returns>
    public bool ValidateOpenCnData(IMeasurementData data)
    {
        Log.Debug("Validating OpenCN data for sensor {SensorId}", data.SensorName);

        // Find the sensor configuration matching this data
        OpenCnSensorConfig? sensorConfig = configuration.Sensors.FirstOrDefault(s => s.Name == data.SensorName);

        if (sensorConfig == null)
        {
            Log.Warning("No sensor configuration found for {SensorName}", data.SensorName);
            return false;
        }

        // Verify the data type matches the expected type
        Type expectedType = sensorConfig.DataType.GetClrType();
        if (data.ValueType != expectedType)
        {
            Log.Warning(
                "Data type mismatch for sensor {SensorName}. Expected {Expected}, got {Actual}",
                data.SensorName,
                expectedType.Name,
                data.ValueType.Name);
            return false;
        }

        // For OpenCN sensors, also check for pin configuration if it's an OpenCnSensorConfig
        if (sensorConfig is OpenCnSensorConfig openCnSensor && string.IsNullOrEmpty(openCnSensor.PinName))
        {
            Log.Warning("OpenCN sensor {SensorName} has no pin configuration", data.SensorName);
            return false;
        }

        return true;
    }

    /// <summary>
    /// Enriches the measurement data with OpenCN-specific metadata.
    /// </summary>
    /// <param name="data">The measurement data to enrich.</param>
    /// <returns>The enriched measurement data.</returns>
    public IMeasurementData EnrichWithOpenCnMetadata(IMeasurementData data)
    {
        Log.Debug("Enriching data with OpenCN metadata for sensor {SensorId}", data.SensorName);

        // Find the sensor configuration for additional metadata
        OpenCnSensorConfig? sensorConfig = configuration.Sensors.FirstOrDefault(s => s.Name == data.SensorName);

        if (sensorConfig == null)
        {
            return data; // Cannot enrich without configuration
        }

        // TODO: Implement OpenCN-specific enrichment
        // This could include adding pin information, sampling rate, or other OpenCN-specific metadata
        return data;
    }

    /// <summary>
    /// Handles OpenCN-specific logic asynchronously.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task HandleOpenCnSpecificLogic()
    {
        Log.Information("Handling OpenCN specific logic with sampling rate {SamplingRate}Hz", configuration.SamplingRate);

        // Example of using OpenCN-specific configuration
        if (!string.IsNullOrEmpty(configuration.CfgString))
        {
            Log.Debug("Processing OpenCN configuration: {CfgString}", configuration.CfgString);

            // Parse and apply configuration
        }

        // Apply sampling rate configuration
        Log.Debug("Setting sampling rate to {SamplingRate}Hz", configuration.SamplingRate);

        // Check all pin configurations
        foreach (OpenCnSensorConfig sensor in configuration.Sensors.OfType<OpenCnSensorConfig>())
        {
            Log.Debug("Configuring pin {PinName} for sensor {SensorName}", sensor.PinName, sensor.Name);

            // Apply pin-specific configuration
        }

        await Task.Delay(10); // Simulate some processing time
    }
}
