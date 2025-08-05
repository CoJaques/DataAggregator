using DataAggregator.Collector.Shared.Models;

namespace DataAggregator.Processor.Services.Processing.Abstraction;

/// <summary>
/// Base interface for data processors that handle measurement data.
/// </summary>
public interface IDataProcessor
{
    /// <summary>
    /// Processes a collection of measurement data asynchronously and return a new processed list of measurements.
    /// </summary>
    /// <param name="input">The input measurements.</param>
    /// <returns>A new IEnumerable of IMeasurementData which is processed.</returns>
    public Task<IEnumerable<IMeasurementData>> ProcessAsync(IEnumerable<IMeasurementData> input);
}
