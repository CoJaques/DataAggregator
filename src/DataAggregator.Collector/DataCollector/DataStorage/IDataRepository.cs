using DataAggregator.Collector.DataCollector.Models;

namespace DataAggregator.Collector.DataCollector.DataStorage;

/// <summary>
/// Interface for data repository operations.
/// </summary>
public interface IDataRepository
{
    /// <summary>
    /// Inserts multiple measurement data points asynchronously.
    /// </summary>
    /// <param name="data">The collection of measurement data to insert.</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation. The task result contains a boolean indicating whether the operation was successful.</returns>
    public Task<bool> BulkInsertAsync(IEnumerable<MeasurementData> data);
}
