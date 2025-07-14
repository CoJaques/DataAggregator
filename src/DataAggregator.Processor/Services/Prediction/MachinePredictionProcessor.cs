using DataAggregator.Collector.Shared.Models;
using DataAggregator.Processor.Configuration;
using DataAggregator.Processor.Services.DataStorage;
using DataAggregator.Processor.Services.PreProcessing;
using DataAggregator.Processor.Services.Registration;
using DataAggregator.Shared.DTOs;
using Serilog;

namespace DataAggregator.Processor.Services.Prediction;

/// <summary>
/// Processor for machine prediction operations.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="MachinePredictionProcessor"/> class.
/// </remarks>
/// <param name="influxRepository">The InfluxDB repository.</param>
/// <param name="registrationClient">The registration service client.</param>
/// <param name="predictionEngine">The ONNX prediction engine.</param>
/// <param name="strategyFactory">The preprocessing strategy factory.</param>
public class MachinePredictionProcessor(
    IInfluxV3Repository influxRepository,
    IRegistrationServiceClient registrationClient,
    IOnnxPredictionEngine predictionEngine,
    IPreprocessingStrategyFactory strategyFactory)
{
    // Track the last endpoint used to avoid unnecessary reinitializations
    private string? _lastEndpoint;

    /// <summary>
    /// Processes prediction for a specific machine.
    /// </summary>
    /// <param name="config">The machine prediction configuration.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task ProcessAsync(MachinePredictionConfig config)
    {
        try
        {
            Log.Debug("Starting prediction process for machine {MachineName}", config.MachineName);

            // Get collector info from registration service
            CollectorInfoDto? collectorInfo = await registrationClient.GetCollectorInfoAsync(config.MachineName);

            if (collectorInfo == null)
            {
                Log.Warning("Collector info not found for machine {MachineName}", config.MachineName);
                return;
            }

            // Validate that all required sensors are available
            var availableSensors = collectorInfo.Sensors.ToDictionary(s => s.SensorName, s => s);
            var requestedSensors = config.InputSensors.Where(s => availableSensors.ContainsKey(s)).ToList();

            if (requestedSensors.Count != config.InputSensors.Count)
            {
                IEnumerable<string> missingSensors = config.InputSensors.Except(requestedSensors);
                Log.Warning(
                    "Missing sensors for machine {MachineName}: {MissingSensors}",
                    config.MachineName,
                    string.Join(", ", missingSensors));

                if (requestedSensors.Count == 0)
                {
                    Log.Error("No valid sensors found for machine {MachineName}", config.MachineName);
                    return;
                }
            }

            // Initialize InfluxDB repository only if endpoint changed
            if (_lastEndpoint != collectorInfo.AssignedInfluxEndpoint.Endpoint)
            {
                influxRepository.InitializeAsync(
                    collectorInfo.AssignedInfluxEndpoint.Endpoint,
                    collectorInfo.AssignedInfluxEndpoint.Token,
                    "Dataggregator");

                _lastEndpoint = collectorInfo.AssignedInfluxEndpoint.Endpoint;
                Log.Debug("Reinitialized InfluxDB connection with new endpoint: {Endpoint}", _lastEndpoint);
            }

            // Get sensor info for requested sensors
            var requestedSensorInfos = requestedSensors
                .Select(sensorName => availableSensors[sensorName])
                .ToList();

            // Fetch data window with sensor type information
            List<IMeasurementData> measurements = await FetchDataWindowAsync(config, requestedSensorInfos);

            if (measurements.Count == 0)
            {
                Log.Warning("No measurements found for machine {MachineName} in the specified time window", config.MachineName);
                return;
            }

            // Preprocess data using strategy
            float[] preprocessedData = PreprocessDataAsync(measurements, config);

            if (preprocessedData == null || preprocessedData.Length == 0)
            {
                Log.Warning("Data preprocessing failed for machine {MachineName}", config.MachineName);
                return;
            }

            // Perform prediction
            float[] predictions = await predictionEngine.PredictAsync(config.ModelPath, preprocessedData);

            // Create prediction measurement
            IMeasurementData predictionMeasurement = CreatePredictionMeasurementAsync(predictions, config);

            // Write prediction to InfluxDB
            await influxRepository.WriteMeasurementAsync(config.MachineName, predictionMeasurement);

            Log.Information(
                "Prediction completed for machine {MachineName}: {PredictionValue}",
                config.MachineName,
                predictionMeasurement.GetRawValue());
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error processing prediction for machine {MachineName}", config.MachineName);
            throw;
        }
    }

    /// <summary>
    /// Fetches data window for prediction.
    /// </summary>
    /// <param name="config">The machine prediction configuration.</param>
    /// <param name="sensors">The list of available sensors with type information.</param>
    /// <returns>A list of measurement data.</returns>
    private async Task<List<IMeasurementData>> FetchDataWindowAsync(MachinePredictionConfig config, List<SensorInfoDto> sensors)
    {
        DateTime endTime = DateTime.UtcNow;
        DateTime startTime = endTime.AddSeconds(-config.WindowSizeSeconds);

        return await influxRepository.QueryMeasurementsAsync(
            config.MachineName,
            startTime,
            endTime,
            sensors);
    }

    /// <summary>
    /// Preprocesses data using the configured strategy.
    /// </summary>
    /// <param name="measurements">The list of measurements.</param>
    /// <param name="config">The machine prediction configuration.</param>
    /// <returns>The preprocessed data as a float array for a single sample.</returns>
    private float[] PreprocessDataAsync(List<IMeasurementData> measurements, MachinePredictionConfig config)
    {
        try
        {
            if (string.IsNullOrEmpty(config.PreprocessingStrategy))
            {
                Log.Error("No preprocessing strategy configured for machine {MachineName}", config.MachineName);
                return [];
            }

            IPreprocessingStrategy strategy = strategyFactory.CreateStrategy(config.PreprocessingStrategy);
            float[] preprocessedData = strategy.PreprocessAsync(measurements, config);

            Log.Debug(
                "Preprocessed data for machine {MachineName} using strategy {Strategy}: {Features} features",
                config.MachineName,
                config.PreprocessingStrategy,
                preprocessedData.Length);

            return preprocessedData;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error preprocessing data for machine {MachineName}", config.MachineName);
            return Array.Empty<float>();
        }
    }

    /// <summary>
    /// Creates a prediction measurement from the model output.
    /// </summary>
    /// <param name="predictions">The prediction results.</param>
    /// <param name="config">The machine prediction configuration.</param>
    /// <returns>The prediction measurement.</returns>
    private IMeasurementData CreatePredictionMeasurementAsync(float[] predictions, MachinePredictionConfig config)
    {
        // For simplicity, we'll use the first prediction value
        // In a real scenario, you might want to handle multiple outputs differently
        float predictionValue = predictions.Length > 0 ? predictions[0] : 0.0f;

        return new MeasurementData<float>(
            DateTime.UtcNow,
            config.PredictionSensorName,
            predictionValue);
    }
}
