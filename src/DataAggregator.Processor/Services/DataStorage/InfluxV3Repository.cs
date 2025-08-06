using DataAggregator.Collector.Shared.Models;
using DataAggregator.Shared.Configuration.TimeSeries;
using DataAggregator.Shared.Domain.DataType;
using DataAggregator.Shared.DTOs;
using InfluxDB3.Client;
using InfluxDB3.Client.Config;
using InfluxDB3.Client.Write;
using Serilog;

namespace DataAggregator.Processor.Services.DataStorage;

/// <summary>
/// Implementation of InfluxDB v3 repository for prediction service.
/// </summary>
public class InfluxV3Repository : IDataRepository, IDisposable
{
    private InfluxDBClient? _client;

    /// <inheritdoc/>
    public void Initialize(string endpoint, string token)
    {
        _client?.Dispose();

        try
        {
            var clientConfig = new ClientConfig()
            {
                Token = token,
                Host = endpoint,
                Organization = InfluxHelper.DatabaseName,
                Database = InfluxHelper.DatabaseName,
            };

            _client = new InfluxDBClient(clientConfig);

            Log.Information("InfluxDB v3 repository initialized with endpoint: {Endpoint}", endpoint);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Failed to initialize InfluxDB v3 repository");
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<List<IMeasurementData>> QueryMeasurementsAsync(
        string table,
        DateTime startTime,
        DateTime endTime,
        List<SensorInfoDto> sensors)
    {
        if (_client == null)
        {
            throw new InvalidOperationException("InfluxDB client is not initialized.");
        }

        string sensorColumns = string.Join(", ", sensors.Select(s => $"\"{s.SensorName}\""));

        string query = $"""
        SELECT time, {sensorColumns}
        FROM "{table}"
        WHERE time >= '{startTime:yyyy-MM-ddTHH:mm:ssZ}'
          AND time < '{endTime:yyyy-MM-ddTHH:mm:ssZ}'
        """;

        return await ExecuteMeasurementQuery(query, sensors, table, startTime, endTime);
    }

    /// <inheritdoc/>
    public async Task<List<IMeasurementData>> QueryLastMeasurements(
        string table,
        int windowSize,
        List<SensorInfoDto> sensors)
    {
        if (_client == null)
        {
            throw new InvalidOperationException("InfluxDB client is not initialized.");
        }

        string sensorColumns = string.Join(", ", sensors.Select(s => $"\"{s.SensorName}\""));

        string query = $"""
        SELECT time, {sensorColumns}
        FROM "{table}"
        ORDER BY time DESC
        LIMIT {windowSize}
        """;

        // Note: sorting back to ascending to preserve chronological order
        List<IMeasurementData> reversed = await ExecuteMeasurementQuery(query, sensors, table);
        return reversed.OrderBy(m => m.TimeStamp).ToList();
    }

    /// <inheritdoc/>
    public async Task WriteMeasurementAsync(string tag, IEnumerable<IMeasurementData> measurements)
    {
        if (_client == null)
        {
            throw new InvalidOperationException("InfluxDB client is not initialized.");
        }

        try
        {
            IEnumerable<PointData> groupedPoints = measurements
                .GroupBy(m => m.TimeStamp)
                .Select(group =>
                {
                    var fields = group.ToDictionary(
                        m => m.SensorName,
                        m => m.GetRawValue());

                    return PointData
                        .Measurement(InfluxHelper.PredictionTableName)
                        .SetTimestamp(DateTime.SpecifyKind(group.Key, DateTimeKind.Utc))
                        .SetFields(fields)
                        .SetTag(InfluxHelper.MachineNameTag, tag);
                });

            await _client.WritePointsAsync(groupedPoints, null, WritePrecision.Ms);

            Log.Information($"Inserted {groupedPoints.Count()} element");
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Failed to write measurement to InfluxDB for table {Table}", tag);
            throw;
        }
    }

    /// <summary>
    /// Execute a generic InfluxDB query and convert results to a flat list of IMeasurementData.
    /// Assumes the data is stored in a "wide" format (one row per timestamp, multiple sensors as columns).
    /// </summary>
    private async Task<List<IMeasurementData>> ExecuteMeasurementQuery(
        string query,
        List<SensorInfoDto> sensors,
        string table,
        DateTime? start = null,
        DateTime? end = null)
    {
        if (_client is null)
        {
            return [];
        }

        var measurements = new List<IMeasurementData>();
        var sensorDict = sensors.ToDictionary(s => s.SensorName, s => s);

        try
        {
            await foreach (PointDataValues point in _client.QueryPoints(query))
            {
                // point contains the original structure with all fields at once
                // This matches how we write data: one row per timestamp with multiple sensors
                System.Numerics.BigInteger? timestampBigInt = point.GetTimestamp();
                if (timestampBigInt == null)
                {
                    Log.Warning("Skipping point with null timestamp");
                    continue;
                }

                // Convert BigInteger timestamp to DateTime
                DateTime timestamp = DateTimeOffset
                    .FromUnixTimeMilliseconds((long)(timestampBigInt.Value / 1_000_000))
                    .DateTime;

                string[] fieldNames = point.GetFieldNames();
                foreach (string fieldName in fieldNames)
                {
                    string sensorName = fieldName;

                    // Only include sensors that were requested
                    if (!sensorDict.TryGetValue(sensorName, out SensorInfoDto? sensorInfo))
                        continue;

                    object? value = point.GetField(sensorName);
                    if (value == null)
                        continue;

                    // Deserialize the value according to the expected data type
                    IMeasurementData? measurement = sensorInfo.DataType switch
                    {
                        SensorDataType.Boolean when bool.TryParse(value.ToString(), out bool b) =>
                            new MeasurementData<bool>(timestamp, sensorName, b),

                        SensorDataType.Integer when int.TryParse(value.ToString(), out int i) =>
                            new MeasurementData<int>(timestamp, sensorName, i),

                        SensorDataType.Double or SensorDataType.Float when double.TryParse(value.ToString(), out double d) =>
                            new MeasurementData<double>(timestamp, sensorName, d),

                        SensorDataType.String =>
                            new MeasurementData<string>(timestamp, sensorName, value.ToString() ?? string.Empty),

                        _ => null,
                    };

                    if (measurement != null)
                    {
                        measurements.Add(measurement);
                    }
                    else
                    {
                        Log.Debug(
                            "Skipping value for sensor {Sensor} with type {DataType}: {Value}",
                            sensorName,
                            sensorInfo.DataType,
                            value);
                    }
                }
            }

            Log.Debug(
                "Fetched {Count} measurements from {Table} {Range}",
                measurements.Count,
                table,
                start != null && end != null ? $"from {start} to {end}" : "(last N)");

            return measurements;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Query failed for table {Table}", table);
            throw;
        }
    }

    /// <summary>
    /// Disposes the InfluxDB client.
    /// </summary>
    public void Dispose()
        => _client?.Dispose();
}
