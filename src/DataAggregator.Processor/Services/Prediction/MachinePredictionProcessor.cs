using System.Text.Json;
using DataAggregator.Collector.Shared.Models;
using DataAggregator.Processor.Configuration;
using DataAggregator.Processor.Services.DataStorage;
using DataAggregator.Processor.Services.Processing.Abstraction;
using DataAggregator.Processor.Services.Processing.Factory;
using DataAggregator.Processor.Services.Registration;
using DataAggregator.Shared.DTOs;
using Serilog;

namespace DataAggregator.Processor.Services.Prediction;

/// <inheritdoc/>
public class MachinePredictionProcessor(
    IDataRepository influxRepository,
    IRegistrationServiceClient registrationClient,
    IDataProcessorFactory processorFactory) : IMachinePredictionProcessor
{
    /// <inheritdoc/>
    public async Task ProcessAsync(MachinePredictionConfig config)
    {
        try
        {
            Log.Debug("Starting prediction process for machine {MachineName}", config.MachineName);

            CollectorInfoDto? collectorInfo = await FetchCollectorInfo(config.MachineName);
            if (collectorInfo == null) return;

            List<SensorInfoDto>? requestedSensors = ValidateAndGetRequestedSensors(config, collectorInfo);
            if (requestedSensors == null) return;

            InitializeRepositoryIfNeeded(collectorInfo);

            List<IMeasurementData> measurements = await FetchDataWindowAsync(config, requestedSensors);
            if (measurements.Count == 0)
            {
                Log.Warning("No measurements found for machine {MachineName} in the specified time window", config.MachineName);
                return;
            }

            if (!BuildPipelineIfNeeded(config))
            {
                Log.Error("Failed to build processing pipeline for machine {MachineName}", config.MachineName);
                return;
            }

            IEnumerable<IMeasurementData>? processedData = await RunPipelineAsync(measurements, config.MachineName);
            if (processedData == null) return;

            await influxRepository.WriteMeasurementAsync(config.MachineName, processedData);
            Log.Information("Prediction pipeline completed for machine {MachineName}", config.MachineName);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error processing prediction for machine {MachineName}", config.MachineName);
            throw;
        }
    }

    #region Private methods
    private async Task<CollectorInfoDto?> FetchCollectorInfo(string machineName)
    {
        CollectorInfoDto? collectorInfo = await registrationClient.GetCollectorInfoAsync(machineName);
        if (collectorInfo == null)
            Log.Warning("Collector info not found for machine {MachineName}", machineName);
        return collectorInfo;
    }

    private List<SensorInfoDto>? ValidateAndGetRequestedSensors(MachinePredictionConfig config, CollectorInfoDto collectorInfo)
    {
        var availableSensors = collectorInfo.Sensors.ToDictionary(s => s.SensorName);
        var requestedSensors = config.InputSensors.Where(availableSensors.ContainsKey).ToList();

        if (requestedSensors.Count != config.InputSensors.Count)
        {
            IEnumerable<string> missing = config.InputSensors.Except(requestedSensors);
            Log.Warning("Missing sensors for machine {MachineName}: {MissingSensors}", config.MachineName, string.Join(", ", missing));
            if (requestedSensors.Count == 0)
            {
                Log.Error("No valid sensors found for machine {MachineName}", config.MachineName);
                return null;
            }
        }

        return requestedSensors.Select(s => availableSensors[s]).ToList();
    }

    private void InitializeRepositoryIfNeeded(CollectorInfoDto collectorInfo)
    {
        if (_lastEndpoint != collectorInfo.AssignedInfluxEndpoint.Endpoint)
        {
            influxRepository.Initialize(
                collectorInfo.AssignedInfluxEndpoint.Endpoint,
                collectorInfo.AssignedInfluxEndpoint.Token);
            _lastEndpoint = collectorInfo.AssignedInfluxEndpoint.Endpoint;
            Log.Debug("Reinitialized InfluxDB connection with new endpoint: {Endpoint}", _lastEndpoint);
        }
    }

    private async Task<IEnumerable<IMeasurementData>?> RunPipelineAsync(IEnumerable<IMeasurementData> data, string machineName)
    {
        foreach (IDataProcessor processor in _pipelineProcessors!)
        {
            try
            {
                data = await processor.ProcessAsync(data);
                if (data is null || !data.Any())
                {
                    Log.Warning("Processor {Processor} returned no data for machine {MachineName}", processor.GetType().Name, machineName);
                    return null;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error in processor {Processor} for machine {MachineName}", processor.GetType().Name, machineName);
                return null;
            }
        }

        return data;
    }

    private bool BuildPipelineIfNeeded(MachinePredictionConfig config)
    {
        if (_pipelineProcessors == null)
        {
            if (config.ProcessingPipeline == null)
            {
                Log.Error("No processing pipeline configured for machine {MachineName}", config.MachineName);
                return false;
            }

            string pipelineJson = JsonSerializer.Serialize(config.ProcessingPipeline);
            var pipelineElements = JsonDocument.Parse(pipelineJson).RootElement.EnumerateArray().ToList();
            _pipelineProcessors = processorFactory.CreateProcessors(pipelineElements);
        }

        return true;
    }

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
    #endregion

    #region Private fields
    private List<IDataProcessor>? _pipelineProcessors;
    private string? _lastEndpoint;
    #endregion
}
