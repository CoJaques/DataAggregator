namespace DataAggregator.Processor.Services.PreProcessing.ActuatorMergingCurrentPreprocessing;

/// <summary>
/// Utility class for mathematical operations used in feature extraction.
/// </summary>
public static class MathUtils
{
    /// <summary>
    /// Calculates the mean of a collection of values.
    /// </summary>
    /// <param name="values">Collection of float values.</param>
    /// <returns>Mean value.</returns>
    public static float Mean(IEnumerable<float> values)
    {
        return values == null || !values.Any() ? 0.0f : values.Average();
    }

    /// <summary>
    /// Calculates the standard deviation of a collection of values.
    /// </summary>
    /// <param name="values">Collection of float values.</param>
    /// <returns>Standard deviation.</returns>
    public static float StandardDeviation(IEnumerable<float> values)
    {
        if (values == null || values.Count() < 2)
        {
            return 0.0f;
        }

        float mean = values.Average();
        float variance = values.Select(x => (x - mean) * (x - mean)).Average();

        return (float)Math.Sqrt(variance);
    }

    /// <summary>
    /// Calculates the percentile value from a collection of values.
    /// </summary>
    /// <param name="values">Collection of float values.</param>
    /// <param name="percentile">Percentile value (0-100).</param>
    /// <returns>Percentile value.</returns>
    public static float Percentile(IEnumerable<float> values, float percentile)
    {
        if (values == null || !values.Any())
        {
            return 0.0f;
        }

        var sorted = values.OrderBy(x => x).ToList();
        double index = percentile / 100.0 * (sorted.Count - 1);
        int lower = (int)Math.Floor(index);
        int upper = (int)Math.Ceiling(index);

        if (lower == upper)
        {
            return sorted[lower];
        }

        double weight = index - lower;
        return (float)((sorted[lower] * (1 - weight)) + (sorted[upper] * weight));
    }

    /// <summary>
    /// Calculates the skewness of a collection of values.
    /// </summary>
    /// <param name="values">Collection of float values.</param>
    /// <returns>Skewness value.</returns>
    public static float Skewness(IEnumerable<float> values)
    {
        if (values == null || values.Count() < 3)
        {
            return 0.0f;
        }

        var valuesList = values.ToList();
        float mean = valuesList.Average();
        float std = StandardDeviation(valuesList);

        if (std == 0)
        {
            return 0.0f;
        }

        double skew = valuesList.Select(x => Math.Pow((x - mean) / std, 3)).Average();

        return (float)skew;
    }

    /// <summary>
    /// Calculates the kurtosis of a collection of values.
    /// </summary>
    /// <param name="values">Collection of float values.</param>
    /// <returns>Kurtosis value.</returns>
    public static float Kurtosis(IEnumerable<float> values)
    {
        if (values == null || values.Count() < 4)
        {
            return 0.0f;
        }

        var valuesList = values.ToList();
        float mean = valuesList.Average();
        float std = StandardDeviation(valuesList);

        if (std == 0)
        {
            return 0.0f;
        }

        double kurt = valuesList.Select(x => Math.Pow((x - mean) / std, 4)).Average() - 3;

        return (float)kurt;
    }

    /// <summary>
    /// Calculates the correlation coefficient between two collections of values.
    /// </summary>
    /// <param name="x">First collection of values.</param>
    /// <param name="y">Second collection of values.</param>
    /// <returns>Correlation coefficient.</returns>
    public static float Correlation(IEnumerable<float> x, IEnumerable<float> y)
    {
        if (x == null || y == null || !x.Any() || !y.Any())
        {
            return 0.0f;
        }

        var xList = x.ToList();
        var yList = y.ToList();

        if (xList.Count != yList.Count || xList.Count < 2)
        {
            return 0.0f;
        }

        float meanX = xList.Average();
        float meanY = yList.Average();

        float numerator = xList.Zip(yList, (xi, yi) => (xi - meanX) * (yi - meanY)).Sum();
        float denomX = xList.Select(xi => (xi - meanX) * (xi - meanX)).Sum();
        float denomY = yList.Select(yi => (yi - meanY) * (yi - meanY)).Sum();

        double denominator = Math.Sqrt(denomX * denomY);

        return denominator == 0 ? 0.0f : (float)(numerator / denominator);
    }
}
