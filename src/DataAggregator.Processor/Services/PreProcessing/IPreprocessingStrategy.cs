using DataAggregator.Collector.Shared.Models;
using DataAggregator.Processor.Configuration;

namespace DataAggregator.Processor.Services.PreProcessing;

/// <summary>
/// Interface for preprocessing strategies that convert raw measurement data into feature vectors for ML models.
/// </summary>
public interface IPreprocessingStrategy
{
    /// <summary>
    /// Preprocesses a list of measurements into a feature vector for a single prediction sample.
    /// </summary>
    /// <param name="measurements">List of raw measurements from the data window.</param>
    /// <param name="config">Configuration for the machine prediction.</param>
    /// <returns>Feature vector as float array for a single sample.</returns>
    public float[] PreprocessAsync(List<IMeasurementData> measurements, MachinePredictionConfig config);
}
