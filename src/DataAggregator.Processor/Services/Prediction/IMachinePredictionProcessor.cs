using DataAggregator.Processor.Configuration;

namespace DataAggregator.Processor.Services.Prediction;

/// <summary>
/// Interface for processing machine predictions.
/// </summary>
public interface IMachinePredictionProcessor
{
    /// <summary>
    /// Processes prediction for a specific machine.
    /// </summary>
    /// <param name="config">The machine prediction configuration.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public Task ProcessAsync(MachinePredictionConfig config);
}
