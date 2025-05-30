using DataAggregator.Collector.DataCollector.Models;

namespace DataAggregator.Collector.DataCollector.DataStorage;

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
    /// <typeparam name="T">The type of the measurement value.</typeparam>
    /// <param name="data">The collection of measurement data to insert.</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation. The task result contains a boolean indicating whether the operation was successful.</returns>
    public Task<bool> BulkInsertAsync<T>(IEnumerable<MeasurementData<T>> data);
}
