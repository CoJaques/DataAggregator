using DataAggregator.Collector.DataCollector.Abstraction;
using DataAggregator.Collector.DataCollector.Models;
using Serilog;

namespace DataAggregator.Collector.DataCollector.Connectors.CapnProto;

// TODO CJS -> Implement Cap'n Proto-specific methods and properties in this class
/// <summary>
/// Connector for Cap'n Proto data source.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="CapnProtoConnector"/> class.
/// </remarks>
/// <param name="config">The Cap'n Proto configuration.</param>
public class CapnProtoConnector(CapnProtoConfig config) : IDataSourceConnector
{
    private readonly string _serverAddress = config.ServerAddress;
    private readonly int _samplingRate;

    // This would be replaced with actual CapnProto client in the implementation
    private bool _isConnected;

    /// <inheritdoc/>
    public async Task ConnectAsync(string endpoint)
    {
        Log.Information("Connecting to Cap'n Proto server at {ServerAddress}", _serverAddress);

        // Implementation will be added later
        _isConnected = true;
        await Task.CompletedTask;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<MeasurementData>> FetchDataAsync()
    {
        Log.Debug("Fetching data from Cap'n Proto server at sampling rate {SamplingRate}Hz", _samplingRate);

        // Implementation will be added later
        return await Task.FromResult(Enumerable.Empty<MeasurementData>());
    }

    /// <inheritdoc/>
    public Task<bool> IsConnectedAsync() => Task.FromResult(_isConnected);

    /// <inheritdoc/>
    public async Task DisconnectAsync()
    {
        Log.Information("Disconnecting from Cap'n Proto server");

        // Implementation will be added later
        _isConnected = false;
        await Task.CompletedTask;
    }
}
