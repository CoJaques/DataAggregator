using DataAggregator.Collector.Shared.Models;
using DataAggregator.Processor.Configuration;
using Serilog;

namespace DataAggregator.Processor.Services.PreProcessing.ActuatorMergingCurrentPreprocessing;

/// <summary>
/// Feature extractor for actuator current data based on ML.NET approach.
/// Extracts 14 agnostic features with Z-score normalization.
/// </summary>
public class ActuatorCurrentFeatureExtractor : IPreprocessingStrategy
{
    #region Public methods

    /// <summary>
    /// Preprocesses actuator current measurements into a feature vector.
    /// </summary>
    /// <param name="measurements">List of raw measurements from the data window.</param>
    /// <param name="config">Configuration for the machine prediction.</param>
    /// <returns>Feature vector as dictionary mapping feature names to values for a single sample.</returns>
    public Dictionary<string, float[]> PreprocessAsync(List<IMeasurementData> measurements, MachinePredictionConfig config)
    {
        Log.Debug(
            "Preprocessing {Count} measurements for machine {MachineName}",
            measurements.Count,
            config.MachineName);

        // Extract the 14 features from measurements
        float[] features = ExtractFeatures(measurements, config.InputSensors);

        // Apply Z-score normalization if enabled
        float[] normalizedFeatures = NormalizeFeaturesAsync(features, config.Preprocessing);

        // Create dictionary with one key per feature
        var result = new Dictionary<string, float[]>
        {
            ["GlobalActivityRatio"] = [normalizedFeatures[0]],
            ["GlobalChangeDensity"] = [normalizedFeatures[1]],
            ["InterAxisMeanCorrelation"] = [normalizedFeatures[2]],
            ["InterAxisMaxCorrelation"] = [normalizedFeatures[3]],
            ["InterAxisCorrelationVariance"] = [normalizedFeatures[4]],
            ["AxisSynchronization"] = [normalizedFeatures[5]],
            ["AxisLoadBalance"] = [normalizedFeatures[6]],
            ["TemporalStability"] = [normalizedFeatures[7]],
            ["GlobalSkewness"] = [normalizedFeatures[8]],
            ["GlobalKurtosis"] = [normalizedFeatures[9]],
            ["GlobalTrendSlope"] = [normalizedFeatures[10]],
            ["CoefficientOfVariation"] = [normalizedFeatures[11]],
            ["NormalizedIqrMedian"] = [normalizedFeatures[12]],
            ["NormalizedIqrMean"] = [normalizedFeatures[13]],
        };

        Log.Debug("Preprocessing completed for machine {MachineName}", config.MachineName);
        return result;
    }

    #endregion

    #region Private methods

    private float[] ExtractFeatures(List<IMeasurementData> measurements, List<string> sensors)
    {
        if (measurements == null || sensors == null || sensors.Count == 0)
        {
            Log.Warning("No measurements or sensors provided for feature extraction.");
            return new float[14];
        }

        Log.Debug("Extracting features for {SensorCount} sensors", sensors.Count);

        // Concatenate all currents from all actuators (like in the notebook)
        var allCurrents = new List<float>();
        foreach (IMeasurementData measurement in measurements)
        {
            foreach (string sensor in sensors)
            {
                if (measurement.SensorName == sensor)
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
            }
        }

        // Filter out invalid values
        allCurrents = allCurrents.Where(x => !float.IsNaN(x) && !float.IsInfinity(x)).ToList();

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

        // Extract axis currents for correlation analysis
        List<List<float>> axisCurrents = ExtractAxisCurrents(measurements, sensors);

        // Features 3-5: Inter-axis correlations
        List<float> correlations = CalculateInterAxisCorrelations(axisCurrents);
        float meanCorrelation = correlations.Count > 0 ? correlations.Average() : 0f;
        float maxCorrelation = correlations.Count > 0 ? correlations.Max() : 0f;
        float correlationVariance = correlations.Count > 0 ? MathUtils.StandardDeviation(correlations) : 0f;

        // Feature 6: Axis Synchronization
        var axisMeans = axisCurrents.Select(MathUtils.Mean).ToList();
        float meanOfMeans = axisMeans.Average();
        float synchronization = meanOfMeans != 0 ? 1 - (MathUtils.StandardDeviation(axisMeans) / Math.Abs(meanOfMeans)) : 1f;

        // Feature 7: Axis Load Balance
        var axisEnergies = axisCurrents.Select(axis => axis.Sum(x => x * x)).ToList();
        float meanEnergy = axisEnergies.Average();
        float loadBalance = meanEnergy != 0 ? 1 - (MathUtils.StandardDeviation(axisEnergies) / meanEnergy) : 1f;

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

        return new float[]
        {
            activeRatio,            // 1. GlobalActivityRatio
            changeDensity,          // 2. GlobalChangeDensity
            meanCorrelation,        // 3. InterAxisMeanCorrelation
            maxCorrelation,         // 4. InterAxisMaxCorrelation
            correlationVariance,    // 5. InterAxisCorrelationVariance
            synchronization,        // 6. AxisSynchronization
            loadBalance,            // 7. AxisLoadBalance
            temporalStability,      // 8. TemporalStability
            globalSkewness,         // 9. GlobalSkewness
            globalKurtosis,         // 10. GlobalKurtosis
            trendSlope,             // 11. GlobalTrendSlope
            coeffVar,               // 12. CoefficientOfVariation
            normIqrMedian,          // 13. NormalizedIqrMedian
            normIqrMean,            // 14. NormalizedIqrMean
        };
    }

    private List<List<float>> ExtractAxisCurrents(List<IMeasurementData> measurements, List<string> sensors)
    {
        var axisCurrents = new List<List<float>>();

        // Group measurements by sensor
        var measurementsBySensor = measurements
            .GroupBy(m => m.SensorName)
            .ToDictionary(g => g.Key, g => g.ToList());

        // Extract currents for each sensor/axis
        foreach (string sensor in sensors)
        {
            var axisCurrent = new List<float>();
            if (measurementsBySensor.TryGetValue(sensor, out List<IMeasurementData>? sensorMeasurements))
            {
                foreach (IMeasurementData? measurement in sensorMeasurements.OrderBy(m => m.TimeStamp))
                {
                    object value = measurement.GetRawValue();
                    if (value is float floatValue)
                    {
                        axisCurrent.Add(floatValue);
                    }
                    else if (value is double doubleValue)
                    {
                        axisCurrent.Add((float)doubleValue);
                    }
                    else if (value is int intValue)
                    {
                        axisCurrent.Add(intValue);
                    }
                }
            }

            axisCurrents.Add(axisCurrent);
        }

        return axisCurrents;
    }

    private List<float> CalculateInterAxisCorrelations(List<List<float>> axisCurrents)
    {
        var correlations = new List<float>();

        for (int axis1 = 0; axis1 < axisCurrents.Count; axis1++)
        {
            for (int axis2 = axis1 + 1; axis2 < axisCurrents.Count; axis2++)
            {
                float corr = MathUtils.Correlation(axisCurrents[axis1], axisCurrents[axis2]);
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
