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
    private ICMCtlSampler? _samplerProxy;
    private string? _samplerName;
    private bool _isConnected;
    private bool _isFirstRead = true;

    // The frequency of data acquisition in nanoseconds defined by the selected thread
    private int _dataAcquisitionFrequency;
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
            _samplerProxy = _rpcClient.GetMain<ICMCtlSampler>();

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
            _dataAcquisitionFrequency = bestThread.Period;

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
            _rpcClient?.Dispose();
            _samplerProxy?.Dispose();
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
            if (_isFirstRead)
            {
                // Flush initial state
                await _samplerProxy.GetSamplesData(_samplerName);
                _isFirstRead = false;
            }

            (CMCtlSampler.SampleData data, bool success) = await _samplerProxy.GetSamplesData(_samplerName);
            if (!success || data.Samples.Count == 0)
            {
                Log.Debug("No new samples received.");
                return [];
            }

            CheckDataIntegrity(data);

            DateTime timestampNow = DateTime.UtcNow;
            double maxSampleId = data.Samples.Max(s => s.Id);
            var measurements = new List<IMeasurementData>();

            foreach (CMCtlSampler.SamplePoint sample in data.Samples)
            {
                OpenCnSensorConfig? pinConfig = _openCnConfig.Sensors.FirstOrDefault(s => s.PinName == sample.PinName);
                if (pinConfig == null)
                    continue;

                double nSample = maxSampleId - sample.Id;
                DateTime sampleTimestamp = timestampNow - TimeSpan.FromMicroseconds(nSample * _dataAcquisitionFrequency / 1_000.0);

                CMCtlPins.PinValue.value valueUnion = sample.Value.Value;
                IMeasurementData? measurement = pinConfig.DataType switch
                {
                    SensorDataType.Boolean when valueUnion.B.HasValue =>
                        new MeasurementData<bool>(sampleTimestamp, pinConfig.Name, valueUnion.B.Value),

                    SensorDataType.Integer when valueUnion.S.HasValue =>
                        new MeasurementData<int>(sampleTimestamp, pinConfig.Name, valueUnion.S.Value),

                    SensorDataType.Double or SensorDataType.Float when valueUnion.F.HasValue =>
                        new MeasurementData<double>(sampleTimestamp, pinConfig.Name, valueUnion.F.Value),

                    _ => null,
                };

                if (measurement != null)
                {
                    measurements.Add(measurement);
                }
                else
                {
                    Log.Warning("Received null or unsupported value for pin {PinName}", pinConfig.Name);
                }
            }

            return measurements;
        }
        catch (System.Exception ex)
        {
            Log.Error(ex, "Error fetching sampler data");
            return [];
        }
    }

    private void CheckDataIntegrity(CMCtlSampler.SampleData data)
    {
        if (data.Samples.Count % _openCnConfig.Sensors.Count != 0)
        {
            Log.Warning(
                "Inconsistent sample count: received {Count} samples for {SensorCount} sensors",
                data.Samples.Count,
                _openCnConfig.Sensors.Count);
        }

        IEnumerable<IGrouping<string, CMCtlSampler.SamplePoint>> groupedByPin = data.Samples.GroupBy(s => s.PinName);
        foreach (IGrouping<string, CMCtlSampler.SamplePoint> group in groupedByPin)
        {
            double[] ids = group.Select(s => s.Id).ToArray();
            for (int i = 1; i < ids.Length; i++)
            {
                if (ids[i] != ids[i - 1] + 1)
                {
                    Log.Warning(
                        "Non-consecutive IDs for pin {PinName}: {PrevId} -> {CurrentId}",
                        group.Key,
                        ids[i - 1],
                        ids[i]);
                    break;
                }
            }
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
