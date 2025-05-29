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
    private readonly Queue<MeasurementData> _buffer = new();

    /// <summary>
    /// Adds a measurement to the buffer.
    /// </summary>
    /// <param name="data">The measurement data to add.</param>
    /// <returns>True if the data was successfully added; otherwise, false.</returns>
    public bool Add(MeasurementData data)
    {
        if (_buffer.Count >= maxBufferSize)
        {
            Log.Warning("Buffer is full. Dropping oldest measurement.");
            _buffer.Dequeue(); // Remove the oldest measurement
        }

        _buffer.Enqueue(data);
        return true;
    }

    /// <summary>
    /// Adds multiple measurements to the buffer.
    /// </summary>
    /// <param name="data">The collection of measurement data to add.</param>
    /// <returns>The number of measurements successfully added.</returns>
    public int AddRange(IEnumerable<MeasurementData> data)
    {
        int added = 0;

        foreach (MeasurementData item in data)
        {
            if (Add(item))
            {
                added++;
            }
        }

        return added;
    }

    /// <summary>
    /// Gets and removes all measurements from the buffer.
    /// </summary>
    /// <returns>The collection of measurement data from the buffer.</returns>
    public IEnumerable<MeasurementData> GetAndClearBuffer()
    {
        var result = _buffer.ToList();
        _buffer.Clear();
        return result;
    }

    /// <summary>
    /// Gets the current size of the buffer.
    /// </summary>
    /// <returns>The number of measurements in the buffer.</returns>
    public int GetBufferSize()
        => _buffer.Count;
}
