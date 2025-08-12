using System.Globalization;
using DataAggregator.Collector.FileCollector.Configuration;
using DataAggregator.Collector.Shared.Abstraction;
using DataAggregator.Collector.Shared.Abstraction.Configuration;
using DataAggregator.Collector.Shared.Models;
using DataAggregator.Shared.Domain.DataType;

namespace DataAggregator.Collector.FileCollector.Connector;

/// <summary>
/// Define a connector for file-based data sources, used to read data from files.
/// Useful for testing or when data is stored in files.
/// </summary>
public class FileConnector(FileConnectorConfiguration config) : IDataSourceConnector
{
    #region Private Fields
    private List<StreamReader> _readers = [];
    private List<string> _sensorNames = [];
    private int _currentFileIndex = 0;
    private DateTime _lastFetchTime = DateTime.MinValue;
    private bool _initialized = false;
    #endregion

    /// <summary>
    /// Initializes the connector by opening the files and reading the sensor headers.
    /// Files are read line by line on demand, without loading the entire content into memory.
    /// </summary>
    /// <returns>Task.CompletedTask.</returns>
    public Task ConnectAsync()
    {
        DisposeReaders();
        _readers = [];
        _sensorNames = [];
        _currentFileIndex = 0;
        _lastFetchTime = DateTime.MinValue;
        _initialized = false;

        foreach (string file in config.Files)
        {
            string executablePath = AppContext.BaseDirectory;
            string filePath = Path.Combine(executablePath, file);
            if (!File.Exists(filePath))
                continue;

            var reader = new StreamReader(filePath);
            string? header = reader.ReadLine();
            if (header == null)
            {
                reader.Dispose();
                continue;
            }

            if (_sensorNames.Count == 0)
                _sensorNames.AddRange(header.Split(','));

            _readers.Add(reader);
        }

        _initialized = _readers.Count > 0 && _sensorNames.Count > 0;
        return Task.CompletedTask;
    }

    /// <summary>
    /// Closes and disposes all open files, and resets the connector state.
    /// </summary>
    /// <returns>Task.CompletedTask.</returns>
    public Task DisconnectAsync()
    {
        DisposeReaders();
        _readers = [];
        _sensorNames = [];
        _currentFileIndex = 0;
        _lastFetchTime = DateTime.MinValue;
        _initialized = false;

        return Task.CompletedTask;
    }

    /// <summary>
    /// Reads data from the files line by line, respecting the configured sampling rate (SamplingRate).
    /// Returns a batch of N lines, where N depends on the elapsed time since the last call and the frequency.
    /// Timestamps are retroactive: the last data point is timestamped at "now",
    /// previous ones are spaced according to the frequency.
    /// Files are read on demand and automatically loop when reaching the end.
    /// </summary>
    /// <returns>A batch of sensor data, with correct timestamps.</returns>
    public Task<IEnumerable<IMeasurementData>> FetchDataAsync()
    {
        if (!_initialized)
            return Task.FromResult(Enumerable.Empty<IMeasurementData>());

        double samplingRate = config.SamplingRate > 0 ? config.SamplingRate : 1.0;
        double intervalMs = 1000.0 / samplingRate;

        DateTime now = DateTime.UtcNow;
        if (_lastFetchTime == DateTime.MinValue)
            _lastFetchTime = now;

        double elapsedMs = (now - _lastFetchTime).TotalMilliseconds;
        int linesToProvide = (int)(elapsedMs / intervalMs);

        if (linesToProvide <= 0)
            return Task.FromResult<IEnumerable<IMeasurementData>>([]);

        List<IMeasurementData> result = [];

        // Calculate timestamps for each line: oldest = now - (linesToProvide-1)*interval, newest = now
        for (int i = 0; i < linesToProvide; i++)
        {
            string? line = null;
            int attempts = 0;
            while (attempts < _readers.Count)
            {
                if (_readers[_currentFileIndex].EndOfStream)
                {
                    _readers[_currentFileIndex].DiscardBufferedData();
                    _readers[_currentFileIndex].BaseStream.Seek(0, SeekOrigin.Begin);
                    _readers[_currentFileIndex].ReadLine(); // skip header
                }

                line = _readers[_currentFileIndex].ReadLine();
                _currentFileIndex = (_currentFileIndex + 1) % _readers.Count;

                if (!string.IsNullOrWhiteSpace(line))
                    break;

                attempts++;
            }

            if (string.IsNullOrWhiteSpace(line))
                continue;

            string[] row = line.Split(',');

            // Retro-timestamping: oldest = now - (linesToProvide-1)*interval, newest = now
            DateTime timestamp = now - TimeSpan.FromMilliseconds(intervalMs * (linesToProvide - 1 - i));
            foreach (SensorConfig sensor in config.Sensors)
            {
                int colIdx = _sensorNames.FindIndex(n => n == sensor.Name);

                if (colIdx == -1 || colIdx >= row.Length)
                    continue;

                string valueStr = row[colIdx];

                IMeasurementData? measurement = sensor.DataType switch
                {
                    SensorDataType.Boolean => new MeasurementData<bool>(timestamp, sensor.Name, float.Parse(valueStr, CultureInfo.InvariantCulture) != 0),
                    SensorDataType.Integer => new MeasurementData<int>(timestamp, sensor.Name, (int)float.Parse(valueStr, CultureInfo.InvariantCulture)),
                    SensorDataType.Double or SensorDataType.Float => new MeasurementData<double>(timestamp, sensor.Name, double.Parse(valueStr, CultureInfo.InvariantCulture)),
                    SensorDataType.String => new MeasurementData<string>(timestamp, sensor.Name, valueStr),
                    _ => null,
                };

                if (measurement != null)
                    result.Add(measurement);
            }
        }

        _lastFetchTime = _lastFetchTime.AddMilliseconds(linesToProvide * intervalMs);
        return Task.FromResult<IEnumerable<IMeasurementData>>(result);
    }

    /// <summary>
    /// Checks that all files specified in the configuration exist and are accessible.
    /// </summary>
    /// <returns>True if all files exist, false otherwise.</returns>
    public Task<bool> IsConnectedAsync()
    {
        bool isConnected = true;
        foreach (string filePath in config.Files)
        {
            if (!File.Exists(filePath))
            {
                isConnected = false;
                break;
            }
        }

        return Task.FromResult(isConnected);
    }

    private void DisposeReaders()
    {
        foreach (StreamReader reader in _readers)
        {
            reader.Dispose();
        }
    }
}
