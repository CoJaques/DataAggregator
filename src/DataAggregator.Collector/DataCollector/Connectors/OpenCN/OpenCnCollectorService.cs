using DataAggregator.Collector.DataCollector.Abstraction;
using DataAggregator.Collector.DataCollector.DataStorage;
using DataAggregator.Collector.DataCollector.LocalStorage;
using DataAggregator.Collector.DataCollector.Models;
using DataAggregator.Collector.DataCollector.Registration;
using Serilog;

namespace DataAggregator.Collector.DataCollector.Connectors.OpenCN;

/// <summary>
/// Collector service implementation for OpenCN.
/// </summary>
public class OpenCnCollectorService : CollectorService
{
    private readonly OpenCnCollectorConfiguration _openCnConfiguration;

    /// <summary>
    /// Initializes a new instance of the <see cref="OpenCnCollectorService"/> class.
    /// </summary>
    /// <param name="dataSourceConnector">The data source connector.</param>
    /// <param name="dataRepository">The data repository.</param>
    /// <param name="initializationService">The collector initialization service.</param>
    /// <param name="dataBufferService">The data buffer service.</param>
    /// <param name="configuration">The OpenCN collector configuration.</param>
    public OpenCnCollectorService(
        IDataSourceConnector dataSourceConnector,
        IDataRepository dataRepository,
        CollectorInitializationService initializationService,
        DataBufferService dataBufferService,
        OpenCnCollectorConfiguration configuration)
        : base(dataSourceConnector, dataRepository, initializationService, dataBufferService, configuration)
    {
        _openCnConfiguration = configuration;
    }

    /// <summary>
    /// Validates OpenCN data to ensure it meets specific criteria.
    /// </summary>
    /// <param name="data">The measurement data to validate.</param>
    /// <returns>True if the data is valid; otherwise, false.</returns>
    public bool ValidateOpenCnData(IMeasurementData data)
    {
        Log.Debug("Validating OpenCN data for sensor {SensorId}", data.SensorName);

        // Find the sensor configuration matching this data
        var sensorConfig = _openCnConfiguration.Sensors.FirstOrDefault(s => s.Name == data.SensorName);

        if (sensorConfig == null)
        {
            Log.Warning("No sensor configuration found for {SensorName}", data.SensorName);
            return false;
        }

        // Verify the data type matches the expected type
        Type expectedType = sensorConfig.GetClrType();
        if (data.ValueType != expectedType)
        {
            Log.Warning("Data type mismatch for sensor {SensorName}. Expected {Expected}, got {Actual}",
                data.SensorName, expectedType.Name, data.ValueType.Name);
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
        var sensorConfig = _openCnConfiguration.Sensors.FirstOrDefault(s => s.Name == data.SensorName);

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
        Log.Information("Handling OpenCN specific logic with sampling rate {SamplingRate}Hz", _openCnConfiguration.SamplingRate);

        // Example of using OpenCN-specific configuration
        if (!string.IsNullOrEmpty(_openCnConfiguration.CfgString))
        {
            Log.Debug("Processing OpenCN configuration: {CfgString}", _openCnConfiguration.CfgString);
            // Parse and apply configuration
        }

        // Apply sampling rate configuration
        Log.Debug("Setting sampling rate to {SamplingRate}Hz", _openCnConfiguration.SamplingRate);

        // Check all pin configurations
        foreach (var sensor in _openCnConfiguration.Sensors.OfType<OpenCnSensorConfig>())
        {
            Log.Debug("Configuring pin {PinName} for sensor {SensorName}", sensor.PinName, sensor.Name);
            // Apply pin-specific configuration
        }

        await Task.Delay(10); // Simulate some processing time
    }
}
