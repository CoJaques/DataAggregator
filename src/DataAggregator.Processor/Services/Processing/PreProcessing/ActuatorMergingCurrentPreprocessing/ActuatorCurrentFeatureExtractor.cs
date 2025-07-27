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

        if (input.Count() != 0)
            meanTime = input.ElementAt(input.Count() / 2).TimeStamp;
        else
            meanTime = DateTime.UtcNow;

        var result = new List<IMeasurementData>
        {
            new MeasurementData<float>(meanTime, "GlobalActivityRatio", normalizedFeatures[0]),
            new MeasurementData<float>(meanTime, "GlobalChangeDensity", normalizedFeatures[1]),
            new MeasurementData<float>(meanTime, "InterAxisMeanCorrelation", normalizedFeatures[2]),
            new MeasurementData<float>(meanTime, "InterAxisMaxCorrelation", normalizedFeatures[3]),
            new MeasurementData<float>(meanTime, "InterAxisCorrelationVariance", normalizedFeatures[4]),
            new MeasurementData<float>(meanTime, "AxisSynchronization", normalizedFeatures[5]),
            new MeasurementData<float>(meanTime, "AxisLoadBalance", normalizedFeatures[6]),
            new MeasurementData<float>(meanTime, "TemporalStability", normalizedFeatures[7]),
            new MeasurementData<float>(meanTime, "GlobalSkewness", normalizedFeatures[8]),
            new MeasurementData<float>(meanTime, "GlobalKurtosis", normalizedFeatures[9]),
            new MeasurementData<float>(meanTime, "GlobalTrendSlope", normalizedFeatures[10]),
            new MeasurementData<float>(meanTime, "CoefficientOfVariation", normalizedFeatures[11]),
            new MeasurementData<float>(meanTime, "NormalizedIqrMedian", normalizedFeatures[12]),
            new MeasurementData<float>(meanTime, "NormalizedIqrMean", normalizedFeatures[13]),
            new MeasurementData<string>(meanTime, "Label", string.Empty),
        };

        return Task.FromResult(result.AsEnumerable());
    }

    #endregion

    #region Private methods

    private float[] ExtractFeatures(IEnumerable<IMeasurementData> measurements)
    {
        if (measurements == null)
        {
            Log.Warning("No measurements provided for feature extraction.");
            return new float[14];
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
            return new float[14];
        }

        // Calculate global statistics
        float globalStd = MathUtils.StandardDeviation(allCurrents);
        float globalMean = MathUtils.Mean(allCurrents);
        float globalMedian = MathUtils.Percentile(allCurrents, 50);
        float globalQ25 = MathUtils.Percentile(allCurrents, 25);
        float globalQ75 = MathUtils.Percentile(allCurrents, 75);
        float globalIqr = globalQ75 - globalQ25;

        // Feature 1: Global Activity Ratio
        float activityThreshold = globalStd * 2;
        float activeRatio = allCurrents.Count(x => Math.Abs(x) > activityThreshold) / (float)allCurrents.Count;

        // Feature 2: Global Change Density
        var diffSignals = allCurrents.Zip(allCurrents.Skip(1), (a, b) => Math.Abs(b - a)).ToList();
        int significantChanges = diffSignals.Count(x => x > globalStd * 1.5);
        float changeDensity = significantChanges / (float)allCurrents.Count;

        // Extract actuator currents for correlation analysis
        List<List<float>> actuatorsCurrents = ExtractActuatorCurrents(measurements);

        // Features 3-5: Inter-actuator correlations
        List<float> correlations = CalculateInterActuatorCorrelations(actuatorsCurrents);
        float meanCorrelation = correlations.Count > 0 ? correlations.Average() : 0f;
        float maxCorrelation = correlations.Count > 0 ? correlations.Max() : 0f;
        float correlationVariance = correlations.Count > 0 ? MathUtils.StandardDeviation(correlations) : 0f;

        // Feature 6: Actuator Synchronization
        var actuatorsMeans = actuatorsCurrents.Select(MathUtils.Mean).ToList();
        float meanOfMeans = actuatorsMeans.Average();
        float synchronization = meanOfMeans != 0 ? 1 - (MathUtils.StandardDeviation(actuatorsMeans) / Math.Abs(meanOfMeans)) : 1f;

        // Feature 7: Actuator Load Balance
        var actuatorsEnergies = actuatorsCurrents.Select(actuator => actuator.Sum(x => x * x)).ToList();
        float meanEnergy = actuatorsEnergies.Average();
        float loadBalance = meanEnergy != 0 ? 1 - (MathUtils.StandardDeviation(actuatorsEnergies) / meanEnergy) : 1f;

        // Feature 8: Temporal Stability
        float temporalStability = CalculateTemporalStability(allCurrents);

        // Features 9-10: Global Skewness and Kurtosis
        float globalSkewness = MathUtils.Skewness(allCurrents);
        float globalKurtosis = MathUtils.Kurtosis(allCurrents);

        // Feature 11: Global Trend Slope
        float trendSlope = CalculateTrendSlope(allCurrents);

        // Features 12-14: Normalized coefficients
        float coeffVar = Math.Abs(globalMean) > 1e-8f ? globalStd / globalMean : 0f;
        float normIqrMedian = Math.Abs(globalMedian) > 1e-8f ? globalIqr / globalMedian : 0f;
        float normIqrMean = Math.Abs(globalMean) > 1e-8f ? globalIqr / globalMean : 0f;

        return
        [
            activeRatio,
            changeDensity,
            meanCorrelation,
            maxCorrelation,
            correlationVariance,
            synchronization,
            loadBalance,
            temporalStability,
            globalSkewness,
            globalKurtosis,
            trendSlope,
            coeffVar,
            normIqrMedian,
            normIqrMean,
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

    private float CalculateTrendSlope(List<float> allCurrents)
    {
        if (allCurrents.Count < 2)
        {
            return 0f;
        }

        var timeIndices = Enumerable.Range(0, allCurrents.Count).Select(x => (float)x).ToList();
        float meanTime = timeIndices.Average();
        float meanCurrent = allCurrents.Average();

        float numerator = timeIndices.Zip(allCurrents, (t, c) => (t - meanTime) * (c - meanCurrent)).Sum();
        float denominator = timeIndices.Sum(t => (t - meanTime) * (t - meanTime));

        return denominator != 0 ? numerator / denominator : 0f;
    }

    private float[] NormalizeFeaturesAsync(float[] features, PreprocessingConfig preprocessing)
    {
        if (!preprocessing.EnableZScoreNormalization)
        {
            return features;
        }

        string[] featureNames =
        [
            "GlobalActivityRatio", "GlobalChangeDensity", "InterAxisMeanCorrelation",
            "InterAxisMaxCorrelation", "InterAxisCorrelationVariance", "AxisSynchronization",
            "AxisLoadBalance", "TemporalStability", "GlobalSkewness", "GlobalKurtosis",
            "GlobalTrendSlope", "CoefficientOfVariation", "NormalizedIqrMedian", "NormalizedIqrMean",
        ];

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
