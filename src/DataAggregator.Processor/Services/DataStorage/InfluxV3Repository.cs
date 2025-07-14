using DataAggregator.Collector.Shared.Models;
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
public class InfluxV3Repository : IInfluxV3Repository, IDisposable
{
    private readonly string _database = "Dataggregator";
    private InfluxDBClient? _client;

    /// <inheritdoc/>
    public void InitializeAsync(string endpoint, string token, string org)
    {
        _client?.Dispose();

        try
        {
            var clientConfig = new ClientConfig()
            {
                Token = token,
                Host = endpoint,
                Organization = org,
                Database = _database,
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
    public async Task<List<IMeasurementData>> QueryMeasurementsAsync(string table, DateTime startTime, DateTime endTime, List<SensorInfoDto> sensors)
    {
        if (_client == null)
        {
            throw new InvalidOperationException("InfluxDB client is not initialized.");
        }

        try
        {
            // Build the Flux query - no pivot needed since we want the original structure
            string sensorFilter = string.Join(" or ", sensors.Select(s => $"r[\"_field\"] == \"{s.SensorName}\""));
            string query = $@"
                from(bucket: ""{_database}"")
                |> range(start: {startTime:yyyy-MM-ddTHH:mm:ssZ}, stop: {endTime:yyyy-MM-ddTHH:mm:ssZ})
                |> filter(fn: (r) => r[""_measurement""] == ""{table}"")
                |> filter(fn: (r) => {sensorFilter})";

            var measurements = new List<IMeasurementData>();
            var sensorDict = sensors.ToDictionary(s => s.SensorName, s => s);

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
                DateTime timestamp = DateTimeOffset.FromUnixTimeMilliseconds((long)(timestampBigInt.Value / 1_000_000)).DateTime;

                string[] fieldsNames = point.GetFieldNames();
                foreach (string fieldName in fieldsNames)
                {
                    string sensorName = fieldName;
                    object? value = point.GetField(fieldName);

                    // Only include sensors that were requested
                    if (sensorDict.TryGetValue(sensorName, out SensorInfoDto? sensorInfo) && value != null)
                    {
                        IMeasurementData? measurement = sensorInfo.DataType switch
                        {
                            SensorDataType.Boolean when bool.TryParse(value.ToString(), out bool boolValue) =>
                                new MeasurementData<bool>(timestamp, sensorName, boolValue),

                            SensorDataType.Integer when int.TryParse(value.ToString(), out int intValue) =>
                                new MeasurementData<int>(timestamp, sensorName, intValue),

                            SensorDataType.Double or SensorDataType.Float when double.TryParse(value.ToString(), out double doubleValue) =>
                                new MeasurementData<double>(timestamp, sensorName, doubleValue),

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
            }

            Log.Debug(
                "Queried {Count} measurements for table {Table} from {StartTime} to {EndTime}",
                measurements.Count,
                table,
                startTime,
                endTime);

            return measurements;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Failed to query measurements from InfluxDB for table {Table}", table);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task WriteMeasurementAsync(string table, IMeasurementData measurement)
    {
        if (_client == null)
        {
            throw new InvalidOperationException("InfluxDB client is not initialized.");
        }

        try
        {
            PointData point = PointData
                .Measurement(table)
                .SetTimestamp(DateTime.SpecifyKind(measurement.TimeStamp, DateTimeKind.Utc))
                .SetField(measurement.SensorName, measurement.GetRawValue())
                .SetTag("type", "Prediction");

            await _client.WritePointsAsync(new[] { point }, null, WritePrecision.Ms);

            Log.Debug("Written measurement for table {Table}, sensor {Sensor}", table, measurement.SensorName);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Failed to write measurement to InfluxDB for table {Table}", table);
            throw;
        }
    }

    /// <summary>
    /// Disposes the InfluxDB client.
    /// </summary>
    public void Dispose()
        => _client?.Dispose();
}
