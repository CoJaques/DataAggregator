using DataAggregator.Collector.DataCollector.Abstraction;
using DataAggregator.Collector.DataCollector.Connectors.OpenCN;
using DataAggregator.Collector.DataCollector.Models;
using Serilog;

namespace DataAggregator.Collector.DataCollector.Connectors.CapnProto;

/// <summary>
/// Connector for Cap'n Proto data source.
/// </summary>
public class CapnProtoConnector : IDataSourceConnector
{
    private readonly string _serverAddress;
    private readonly int _port;
    private readonly int _timeoutMs;
    private readonly OpenCnCollectorConfiguration? _openCnConfig;

    private bool _isConnected;
    private readonly Random _random = new();
    private readonly Dictionary<string, SensorDataType> _sensorTypeMap = [];

    /// <summary>
    /// Initializes a new instance of the <see cref="CapnProtoConnector"/> class.
    /// </summary>
    /// <param name="config">The Cap'n Proto configuration.</param>
    public CapnProtoConnector(CapnProtoConfig config)
    {
        _serverAddress = config.ServerAddress;
        _port = config.Port;
        _timeoutMs = config.TimeoutMs;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CapnProtoConnector"/> class with OpenCN configuration.
    /// </summary>
    /// <param name="config">The Cap'n Proto configuration.</param>
    /// <param name="openCnConfig">The OpenCN configuration for sensor mapping.</param>
    public CapnProtoConnector(CapnProtoConfig config, OpenCnCollectorConfiguration openCnConfig)
        : this(config)
    {
        _openCnConfig = openCnConfig;

        // Build sensor type map from OpenCN configuration
        if (_openCnConfig?.Sensors != null)
        {
            foreach (OpenCnSensorConfig sensor in _openCnConfig.Sensors)
            {
                if (sensor is OpenCnSensorConfig openCnSensor)
                {
                    _sensorTypeMap[openCnSensor.Name] = openCnSensor.DataType;
                }
                else
                {
                    _sensorTypeMap[sensor.Name] = sensor.DataType;
                }
            }
        }
    }

    /// <inheritdoc/>
    public async Task ConnectAsync(string endpoint)
    {
        if (_isConnected)
        {
            Log.Debug("Already connected to Cap'n Proto server, disconnecting first");
            await DisconnectAsync();
        }

        Log.Information("Connecting to Cap'n Proto server at {ServerAddress}:{Port}", _serverAddress, _port);

        try
        {
            // TODO: Implement actual Cap'n Proto client connection
            // This is a placeholder implementation that simulates connection initialization

            // Simulate connection delay
            await Task.Delay(_random.Next(50, 200));

            _isConnected = true;
            Log.Information("Connected to Cap'n Proto server");
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Failed to connect to Cap'n Proto server at {ServerAddress}:{Port}", _serverAddress, _port);
            _isConnected = false;
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<IMeasurementData>> FetchDataAsync()
    {
        if (!_isConnected)
        {
            Log.Warning("Attempted to fetch data while not connected");
            return Enumerable.Empty<IMeasurementData>();
        }

        Log.Debug("Fetching data from Cap'n Proto server at {ServerAddress}:{Port}", _serverAddress, _port);

        try
        {
            // TODO: Implement actual Cap'n Proto data fetching
            // This is a placeholder implementation that generates random data for testing

            // Simulate network delay
            await Task.Delay(_random.Next(10, 50));

            // Generate sample data
            return GenerateSampleData();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error fetching data from Cap'n Proto server");
            return Enumerable.Empty<IMeasurementData>();
        }
    }

    /// <inheritdoc/>
    public Task<bool> IsConnectedAsync() => Task.FromResult(_isConnected);

    /// <inheritdoc/>
    public async Task DisconnectAsync()
    {
        if (!_isConnected)
        {
            return;
        }

        Log.Information("Disconnecting from Cap'n Proto server");

        try
        {
            // TODO: Implement actual Cap'n Proto client disconnection
            // This is a placeholder implementation that simulates disconnection

            // Simulate disconnection delay
            await Task.Delay(_random.Next(10, 50));

            _isConnected = false;
            Log.Information("Disconnected from Cap'n Proto server");
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error disconnecting from Cap'n Proto server");
            throw;
        }
    }

    private IEnumerable<IMeasurementData> GenerateSampleData()
    {
        DateTime timestamp = DateTime.UtcNow;
        var result = new List<IMeasurementData>();

        // If we have OpenCN configuration, use it to generate data based on the configured sensors
        if (_openCnConfig?.Sensors != null && _openCnConfig.Sensors.Any())
        {
            foreach (OpenCnSensorConfig sensor in _openCnConfig.Sensors)
            {
                // Generate value based on sensor's data type
                switch (sensor.DataType)
                {
                    case SensorDataType.Boolean:
                        result.Add(new MeasurementData<bool>(timestamp, sensor.Name, _random.Next(10) > 1));
                        break;
                    case SensorDataType.Integer:
                        result.Add(new MeasurementData<int>(timestamp, sensor.Name, _random.Next(0, 100)));
                        break;
                    case SensorDataType.Double:
                        result.Add(new MeasurementData<double>(timestamp, sensor.Name, _random.NextDouble() * 100));
                        break;
                    case SensorDataType.String:
                        result.Add(new MeasurementData<string>(timestamp, sensor.Name, $"Value-{_random.Next(1000)}"));
                        break;
                    default:
                        // Skip unknown types
                        break;
                }
            }
        }
        else
        {
            // If no sensors are configured, generate some default test data
            result.Add(new MeasurementData<double>(timestamp, "Temperature", _random.NextDouble() * 50));
            result.Add(new MeasurementData<double>(timestamp, "Pressure", _random.NextDouble() * 10));
            result.Add(new MeasurementData<bool>(timestamp, "PowerStatus", _random.Next(10) > 1));
            result.Add(new MeasurementData<int>(timestamp, "CycleCounter", _random.Next(1000)));
        }

        return result;
    }
}
