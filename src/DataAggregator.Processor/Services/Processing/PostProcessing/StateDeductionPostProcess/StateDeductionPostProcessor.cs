using DataAggregator.Collector.Shared.Models;
using DataAggregator.Processor.Services.Processing.Abstraction;

namespace DataAggregator.Processor.Services.Processing.PostProcessing.StateDeductionPostProcess;

/// <summary>
/// Post processor for state deduction which avoids bad transitions recognition
/// by not allowing state change if the change duration is less than a threshold.
/// </summary>
public class StateDeductionPostProcessor(StateDeductionPostProcessorConfig config) : IDataProcessor
{
    private readonly string _resultOutputName = "PredictedLabel.output_0";
    private string? _lastState = null;
    private int _stableCount = 0;

    /// <inheritdoc/>
    public Task<IEnumerable<IMeasurementData>> ProcessAsync(IEnumerable<IMeasurementData> input)
    {
        var inputList = input.ToList();
        IMeasurementData? prediction = inputList.FirstOrDefault(x => x.SensorName == _resultOutputName);
        string currentPredictedState = prediction?.GetRawValue() as string ?? string.Empty;

        if (_lastState == null)
        {
            // first run, initialize the last state
            _lastState = currentPredictedState;
            _stableCount = 1;
            return Task.FromResult<IEnumerable<IMeasurementData>>(inputList);
        }

        if (currentPredictedState == _lastState)
        {
            // stable state, increment the stable count
            _stableCount++;
            return Task.FromResult<IEnumerable<IMeasurementData>>(inputList);
        }
        else
        {
            // state change detected
            _stableCount = 1;

            // Accept the state change only if the stable count reaches the threshold
            if (_stableCount >= config.Threshold)
            {
                _lastState = currentPredictedState;
                return Task.FromResult<IEnumerable<IMeasurementData>>(inputList);
            }
            else
            {
                if (prediction != null)
                {
                    var forced = new MeasurementDataWrapper(prediction, _lastState!);
                    var output = inputList.Select(x => x.SensorName == _resultOutputName ? forced : x).ToList();
                    return Task.FromResult<IEnumerable<IMeasurementData>>(output);
                }
                else
                {
                    return Task.FromResult<IEnumerable<IMeasurementData>>(inputList);
                }
            }
        }
    }

    private class MeasurementDataWrapper(IMeasurementData original, string forcedValue) : IMeasurementData
    {
        public DateTime TimeStamp => original.TimeStamp;

        public string SensorName => original.SensorName;

        public Type ValueType => typeof(string);

        public object GetRawValue() => forcedValue;
    }
}
