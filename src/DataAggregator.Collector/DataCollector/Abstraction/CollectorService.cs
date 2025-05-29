using DataAggregator.Collector.DataCollector.Abstraction.Configuration;
using DataAggregator.Collector.DataCollector.DataStorage;
using DataAggregator.Collector.DataCollector.Models;
using DataAggregator.Collector.DataCollector.Registration;
using Serilog;

namespace DataAggregator.Collector.DataCollector.Abstraction;

/// <summary>
/// Abstract base class for collector services.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="CollectorService"/> class.
/// </remarks>
/// <param name="dataSourceConnector">The data source connector.</param>
/// <param name="dataRepository">The data repository.</param>
/// <param name="registrationService">The registration service.</param>
/// <param name="configuration">The collector configuration.</param>
public abstract class CollectorService(
    IDataSourceConnector dataSourceConnector,
    IDataRepository dataRepository,
    RegistrationService registrationService,
    CollectorConfiguration configuration)
{
    // TODO CJS -> Implement the abstract class methods and properties as needed

    /// <summary>
    /// Starts the collector service asynchronously.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task StartAsync() => Log.Information("Starting collector service for device {DeviceName}", configuration.DeviceName);

    /// <summary>
    /// Stops the collector service asynchronously.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task StopAsync() => Log.Information("Stopping collector service for device {DeviceName}", configuration.DeviceName);

    /// <summary>
    /// Processes the collected data asynchronously.
    /// </summary>
    /// <param name="data">The data to process.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    protected async Task ProcessDataAsync(IEnumerable<MeasurementData> data)
    {
        // Method implementation will be added later
    }
}
