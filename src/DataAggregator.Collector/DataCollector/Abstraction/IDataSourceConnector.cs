using DataAggregator.Collector.DataCollector.Models;

namespace DataAggregator.Collector.DataCollector.Abstraction;

/// <summary>
/// Interface for connecting to and fetching data from a data source.
/// </summary>
public interface IDataSourceConnector
{
    /// <summary>
    /// Connects to the data source asynchronously.
    /// </summary>
    /// <param name="endpoint">The endpoint to connect to.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public Task ConnectAsync(string endpoint);

    /// <summary>
    /// Fetches data from the data source asynchronously.
    /// </summary>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation. The task result contains a collection of measurement data.</returns>
    public Task<IEnumerable<MeasurementData>> FetchDataAsync();

    /// <summary>
    /// Checks if the connector is connected to the data source asynchronously.
    /// </summary>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation. The task result contains a boolean indicating whether the connector is connected.</returns>
    public Task<bool> IsConnectedAsync();

    /// <summary>
    /// Disconnects from the data source asynchronously.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public Task DisconnectAsync();
}
