using DataAggregator.Processor.Configuration;
using DataAggregator.Processor.Services.Prediction;
using Microsoft.Extensions.Options;
using Serilog;

namespace DataAggregator.Processor.Services;

/// <summary>
/// Background service for processing machine predictions on a scheduled basis.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="PredictionBackgroundService"/> class.
/// </remarks>
/// <param name="configuration">The prediction service configuration.</param>
/// <param name="predictionProcessor">The machine prediction processor.</param>
public class PredictionBackgroundService(
    IOptions<PredictionServiceConfiguration> configuration,
    MachinePredictionProcessor predictionProcessor) : BackgroundService
{
    #region Private fields

    private readonly Dictionary<string, Timer> _machineTimers = [];
    private readonly Dictionary<string, bool> _machineErrors = [];

    #endregion

    #region Public methods

    /// <inheritdoc/>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            Log.Information("Starting prediction background service");

            // Validate configuration
            ValidateConfigurationAsync();

            // Schedule machines
            foreach (MachinePredictionConfig machineConfig in configuration.Value.Machines)
            {
                if (machineConfig.Enabled)
                {
                    ScheduleMachine(machineConfig);
                }
            }

            Log.Information(
                "Prediction background service started with {MachineCount} machines",
                configuration.Value.Machines.Count(m => m.Enabled));

            // Keep the service running
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
            }
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Fatal error in prediction background service");
            throw;
        }
    }

    /// <inheritdoc/>
    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        Log.Information("Stopping prediction background service");

        // Dispose all timers
        foreach (Timer timer in _machineTimers.Values)
        {
            timer?.Dispose();
        }

        _machineTimers.Clear();

        await base.StopAsync(cancellationToken);
    }

    #endregion

    #region Private methods

    private void ValidateConfigurationAsync()
    {
        var enabledMachines = configuration.Value.Machines.Where(m => m.Enabled).ToList();

        if (!enabledMachines.Any())
        {
            Log.Warning("No enabled machines found in configuration");
            return;
        }

        foreach (MachinePredictionConfig? machineConfig in enabledMachines)
        {
            try
            {
                // Check if ONNX model file exists
                if (!File.Exists(machineConfig.ModelPath))
                {
                    Log.Error(
                        "ONNX model file not found for machine {MachineName}: {ModelPath}",
                        machineConfig.MachineName,
                        machineConfig.ModelPath);

                    throw new FileNotFoundException($"ONNX model file not found: {machineConfig.ModelPath}");
                }

                // Validate configuration
                if (string.IsNullOrEmpty(machineConfig.MachineName))
                {
                    Log.Error(
                        "Machine name is not configured for machine at index {Index}",
                        enabledMachines.IndexOf(machineConfig));

                    throw new InvalidOperationException("Machine name is not configured");
                }

                if (machineConfig.InputSensors.Count == 0)
                {
                    Log.Error(
                        "No input sensors configured for machine {MachineName}",
                        machineConfig.MachineName);

                    throw new InvalidOperationException($"No input sensors configured for machine {machineConfig.MachineName}");
                }

                if (string.IsNullOrEmpty(machineConfig.PredictionSensorName))
                {
                    Log.Error(
                        "Prediction sensor name is not configured for machine {MachineName}",
                        machineConfig.MachineName);
                    throw new InvalidOperationException($"Prediction sensor name is not configured for machine {machineConfig.MachineName}");
                }

                if (string.IsNullOrEmpty(machineConfig.PreprocessingStrategy))
                {
                    Log.Error(
                        "Preprocessing strategy is not configured for machine {MachineName}",
                        machineConfig.MachineName);

                    throw new InvalidOperationException($"Preprocessing strategy is not configured for machine {machineConfig.MachineName}");
                }

                Log.Information("Configuration validated for machine {MachineName}", machineConfig.MachineName);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Configuration validation failed for machine {MachineName}", machineConfig.MachineName);
                throw;
            }
        }
    }

    private void ScheduleMachine(MachinePredictionConfig machineConfig)
    {
        try
        {
            var interval = TimeSpan.FromSeconds(machineConfig.CycleIntervalSeconds);

            var timer = new Timer(async _ => await ProcessMachineAsync(machineConfig), null, TimeSpan.Zero, interval);

            _machineTimers[machineConfig.MachineName] = timer;
            _machineErrors[machineConfig.MachineName] = false;

            Log.Information(
                "Scheduled prediction processing for machine {MachineName} with interval {Interval}",
                machineConfig.MachineName,
                interval);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Failed to schedule machine {MachineName}", machineConfig.MachineName);
            throw;
        }
    }

    private async Task ProcessMachineAsync(MachinePredictionConfig machineConfig)
    {
        // Skip if machine has errors
        if (_machineErrors.TryGetValue(machineConfig.MachineName, out bool hasError) && hasError)
        {
            Log.Debug("Skipping prediction for machine {MachineName} due to previous errors", machineConfig.MachineName);
            return;
        }

        try
        {
            await predictionProcessor.ProcessAsync(machineConfig);

            // Clear error flag if processing succeeds
            if (_machineErrors.ContainsKey(machineConfig.MachineName))
            {
                _machineErrors[machineConfig.MachineName] = false;
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error processing prediction for machine {MachineName}", machineConfig.MachineName);

            // Set error flag to stop processing for this machine
            _machineErrors[machineConfig.MachineName] = true;
        }
    }

    #endregion
}
