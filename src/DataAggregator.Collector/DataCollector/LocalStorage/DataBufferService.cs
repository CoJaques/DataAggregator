using DataAggregator.Collector.DataCollector.Models;
using Serilog;

namespace DataAggregator.Collector.DataCollector.LocalStorage;

/// <summary>
/// Service for buffering measurement data when the data repository is unavailable.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="DataBufferService"/> class.
/// </remarks>
/// <param name="maxBufferSize">The maximum buffer size.</param>
public class DataBufferService(int maxBufferSize = 10000)
{
    private readonly Queue<IMeasurementData> _buffer = new();
    private readonly Lock _bufferLock = new();

    /// <summary>
    /// Adds a measurement to the buffer.
    /// </summary>
    /// <param name="data">The measurement data to add.</param>
    public void Add(IMeasurementData data)
    {
        lock (_bufferLock)
        {
            if (_buffer.Count >= maxBufferSize)
            {
                Log.Warning("Buffer is full. Dropping oldest measurement.");
                _buffer.Dequeue(); // Remove the oldest measurement
            }

            _buffer.Enqueue(data);
        }
    }

    /// <summary>
    /// Adds multiple measurements to the buffer.
    /// </summary>
    /// <param name="data">The collection of measurement data to add.</param>
    public void AddRange(IEnumerable<IMeasurementData> data)
    {
        lock (_bufferLock)
        {
            foreach (IMeasurementData item in data)
            {
                Add(item);
            }
        }
    }

    /// <summary>
    /// Gets and removes all measurements from the buffer.
    /// </summary>
    /// <returns>The collection of measurement data from the buffer.</returns>
    public IEnumerable<IMeasurementData> GetAndClearBuffer()
    {
        lock (_bufferLock)
        {
            IMeasurementData[] result = [.. _buffer];
            _buffer.Clear();
            return result;
        }
    }

    /// <summary>
    /// Gets the current size of the buffer.
    /// </summary>
    /// <returns>The number of measurements in the buffer.</returns>
    public int GetBufferSize()
    {
        lock (_bufferLock)
        {
            return _buffer.Count;
        }
    }
}
