using DataAggregator.Collector.Shared.Models;
using DataAggregator.Processor.Services.Processing.Abstraction;

namespace DataAggregator.Processor.Services.Processing.PostProcessing.StateDeductionPostProcess;

/// <summary>
/// Post processor for state deduction which avoids bad transitions recognition
/// by not allowing state change if the change duration is less than a threshold.
/// </summary>
public class StateDeductionPostProcessor(StateDeductionPostProcessorConfig config) : IDataProcessor
{
    #region Public methods

    /// <inheritdoc/>
    public Task<IEnumerable<IMeasurementData>> ProcessAsync(IEnumerable<IMeasurementData> input)
    {
        var inputList = input.ToList();
        IMeasurementData? prediction = inputList.FirstOrDefault(x => x.SensorName == _resultOutputName);
        string currentPredictedState = prediction?.GetRawValue() as string ?? string.Empty;

        if (_lastState == null)
        {
            InitializeFirstState(currentPredictedState);
            return Task.FromResult<IEnumerable<IMeasurementData>>(inputList);
        }

        if (currentPredictedState == _lastState)
            return Task.FromResult<IEnumerable<IMeasurementData>>(inputList);

        if (currentPredictedState == _pendingState)
        {
            _stableCount++;
            _bufferedBatches.Add(inputList);

            if (_stableCount == config.Threshold)
                return ConfirmPendingState();

            return Task.FromResult(Enumerable.Empty<IMeasurementData>());
        }
        else
        {
            IEnumerable<IMeasurementData> flushed = FlushInvalidTransitionAsLastState();

            _pendingState = currentPredictedState;
            _stableCount = 1;
            _bufferedBatches.Clear();
            _bufferedBatches.Add(inputList);

            return Task.FromResult(flushed);
        }
    }

    #endregion

    #region Private methods

    private void InitializeFirstState(string state)
    {
        _lastState = state;
        _pendingState = state;
    }

    private Task<IEnumerable<IMeasurementData>> ConfirmPendingState()
    {
        _lastState = _pendingState;

        var confirmed = _bufferedBatches
            .SelectMany(batch => OverrideStateInBatch(batch, _lastState!))
            .ToList();

        _bufferedBatches.Clear();
        return Task.FromResult<IEnumerable<IMeasurementData>>(confirmed);
    }

    private IEnumerable<IMeasurementData> FlushInvalidTransitionAsLastState()
    {
        if (_bufferedBatches.Count == 0 || _lastState == null)
            return Enumerable.Empty<IMeasurementData>();

        var corrected = _bufferedBatches
            .SelectMany(batch => OverrideStateInBatch(batch, _lastState))
            .ToList();

        _bufferedBatches.Clear();
        return corrected;
    }

    private IEnumerable<IMeasurementData> OverrideStateInBatch(List<IMeasurementData> batch, string forcedValue)
        => batch.Select(
            x => x.SensorName == _resultOutputName
                ? new MeasurementData<string>(x.TimeStamp, x.SensorName, forcedValue)
                : x);

    #endregion

    #region Private fields

    private readonly string _resultOutputName = "PredictedLabel.output_0";

    private readonly List<List<IMeasurementData>> _bufferedBatches = [];
    private string? _lastState = null;
    private string? _pendingState = null;
    private int _stableCount = 0;

    #endregion
}
