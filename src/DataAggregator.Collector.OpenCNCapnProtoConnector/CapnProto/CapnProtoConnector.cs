using Capnp.Rpc;
using CapnpGen;
using DataAggregator.Collector.OpenCNCapnProtoConnector.OpenCN;
using DataAggregator.Collector.Shared.Abstraction;
using DataAggregator.Collector.Shared.Models;
using DataAggregator.Shared.Domain.DataType;
using Serilog;

namespace DataAggregator.Collector.OpenCNCapnProtoConnector.CapnProto;

/// <summary>
/// Connector for Cap'n Proto data source.
/// </summary>
public class CapnProtoConnector(OpenCnCollectorConfiguration config) : IDataSourceConnector
{
    #region Private Fields
    private readonly OpenCnCollectorConfiguration _openCnConfig = config;
    private TcpRpcClient? _rpcClient;
    private CMCtlSampler_Proxy? _samplerProxy;
    private string? _samplerName;
    private bool _isConnected;
    #endregion

    #region Public Methods

    /// <inheritdoc/>
    public async Task ConnectAsync()
    {
        if (_isConnected)
        {
            Log.Debug("Already connected to Cap'n Proto server, disconnecting first");
            await DisconnectAsync();
        }

        Log.Information(
            "Connecting to Cap'n Proto server at {ServerAddress}:{Port}",
            _openCnConfig.CapnProto.ServerAddress,
            _openCnConfig.CapnProto.Port);

        try
        {
            _rpcClient = new TcpRpcClient(_openCnConfig.CapnProto.ServerAddress, _openCnConfig.CapnProto.Port);
            _samplerProxy = _rpcClient.GetMain<CMCtlSampler_Proxy>();

            // Validate threads
            IReadOnlyList<CMCtlThreads.Thread> threads = await _samplerProxy.GetThreadList();
            if (threads == null || threads.Count == 0)
                throw new InvalidOperationException("No sampling threads available on server");

            double targetHz = _openCnConfig.SamplingRate;
            CMCtlThreads.Thread bestThread = threads
                .Select(t => new { Thread = t, Freq = t.Period > 0 ? 1_000_000_000.0 / t.Period : 0 })
                .OrderBy(x => Math.Abs(x.Freq - targetHz))
                .First().Thread;
            Log.Information("Selected thread {Name} at {Freq:F2} Hz", bestThread.Name, 1_000_000_000.0 / bestThread.Period);

            // Validate pins
            IReadOnlyList<CMCtlPins.Pin> pins = await _samplerProxy.GetPinList();
            var availablePins = pins.Select(p => p.Name).ToHashSet();
            var requestedPins = _openCnConfig.Sensors.Select(s => s.PinName).ToList();

            foreach (string? pin in requestedPins)
            {
                if (!availablePins.Contains(pin))
                    throw new InvalidOperationException($"Requested pin '{pin}' not available on server");
            }

            _samplerName = "monitorSampler";
            bool started = await _samplerProxy.StartSampling(
                requestedPins,
                (uint)_openCnConfig.Sensors.Count,
                bestThread.Name,
                _samplerName);

            if (!started)
                throw new InvalidOperationException("Failed to start sampler");

            _isConnected = true;
            Log.Information("Connected and sampler started successfully");
        }
        catch (System.Exception ex)
        {
            Log.Error(ex, "Failed to connect and initialize sampler");
            _isConnected = false;
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<IMeasurementData>> FetchDataAsync()
    {
        if (!_isConnected || _samplerProxy == null || _samplerName == null)
        {
            Log.Warning("Attempted to fetch data while not connected or sampler not initialized");
            return [];
        }

        try
        {
            (CMCtlSampler.SampleData data, bool success) = await _samplerProxy.GetSamplesData(_samplerName);
            if (!success)
            {
                Log.Warning("GetSamplesData failed for sampler");
                return [];
            }

            IReadOnlyList<CMCtlSampler.SamplePoint> samples = data.Samples;
            DateTime timestamp = DateTime.UtcNow;
            var result = new List<IMeasurementData>();

            foreach (CMCtlSampler.SamplePoint sample in samples)
            {
                OpenCnSensorConfig? pinConfig = _openCnConfig.Sensors.FirstOrDefault(s => s.PinName == sample.PinName);
                if (pinConfig == null)
                    continue;

                CMCtlPins.PinValue.value valueUnion = sample.Value.Value;
                switch (pinConfig.DataType)
                {
                    case SensorDataType.Boolean:
                        if (valueUnion.B == null)
                        {
                            Log.Warning("Received null boolean value for pin {PinName}", pinConfig.Name);
                            break;
                        }

                        result.Add(new MeasurementData<bool>(timestamp, pinConfig.Name, (bool)valueUnion.B));
                        break;
                    case SensorDataType.Integer:
                        if (valueUnion.S == null)
                        {
                            Log.Warning("Received null integer value for pin {PinName}", pinConfig.Name);
                            break;
                        }

                        result.Add(new MeasurementData<int>(timestamp, pinConfig.Name, (int)valueUnion.S));
                        break;
                    case SensorDataType.Double:
                    case SensorDataType.Float:
                        if (valueUnion.F == null)
                        {
                            Log.Warning("Received null float/double value for pin {PinName}", pinConfig.Name);
                            break;
                        }

                        result.Add(new MeasurementData<double>(timestamp, pinConfig.Name, (double)valueUnion.F));
                        break;
                    default:
                        Log.Warning("Unsupported data type for pin {PinName}", pinConfig.Name);
                        break;
                }
            }

            return result;
        }
        catch (System.Exception ex)
        {
            Log.Error(ex, "Error fetching sampler data");
            return [];
        }
    }

    /// <inheritdoc/>
    public Task<bool> IsConnectedAsync() => Task.FromResult(_isConnected);

    /// <inheritdoc/>
    public async Task DisconnectAsync()
    {
        if (!_isConnected)
            return;

        Log.Information("Disconnecting from Cap'n Proto server");

        try
        {
            if (_samplerProxy != null && _samplerName != null)
            {
                bool stopped = await _samplerProxy.StopSampling(_samplerName);
                if (!stopped)
                {
                    Log.Warning("Failed to stop sampler properly");
                }
            }

            _rpcClient?.Dispose();
            _isConnected = false;
            Log.Information("Disconnected from Cap'n Proto server");
        }
        catch (System.Exception ex)
        {
            Log.Error(ex, "Error disconnecting from Cap'n Proto server");
            throw;
        }
    }
    #endregion
}
