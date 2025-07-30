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
            return InitializeFirstState(inputList, currentPredictedState);

        if (currentPredictedState == _lastState)
            return HandleSameAsLastState(inputList, currentPredictedState);

        HandlePotentialStateChange(currentPredictedState, inputList);

        if (_stableCount >= config.Threshold)
            return ConfirmPendingState();

        return Task.FromResult(Enumerable.Empty<IMeasurementData>());
    }
    #endregion

    #region Private Methods
    private Task<IEnumerable<IMeasurementData>> InitializeFirstState(List<IMeasurementData> inputList, string state)
    {
        _lastState = state;
        _pendingState = state;
        return Task.FromResult<IEnumerable<IMeasurementData>>(inputList);
    }

    private Task<IEnumerable<IMeasurementData>> HandleSameAsLastState(List<IMeasurementData> inputList, string state)
    {
        if (_bufferedBatches.Count > 0)
        {
            var corrected = _bufferedBatches
                .SelectMany(batch => OverrideStateInBatch(batch, _lastState!))
                .ToList();

            _bufferedBatches.Clear();
            _pendingState = state;
            _stableCount = 0;

            return Task.FromResult<IEnumerable<IMeasurementData>>(corrected);
        }

        _pendingState = state;
        _stableCount = 0;
        return Task.FromResult<IEnumerable<IMeasurementData>>(inputList);
    }

    private void HandlePotentialStateChange(string currentState, List<IMeasurementData> inputList)
    {
        if (currentState == _pendingState)
        {
            _stableCount++;
            _bufferedBatches.Add(inputList);
        }
        else
        {
            _pendingState = currentState;
            _stableCount = 1;
            _bufferedBatches.Clear();
            _bufferedBatches.Add(inputList);
        }
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

    private IEnumerable<IMeasurementData> OverrideStateInBatch(List<IMeasurementData> batch, string forcedValue)
        => batch.Select(
            x => x.SensorName == _resultOutputName
                ? new MeasurementDataWrapper(x, forcedValue)
                : x);

    private class MeasurementDataWrapper(IMeasurementData original, string forcedValue) : IMeasurementData
    {
        public DateTime TimeStamp => original.TimeStamp;

        public string SensorName => original.SensorName;

        public Type ValueType => typeof(string);

        public object GetRawValue() => forcedValue;
    }
    #endregion

    #region private fields
    private readonly string _resultOutputName = "PredictedLabel.output_0";

    private readonly List<List<IMeasurementData>> _bufferedBatches = [];
    private string? _lastState = null;
    private string? _pendingState = null;
    private int _stableCount = 0;
    #endregion
}
