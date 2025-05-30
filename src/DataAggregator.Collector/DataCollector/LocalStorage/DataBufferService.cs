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
    private readonly object _bufferLock = new();

    /// <summary>
    /// Adds a measurement to the buffer.
    /// </summary>
    /// <param name="data">The measurement data to add.</param>
    /// <returns>True if the data was successfully added; otherwise, false.</returns>
    public bool Add(IMeasurementData data)
    {
        lock (_bufferLock)
        {
            if (_buffer.Count >= maxBufferSize)
            {
                Log.Warning("Buffer is full. Dropping oldest measurement.");
                _buffer.Dequeue(); // Remove the oldest measurement
            }

            _buffer.Enqueue(data);
            return true;
        }
    }

    /// <summary>
    /// Adds multiple measurements to the buffer.
    /// </summary>
    /// <typeparam name="T">The type of the measurement value.</typeparam>
    /// <param name="data">The collection of measurement data to add.</param>
    /// <returns>The number of measurements successfully added.</returns>
    public int AddRange<T>(IEnumerable<MeasurementData<T>> data)
    {
        int added = 0;
        
        lock (_bufferLock)
        {
            foreach (IMeasurementData item in data)
            {
                if (Add(item))
                {
                    added++;
                }
            }
        }

        return added;
    }
    
    /// <summary>
    /// Adds multiple measurements to the buffer.
    /// </summary>
    /// <param name="data">The collection of measurement data to add.</param>
    /// <returns>The number of measurements successfully added.</returns>
    public int AddRange(IEnumerable<IMeasurementData> data)
    {
        int added = 0;
        
        lock (_bufferLock)
        {
            foreach (IMeasurementData item in data)
            {
                if (Add(item))
                {
                    added++;
                }
            }
        }

        return added;
    }

    /// <summary>
    /// Gets and removes all measurements from the buffer.
    /// </summary>
    /// <returns>The collection of measurement data from the buffer.</returns>
    public IEnumerable<IMeasurementData> GetAndClearBuffer()
    {
        lock (_bufferLock)
        {
            IMeasurementData[] result = _buffer.ToArray();
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
