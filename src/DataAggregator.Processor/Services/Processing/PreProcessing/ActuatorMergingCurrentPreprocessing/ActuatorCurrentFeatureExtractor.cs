using DataAggregator.Collector.Shared.Models;
using DataAggregator.Processor.Services.Processing.Abstraction;
using Serilog;

namespace DataAggregator.Processor.Services.Processing.PreProcessing.ActuatorMergingCurrentPreprocessing;

/// <summary>
/// Feature extractor for actuator current data based on ML.NET approach.
/// Extracts 14 agnostic features with Z-score normalization.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="ActuatorCurrentFeatureExtractor"/> class.
/// </remarks>
/// <param name="config">The configuration of the processor.</param>
public class ActuatorCurrentFeatureExtractor(PreprocessingConfig config) : IDataProcessor
{
    #region Public methods

    /// <summary>
    /// Preprocesses actuator current measurements into a feature vector.
    /// </summary>
    /// <param name="input">List of raw measurements from the data window.</param>
    /// <returns>Feature vector as dictionary mapping feature names to values for a single sample.</returns>
    public Task<IEnumerable<IMeasurementData>> ProcessAsync(IEnumerable<IMeasurementData> input)
    {
        // Extract the 14 features from measurements
        float[] features = ExtractFeatures(input);

        // Apply Z-score normalization if enabled
        float[] normalizedFeatures = NormalizeFeaturesAsync(features, config);

        DateTime meanTime;

        if (input.Any())
            meanTime = input.Min(x => x.TimeStamp);
        else
            meanTime = DateTime.UtcNow;

        string[] featureNames = config.NormalizationParameters.Keys.ToArray();

        var result = new List<IMeasurementData>();

        for (int i = 0; i < normalizedFeatures.Length && i < featureNames.Length; i++)
        {
            result.Add(new MeasurementData<float>(meanTime, featureNames[i], normalizedFeatures[i]));
        }

        result.Add(new MeasurementData<string>(meanTime, "Label", string.Empty)); // Placeholder for label

        return Task.FromResult(result.AsEnumerable());
    }

    #endregion

    #region Private methods

    private float[] ExtractFeatures(IEnumerable<IMeasurementData> measurements)
    {
        if (measurements == null)
        {
            Log.Warning("No measurements provided for feature extraction.");
            return new float[5];
        }

        // Concatenate all currents from all actuators
        var allCurrents = new List<float>();
        foreach (IMeasurementData measurement in measurements)
        {
            object value = measurement.GetRawValue();
            if (value is float floatValue)
            {
                allCurrents.Add(floatValue);
            }
            else if (value is double doubleValue)
            {
                allCurrents.Add((float)doubleValue);
            }
            else if (value is int intValue)
            {
                allCurrents.Add(intValue);
            }
        }

        // Filter out invalid values
        allCurrents = [.. allCurrents.Where(x => !float.IsNaN(x) && !float.IsInfinity(x))];

        if (allCurrents.Count == 0)
        {
            Log.Warning("No valid current values found for feature extraction.");
            return new float[5];
        }

        List<List<float>> actuatorsCurrents = ExtractActuatorCurrents(measurements);

        List<float> correlations = CalculateInterActuatorCorrelations(actuatorsCurrents);
        float maxCorrelation = correlations.Count > 0 ? correlations.Max() : 0f;
        float correlationVariance = correlations.Count > 0 ? MathUtils.StandardDeviation(correlations) : 0f;

        var actuatorsEnergies = actuatorsCurrents.Select(actuator => actuator.Sum(x => x * x)).ToList();
        float meanEnergy = actuatorsEnergies.Average();
        float loadBalance = meanEnergy != 0 ? 1 - (MathUtils.StandardDeviation(actuatorsEnergies) / meanEnergy) : 1f;
        float temporalStability = CalculateTemporalStability(allCurrents);

        float globalSkewness = MathUtils.Skewness(allCurrents);

        return
        [
            loadBalance,
            correlationVariance,
            maxCorrelation,
            temporalStability,
            globalSkewness,
        ];
    }

    private List<List<float>> ExtractActuatorCurrents(IEnumerable<IMeasurementData> measurements)
    {
        var actuatorsCurrents = new List<List<float>>();

        // Group measurements by sensor
        var measurementsBySensor = measurements
            .GroupBy(m => m.SensorName)
            .ToDictionary(g => g.Key, g => g.ToList());

        var sensors = measurementsBySensor.Keys.ToList();

        // Extract currents for each sensor/Actuator
        foreach (string sensor in sensors)
        {
            var actuatorCurrent = new List<float>();
            if (measurementsBySensor.TryGetValue(sensor, out List<IMeasurementData>? sensorMeasurements))
            {
                foreach (IMeasurementData? measurement in sensorMeasurements.OrderBy(m => m.TimeStamp))
                {
                    object value = measurement.GetRawValue();
                    if (value is float floatValue)
                    {
                        actuatorCurrent.Add(floatValue);
                    }
                    else if (value is double doubleValue)
                    {
                        actuatorCurrent.Add((float)doubleValue);
                    }
                    else if (value is int intValue)
                    {
                        actuatorCurrent.Add(intValue);
                    }
                }
            }

            actuatorsCurrents.Add(actuatorCurrent);
        }

        return actuatorsCurrents;
    }

    private List<float> CalculateInterActuatorCorrelations(List<List<float>> actuatorsCurrents)
    {
        var correlations = new List<float>();

        for (int actuator1 = 0; actuator1 < actuatorsCurrents.Count; actuator1++)
        {
            for (int actuator2 = actuator1 + 1; actuator2 < actuatorsCurrents.Count; actuator2++)
            {
                float corr = MathUtils.Correlation(actuatorsCurrents[actuator1], actuatorsCurrents[actuator2]);
                if (!float.IsNaN(corr) && !float.IsInfinity(corr))
                {
                    correlations.Add(corr);
                }
            }
        }

        return correlations;
    }

    private float CalculateTemporalStability(List<float> allCurrents)
    {
        int segmentSize = allCurrents.Count / 4;
        var segmentVars = new List<float>();

        if (segmentSize > 1)
        {
            for (int seg = 0; seg < 4; seg++)
            {
                int startSeg = seg * segmentSize;
                int endSeg = Math.Min((seg + 1) * segmentSize, allCurrents.Count);

                if (endSeg > startSeg)
                {
                    IEnumerable<float> segmentData = allCurrents.Skip(startSeg).Take(endSeg - startSeg);
                    float segVar = MathUtils.StandardDeviation(segmentData);
                    segmentVars.Add(segVar * segVar);
                }
            }
        }

        return segmentVars.Count > 1 && segmentVars.Average() > 0
            ? 1 - (MathUtils.StandardDeviation(segmentVars) / segmentVars.Average())
            : 1f;
    }

    private float[] NormalizeFeaturesAsync(float[] features, PreprocessingConfig preprocessing)
    {
        if (!preprocessing.EnableZScoreNormalization)
        {
            return features;
        }

        string[] featureNames = preprocessing.NormalizationParameters.Keys.ToArray();

        float[] normalized = new float[features.Length];

        for (int i = 0; i < features.Length && i < featureNames.Length; i++)
        {
            string featureName = featureNames[i];
            if (preprocessing.NormalizationParameters.TryGetValue(featureName, out float[]? parameters) && parameters.Length >= 2)
            {
                float mean = parameters[0];
                float std = parameters[1];
                normalized[i] = std > 1e-6f ? (features[i] - mean) / std : features[i];
            }
            else
            {
                normalized[i] = features[i]; // No normalization if parameters not found
            }
        }

        return normalized;
    }

    #endregion
}
