using DataAggregator.Collector.Shared.Models;
using DataAggregator.Shared.DTOs;

namespace DataAggregator.Processor.Services.DataStorage;

/// <summary>
/// Interface for InfluxDB v3 repository operations.
/// </summary>
public interface IInfluxV3Repository
{
    /// <summary>
    /// Initializes the repository with connection parameters.
    /// </summary>
    /// <param name="endpoint">The InfluxDB endpoint.</param>
    /// <param name="token">The authentication token.</param>
    /// <param name="org">The organization name.</param>
    public void InitializeAsync(string endpoint, string token, string org);

    /// <summary>
    /// Queries measurements from InfluxDB for a specific time range and sensors with type information.
    /// </summary>
    /// <param name="table">The table name (machine name).</param>
    /// <param name="startTime">The start time for the query.</param>
    /// <param name="endTime">The end time for the query.</param>
    /// <param name="sensors">The list of sensor information with type data.</param>
    /// <returns>A list of measurement data.</returns>
    public Task<List<IMeasurementData>> QueryMeasurementsAsync(string table, DateTime startTime, DateTime endTime, List<SensorInfoDto> sensors);

    /// <summary>
    /// Writes a single measurement to InfluxDB.
    /// </summary>
    /// <param name="table">The table name (machine name).</param>
    /// <param name="measurement">The measurement data to write.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public Task WriteMeasurementAsync(string table, IMeasurementData measurement);
}
