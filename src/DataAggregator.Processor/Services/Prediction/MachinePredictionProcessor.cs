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

            IEnumerable<IReadOnlyList<IMeasurementData>> measurements = await FetchDataWindowAsync(config, requestedSensors);
            if (!measurements.Any())
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

    private async Task<IReadOnlyList<IMeasurementData>> RunPipelineAsync(
        IEnumerable<IReadOnlyList<IMeasurementData>> dataBlocks,
        string machineName)
    {
        if (_pipelineProcessors is null || _pipelineProcessors.Count == 0)
        {
            Log.Error("No processing pipeline configured for machine {MachineName}", machineName);
            return Array.Empty<IMeasurementData>();
        }

        var results = new List<IMeasurementData>();

        foreach (IReadOnlyList<IMeasurementData> block in dataBlocks)
        {
            IEnumerable<IMeasurementData>? current = block;

            foreach (IDataProcessor processor in _pipelineProcessors)
            {
                try
                {
                    current = await processor.ProcessAsync(current);
                    if (current is null || !current.Any())
                    {
                        Log.Warning("Processor {Processor} returned no data for machine {MachineName} (block skipped)", processor.GetType().Name, machineName);
                        current = null;
                        break;
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "Error in processor {Processor} for machine {MachineName} (block skipped)", processor.GetType().Name, machineName);
                    current = null;
                    break;
                }
            }

            if (current is not null)
            {
                results.AddRange(current);
            }
        }

        return results;
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

            _pipelineProcessors = processorFactory.CreateProcessors(config.ProcessingPipeline);
        }

        return true;
    }

    private async Task<IEnumerable<IReadOnlyList<IMeasurementData>>> FetchDataWindowAsync(MachinePredictionConfig config, List<SensorInfoDto> sensors)
    {
        List<List<IMeasurementData>> measurements = [];

        // If the window size is in secondes, simply query the measurements for the last N seconds.
        if (config.WindowSizeInSeconds)
        {
            DateTime endTime = DateTime.UtcNow;
            DateTime startTime = endTime.AddSeconds(-config.WindowSize);

            List<IMeasurementData> datas = await influxRepository.QueryMeasurementsAsync(
                config.MachineName,
                startTime,
                endTime,
                sensors);

            measurements.Add(datas);
        }
        else
        {
            // If we want to fetch the last N measurements, we must take inaccount the number of sensors
            int windowSize = config.WindowSize * config.InputSensors.Count;

            // Otherwise, query all the measurements since the last query, and slice it in windows size list
            if (_lastQueryTime == DateTime.MinValue)
            {
                // For the first run, we assume we want all the element for the CycleIntervalSeconds period.
                _lastQueryTime = DateTime.UtcNow.AddSeconds(-config.CycleIntervalSeconds);
            }

            List<IMeasurementData> datas = await influxRepository.QueryMeasurementsAsync(
                config.MachineName,
                _lastQueryTime,
                DateTime.UtcNow,
                sensors);

            if (datas.Count < windowSize)
                return measurements;

            datas = [.. datas.OrderBy(d => d.TimeStamp)];

            int fullBlockCount = datas.Count / windowSize;

            // Slice the data into windows of size windowSize.
            for (int i = 0; i < fullBlockCount; i++)
            {
                List<IMeasurementData> block = datas.GetRange(i * windowSize, windowSize);
                measurements.Add(block);
            }

            // Set the last query time to the maximum timestamp of the fetched data complete block,
            // so that the next query will only fetch new data.
            int lastProcessedIndex = (fullBlockCount * windowSize) - 1;
            _lastQueryTime = datas[lastProcessedIndex].TimeStamp;
        }

        Log.Debug("Fetched {Count} data blocks for machine {MachineName}", measurements.Count, config.MachineName);

        return measurements;
    }
    #endregion

    #region Private fields
    private List<IDataProcessor>? _pipelineProcessors;
    private string? _lastEndpoint;
    private DateTime _lastQueryTime = DateTime.MinValue;
    #endregion
}
