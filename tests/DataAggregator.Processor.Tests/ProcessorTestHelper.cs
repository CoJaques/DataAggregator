using DataAggregator.Collector.Shared.Models;

namespace DataAggregator.Processor.Tests;

public static class ProcessorTestHelper
{
    public static IEnumerable<IMeasurementData> GetValidTestData()
    {
        DateTime now = DateTime.Now;

        return new List<IMeasurementData>
        {
            new MeasurementData<float>(now, "GlobalActivityRatio", 1.0f),
            new MeasurementData<float>(now, "GlobalChangeDensity", 2.0f),
            new MeasurementData<float>(now, "InterAxisMeanCorrelation", 3.0f),
            new MeasurementData<float>(now, "InterAxisMaxCorrelation", 4.0f),
            new MeasurementData<float>(now, "InterAxisCorrelationVariance", 5.0f),
            new MeasurementData<float>(now, "AxisSynchronization", 6.0f),
            new MeasurementData<float>(now, "AxisLoadBalance", 7.0f),
            new MeasurementData<float>(now, "TemporalStability", 8.0f),
            new MeasurementData<float>(now, "GlobalSkewness", 9.0f),
            new MeasurementData<float>(now, "GlobalKurtosis", 10.0f),
            new MeasurementData<float>(now, "GlobalTrendSlope", 11.0f),
            new MeasurementData<float>(now, "CoefficientOfVariation", 12.0f),
            new MeasurementData<float>(now, "NormalizedIqrMedian", 13.0f),
            new MeasurementData<float>(now, "NormalizedIqrMean", 14.0f),
            new MeasurementData<string>(now, "Label", string.Empty),
        };
    }

    public static IEnumerable<IMeasurementData> GetValidShutdownStateData()
    {
        DateTime now = DateTime.Now;
        return new List<IMeasurementData>
        {
            new MeasurementData<float>(now, "GlobalActivityRatio",  -0.235608f),
            new MeasurementData<float>(now, "GlobalChangeDensity", -3.017057f),
            new MeasurementData<float>(now, "InterAxisMeanCorrelation", 0.018719f),
            new MeasurementData<float>(now, "InterAxisMaxCorrelation", -1.639018f),
            new MeasurementData<float>(now, "InterAxisCorrelationVariance", -1.741219f),
            new MeasurementData<float>(now, "AxisSynchronization", 0.177111f),
            new MeasurementData<float>(now, "AxisLoadBalance", 3.195790f),
            new MeasurementData<float>(now, "TemporalStability", 0.542284f),
            new MeasurementData<float>(now, "GlobalSkewness", 0.357844f),
            new MeasurementData<float>(now, "GlobalKurtosis",  1.441117f),
            new MeasurementData<float>(now, "GlobalTrendSlope", 0.005194f),
            new MeasurementData<float>(now, "CoefficientOfVariation",0.073491f),
            new MeasurementData<float>(now, "NormalizedIqrMedian", -0.005889f),
            new MeasurementData<float>(now, "NormalizedIqrMean", 0.052909f),
            new MeasurementData<string>(now, "Label", string.Empty),
        };
    }

    public static IEnumerable<IMeasurementData> GetValidProductionStateData()
    {
        DateTime now = DateTime.Now;
        return new List<IMeasurementData>
        {
            new MeasurementData<float>(now, "GlobalActivityRatio", -0.235608f),
            new MeasurementData<float>(now, "GlobalChangeDensity", -0.063313f),
            new MeasurementData<float>(now, "InterAxisMeanCorrelation", 0.353871f),
            new MeasurementData<float>(now, "InterAxisMaxCorrelation", 0.167771f),
            new MeasurementData<float>(now, "InterAxisCorrelationVariance", 0.079077f),
            new MeasurementData<float>(now, "AxisSynchronization", 0.142284f),
            new MeasurementData<float>(now, "AxisLoadBalance", 0.145757f),
            new MeasurementData<float>(now, "TemporalStability", -0.189833f),
            new MeasurementData<float>(now, "GlobalSkewness", -0.014682f),
            new MeasurementData<float>(now, "GlobalKurtosis", -1.229796f),
            new MeasurementData<float>(now, "GlobalTrendSlope", -2.419304f),
            new MeasurementData<float>(now, "CoefficientOfVariation", 0.037766f),
            new MeasurementData<float>(now, "NormalizedIqrMedian", -0.051617f),
            new MeasurementData<float>(now, "NormalizedIqrMean", 0.008711f),
            new MeasurementData<string>(now, "Label", string.Empty),
        };
    }

    public static IEnumerable<IMeasurementData> GetValidIdleStateData()
    {
        DateTime now = DateTime.Now;
        return new List<IMeasurementData>
        {
            new MeasurementData<float>(now, "GlobalActivityRatio", -0.235608f),
            new MeasurementData<float>(now, "GlobalChangeDensity", 0.471617f),
            new MeasurementData<float>(now, "InterAxisMeanCorrelation", 0.087918f),
            new MeasurementData<float>(now, "InterAxisMaxCorrelation", -0.901309f),
            new MeasurementData<float>(now, "InterAxisCorrelationVariance", -1.025950f),
            new MeasurementData<float>(now, "AxisSynchronization", -0.045073f),
            new MeasurementData<float>(now, "AxisLoadBalance", -1.059821f),
            new MeasurementData<float>(now, "TemporalStability", 0.526964f),
            new MeasurementData<float>(now, "GlobalSkewness", 1.200339f),
            new MeasurementData<float>(now, "GlobalKurtosis", 1.352620f),
            new MeasurementData<float>(now, "GlobalTrendSlope", -0.011385f),
            new MeasurementData<float>(now, "CoefficientOfVariation", -0.135674f),
            new MeasurementData<float>(now, "NormalizedIqrMedian", -0.005889f),
            new MeasurementData<float>(now, "NormalizedIqrMean", -0.004563f),
            new MeasurementData<string>(now, "Label", string.Empty),
        };
    }
}
