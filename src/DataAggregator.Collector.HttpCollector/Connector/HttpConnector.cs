using System.Threading.Channels;
using DataAggregator.Collector.HttpCollector.Configuration;
using DataAggregator.Collector.Shared.Abstraction;
using DataAggregator.Collector.Shared.Models;
using Serilog;

namespace DataAggregator.Collector.HttpCollector.Connector;

/// <summary>
/// Connector for HTTP Push data collection.
/// Acts as a thread-safe bridge between HTTP requests and the internal collection loop.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="HttpConnector"/> class.
/// </remarks>
/// <param name="config">The HTTP collector configuration.</param>
public class HttpConnector(HttpCollectorConfiguration config) : IDataSourceConnector
{
    #region Private Fields
    private readonly HttpCollectorConfiguration _config = config;
    private Channel<IMeasurementData>? _channel;
    private bool _isConnected;
    #endregion

    #region Public Methods

    /// <inheritdoc/>
    public Task ConnectAsync()
    {
        Log.Information("HTTP Connector for device {DeviceName} is now OPEN and accepting data.", _config.DeviceName);
        
        // Create a channel with a bounded capacity if needed, or unbounded
        _channel = Channel.CreateUnbounded<IMeasurementData>(new UnboundedChannelOptions
        {
            SingleReader = true, // Only CollectorService.FetchDataAsync reads
            SingleWriter = false // Multiple HTTP requests can write
        });
        
        _isConnected = true;
        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public Task DisconnectAsync()
    {
        Log.Information("HTTP Connector for device {DeviceName} is now CLOSED.", _config.DeviceName);
        _isConnected = false;
        _channel?.Writer.TryComplete();
        _channel = null;
        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<IMeasurementData>> FetchDataAsync()
    {
        if (!_isConnected || _channel == null)
        {
            return Enumerable.Empty<IMeasurementData>();
        }

        var data = new List<IMeasurementData>();
        
        // Drain the channel non-blockingly
        while (_channel.Reader.TryRead(out IMeasurementData? item))
        {
            data.Add(item);
        }

        if (data.Count > 0)
        {
            Log.Debug("Fetched {Count} measurements from HTTP Channel for {DeviceName}.", data.Count, _config.DeviceName);
        }

        return await Task.FromResult(data);
    }

    /// <inheritdoc/>
    public Task<bool> IsConnectedAsync() => Task.FromResult(_isConnected);

    /// <summary>
    /// Attempts to enqueue data from an HTTP request.
    /// </summary>
    /// <param name="measurements">The measurements to enqueue.</param>
    /// <returns>True if the data was accepted, false if the connector is disconnected.</returns>
    public bool TryEnqueueData(IEnumerable<IMeasurementData> measurements)
    {
        if (!_isConnected || _channel == null)
        {
            return false;
        }

        int count = 0;
        foreach (IMeasurementData measurement in measurements)
        {
            if (_channel.Writer.TryWrite(measurement))
            {
                count++;
            }
        }

        if (count > 0)
        {
            Log.Debug("Enqueued {Count} measurements via HTTP for {DeviceName}.", count, _config.DeviceName);
        }

        return true;
    }

    #endregion
}
