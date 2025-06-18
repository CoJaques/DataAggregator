using DataAggregator.Collector.Shared.Abstraction.Configuration;
using DataAggregator.Collector.Shared.Models;

namespace DataAggregator.Collector.Shared.DataStorage;

/// <summary>
/// Interface for data repository operations.
/// </summary>
public interface IDataRepository
{
    /// <summary>
    /// Initializes the repository.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public Task InitializeAsync();

    /// <summary>
    /// Inserts multiple measurement data points asynchronously.
    /// </summary>
    /// <param name="data">The collection of measurement data to insert.</param>
    /// <param name="configuration">The configuration of the collector, used to define th scheme of the repo.</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation. The task result contains a boolean indicating whether the operation was successful.</returns>
    public Task<bool> BulkInsertAsync(IEnumerable<IMeasurementData> data, CollectorConfiguration configuration);

    /// <summary>
    /// Method to check if the repository is connected to the data source asynchronously.
    /// </summary>
    /// <returns>True if connected false otherwise.</returns>
    public Task<bool> IsConnectedAsync();
}
