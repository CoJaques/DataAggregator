using DataAggregator.Processor.Configuration;
using DataAggregator.Processor.Services.Prediction;
using Serilog;

namespace DataAggregator.Processor.Services;

/// <summary>
/// Background service for processing machine predictions on a scheduled basis.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="PredictionBackgroundService"/> class.
/// </remarks>
/// <param name="configuration">The prediction service configuration.</param>
/// <param name="serviceProvider">The service provider.</param>
public class PredictionBackgroundService(
    PredictionServiceConfiguration configuration,
    IServiceProvider serviceProvider) : BackgroundService
{
    #region Private fields

    private readonly Dictionary<string, Timer> _machineTimers = [];
    private readonly Dictionary<string, SemaphoreSlim> _machineLocks = [];
    private readonly Dictionary<string, IServiceScope> _machineScopes = [];
    private readonly Dictionary<string, IMachinePredictionProcessor> _machineProcessors = [];

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
            foreach (MachinePredictionConfig machineConfig in configuration.Machines)
            {
                if (machineConfig.Enabled)
                {
                    ScheduleMachine(machineConfig);
                }
            }

            Log.Information(
                "Prediction background service started with {MachineCount} machines",
                configuration.Machines.Count(m => m.Enabled));

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

        // Dispose per-machine scopes (which own the scoped services, including processors and repositories)
        foreach ((string _, IServiceScope scope) in _machineScopes)
        {
            try
            {
                scope.Dispose();
            }
            catch (Exception ex)
            {
                Log.Warning(ex, "Error disposing scope for a machine");
            }
        }

        _machineScopes.Clear();
        _machineProcessors.Clear();

        await base.StopAsync(cancellationToken);
    }

    #endregion

    #region Private methods

    private void ValidateConfigurationAsync()
    {
        var enabledMachines = configuration.Machines.Where(m => m.Enabled).ToList();

        if (enabledMachines.Count == 0)
        {
            Log.Warning("No enabled machines found in configuration");
            return;
        }

        foreach (MachinePredictionConfig? machineConfig in enabledMachines)
        {
            try
            {
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

            if (!_machineLocks.ContainsKey(machineConfig.MachineName))
            {
                _machineLocks[machineConfig.MachineName] = new SemaphoreSlim(1, 1);
            }

            // Create and retain a dedicated DI scope per machine to keep stateful services alive across ticks
            IServiceScope scope = serviceProvider.CreateScope();
            IMachinePredictionProcessor predictionProcessor =
                scope.ServiceProvider.GetRequiredService<IMachinePredictionProcessor>();

            _machineScopes[machineConfig.MachineName] = scope;
            _machineProcessors[machineConfig.MachineName] = predictionProcessor;

            var timer = new Timer(
                async _ =>
            {
                // Retrieve the dedicated processor for this machine
                if (_machineProcessors.TryGetValue(machineConfig.MachineName, out IMachinePredictionProcessor? proc))
                {
                    await ProcessMachineAsync(machineConfig, proc);
                }
            },
                null,
                TimeSpan.Zero,
                interval);

            _machineTimers[machineConfig.MachineName] = timer;

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

    private async Task ProcessMachineAsync(MachinePredictionConfig machineConfig, IMachinePredictionProcessor predictionProcessor)
    {
        SemaphoreSlim machineLock = _machineLocks[machineConfig.MachineName];

        if (!await machineLock.WaitAsync(0))
        {
            Log.Warning("Prediction already running for machine {MachineName}, skipping this cycle", machineConfig.MachineName);
            return;
        }

        try
        {
            await predictionProcessor.ProcessAsync(machineConfig);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error processing prediction for machine {MachineName}", machineConfig.MachineName);
        }
        finally
        {
            machineLock.Release();
        }
    }

    #endregion
}
