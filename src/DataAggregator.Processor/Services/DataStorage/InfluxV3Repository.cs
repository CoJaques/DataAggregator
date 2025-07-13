using DataAggregator.Collector.Shared.Models;
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
    private string _organization = "Dataggregator";

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
            _organization = org;

            Log.Information("InfluxDB v3 repository initialized with endpoint: {Endpoint}", endpoint);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Failed to initialize InfluxDB v3 repository");
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<List<IMeasurementData>> QueryMeasurementsAsync(string table, DateTime startTime, DateTime endTime, List<string> sensors)
    {
        if (_client == null)
        {
            throw new InvalidOperationException("InfluxDB client is not initialized.");
        }

        try
        {
            // Build the Flux query - no pivot needed since we want the original structure
            string sensorFilter = string.Join(" or ", sensors.Select(s => $"r[\"_field\"] == \"{s}\""));
            string query = $@"
                from(bucket: ""{_database}"")
                |> range(start: {startTime:yyyy-MM-ddTHH:mm:ssZ}, stop: {endTime:yyyy-MM-ddTHH:mm:ssZ})
                |> filter(fn: (r) => r[""_measurement""] == ""{table}"")
                |> filter(fn: (r) => {sensorFilter})";

            var measurements = new List<IMeasurementData>();

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
                    if (sensors.Contains(sensorName) && value != null)
                    {
                        // Try to convert to float, handling different numeric types
                        float floatValue;
                        if (value is float f)
                        {
                            floatValue = f;
                        }
                        else if (value is double d)
                        {
                            floatValue = (float)d;
                        }
                        else if (value is int i)
                        {
                            floatValue = i;
                        }
                        else if (value is long l)
                        {
                            floatValue = l;
                        }
                        else if (float.TryParse(value.ToString(), out floatValue))
                        {
                            // Successfully parsed
                        }
                        else
                        {
                            Log.Debug("Skipping non-numeric value for sensor {Sensor}: {Value}", sensorName, value);
                            continue;
                        }

                        measurements.Add(new MeasurementData<float>(timestamp, sensorName, floatValue));
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

    /// <inheritdoc/>
    public async Task BulkWriteMeasurementsAsync(string table, List<IMeasurementData> measurements)
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
                        .Measurement(table)
                        .SetTimestamp(DateTime.SpecifyKind(group.Key, DateTimeKind.Utc))
                        .SetFields(fields)
                        .SetTag("type", "Prediction");
                });

            await _client.WritePointsAsync(groupedPoints, null, WritePrecision.Ms);

            Log.Debug("Written {Count} measurements to InfluxDB for table {Table}", measurements.Count, table);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Failed to bulk write measurements to InfluxDB for table {Table}", table);
            throw;
        }
    }

    /// <summary>
    /// Disposes the InfluxDB client.
    /// </summary>
    public void Dispose()
        => _client?.Dispose();
}
