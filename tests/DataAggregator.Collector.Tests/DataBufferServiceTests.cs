using DataAggregator.Collector.DataCollector.LocalStorage;
using DataAggregator.Collector.DataCollector.Models;

namespace DataAggregator.Collector.Tests;

public class DataBufferServiceTests
{
    private static IMeasurementData CreateMeasurement(int value)
        => new MeasurementData<int>(DateTime.UtcNow, $"sensor{value}", value);

    [Fact]
    public void Add_ShouldIncreaseBufferSize()
    {
        var buffer = new DataBufferService(10);
        buffer.Add(CreateMeasurement(1));
        Assert.Equal(1, buffer.GetBufferSize());
    }

    [Fact]
    public void AddRange_ShouldAddAllItems()
    {
        var buffer = new DataBufferService(10);
        var data = Enumerable.Range(1, 5).Select(CreateMeasurement).ToList();
        buffer.AddRange(data);
        Assert.Equal(5, buffer.GetBufferSize());
    }

    [Fact]
    public void Add_ShouldDropOldest_WhenBufferIsFull()
    {
        var buffer = new DataBufferService(3);
        buffer.Add(CreateMeasurement(1));
        buffer.Add(CreateMeasurement(2));
        buffer.Add(CreateMeasurement(3));
        buffer.Add(CreateMeasurement(4)); // Doit drop le 1
        var all = buffer.GetAndClearBuffer().ToList();
        Assert.Equal(3, all.Count);
        Assert.DoesNotContain(all, m => ((int)m.GetRawValue()) == 1);
        Assert.Contains(all, m => ((int)m.GetRawValue()) == 4);
    }

    [Fact]
    public void GetAndClearBuffer_ShouldReturnAllAndEmptyBuffer()
    {
        var buffer = new DataBufferService(10);
        buffer.Add(CreateMeasurement(1));
        buffer.Add(CreateMeasurement(2));
        var all = buffer.GetAndClearBuffer().ToList();
        Assert.Equal(2, all.Count);
        Assert.Equal(0, buffer.GetBufferSize());
    }

    [Fact]
    public void GetBufferSize_ShouldReturnCurrentSize()
    {
        var buffer = new DataBufferService(10);
        Assert.Equal(0, buffer.GetBufferSize());
        buffer.Add(CreateMeasurement(1));
        Assert.Equal(1, buffer.GetBufferSize());
    }
}
