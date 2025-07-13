using DataAggregator.Collector.Shared.Models;
using DataAggregator.Processor.Configuration;
using DataAggregator.Processor.Services.DataStorage;
using DataAggregator.Processor.Services.PreProcessing;
using DataAggregator.Processor.Services.Registration;
using DataAggregator.Shared;
using Serilog;

namespace DataAggregator.Processor.Services.Prediction;

/// <summary>
/// Processor for machine prediction operations.
/// </summary>
public class MachinePredictionProcessor
{
    private readonly IInfluxV3Repository _influxRepository;
    private readonly IRegistrationServiceClient _registrationClient;
    private readonly IOnnxPredictionEngine _predictionEngine;
    private readonly IPreprocessingStrategyFactory _strategyFactory;

    // Track the last endpoint used to avoid unnecessary reinitializations
    private string? _lastEndpoint;

    /// <summary>
    /// Initializes a new instance of the <see cref="MachinePredictionProcessor"/> class.
    /// </summary>
    /// <param name="influxRepository">The InfluxDB repository.</param>
    /// <param name="registrationClient">The registration service client.</param>
    /// <param name="predictionEngine">The ONNX prediction engine.</param>
    /// <param name="strategyFactory">The preprocessing strategy factory.</param>
    public MachinePredictionProcessor(
        IInfluxV3Repository influxRepository,
        IRegistrationServiceClient registrationClient,
        IOnnxPredictionEngine predictionEngine,
        IPreprocessingStrategyFactory strategyFactory)
    {
        _influxRepository = influxRepository;
        _registrationClient = registrationClient;
        _predictionEngine = predictionEngine;
        _strategyFactory = strategyFactory;
    }

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

            // Get device info from registration service
            DeviceRegistrationResponse deviceInfo = await _registrationClient.GetDeviceInfoAsync(config.MachineName);

            // Initialize InfluxDB repository only if endpoint changed
            if (_lastEndpoint != deviceInfo.AssignedTimeSeriesEndpoint)
            {
                _influxRepository.InitializeAsync(
                    deviceInfo.AssignedTimeSeriesEndpoint,
                    deviceInfo.DeviceToken,
                    "Dataggregator");

                _lastEndpoint = deviceInfo.AssignedTimeSeriesEndpoint;
                Log.Debug("Reinitialized InfluxDB connection with new endpoint: {Endpoint}", _lastEndpoint);
            }

            // Fetch data window
            List<IMeasurementData> measurements = await FetchDataWindowAsync(config);

            if (measurements.Count == 0)
            {
                Log.Warning("No measurements found for machine {MachineName} in the specified time window", config.MachineName);
                return;
            }

            // Preprocess data using strategy
            float[] preprocessedData = await PreprocessDataAsync(measurements, config);

            if (preprocessedData == null || preprocessedData.Length == 0)
            {
                Log.Warning("Data preprocessing failed for machine {MachineName}", config.MachineName);
                return;
            }

            // Perform prediction
            float[] predictions = await _predictionEngine.PredictAsync(config.ModelPath, preprocessedData);

            // Create prediction measurement
            IMeasurementData predictionMeasurement = CreatePredictionMeasurementAsync(predictions, config);

            // Write prediction to InfluxDB
            await _influxRepository.WriteMeasurementAsync(config.MachineName, predictionMeasurement);

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
    /// <returns>A list of measurement data.</returns>
    private async Task<List<IMeasurementData>> FetchDataWindowAsync(MachinePredictionConfig config)
    {
        DateTime endTime = DateTime.UtcNow;
        DateTime startTime = endTime.AddSeconds(config.WindowSizeSeconds);

        return await _influxRepository.QueryMeasurementsAsync(
            config.MachineName,
            startTime,
            endTime,
            config.InputSensors);
    }

    /// <summary>
    /// Preprocesses data using the configured strategy.
    /// </summary>
    /// <param name="measurements">The list of measurements.</param>
    /// <param name="config">The machine prediction configuration.</param>
    /// <returns>The preprocessed data as a float array for a single sample.</returns>
    private async Task<float[]> PreprocessDataAsync(List<IMeasurementData> measurements, MachinePredictionConfig config)
    {
        try
        {
            if (string.IsNullOrEmpty(config.PreprocessingStrategy))
            {
                Log.Error("No preprocessing strategy configured for machine {MachineName}", config.MachineName);
                return Array.Empty<float>();
            }

            IPreprocessingStrategy strategy = _strategyFactory.CreateStrategy(config.PreprocessingStrategy);
            float[] preprocessedData = await strategy.PreprocessAsync(measurements, config);

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
