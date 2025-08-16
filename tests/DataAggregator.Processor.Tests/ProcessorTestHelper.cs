using DataAggregator.Collector.Shared.Models;

namespace DataAggregator.Processor.Tests;

public static class ProcessorTestHelper
{
    public static IEnumerable<IMeasurementData> GetValidTestData()
    {
        DateTime now = DateTime.Now;

        return new List<IMeasurementData>
        {
            new MeasurementData<float>(now, "InterAxisMaxCorrelation", 0.167771f),
            new MeasurementData<float>(now, "InterAxisCorrelationVariance", 0.079077f),
            new MeasurementData<float>(now, "AxisLoadBalance", 0.145757f),
            new MeasurementData<float>(now, "TemporalStability", -0.189833f),
            new MeasurementData<float>(now, "GlobalSkewness", -0.014682f),
            new MeasurementData<string>(now, "Label", string.Empty),
        };
    }

    public static IEnumerable<IMeasurementData> GetValidProductionStateData()
    {
        DateTime now = DateTime.Now;
        return new List<IMeasurementData>
    {
        new MeasurementData<float>(now, "AxisLoadBalance", -0.115415f),
        new MeasurementData<float>(now, "InterAxisCorrelationVariance", 0.335840f),
        new MeasurementData<float>(now, "InterAxisMaxCorrelation", 0.262740f),
        new MeasurementData<float>(now, "TemporalStability", -0.271154f),
        new MeasurementData<float>(now, "GlobalSkewness", -0.203212f),
        new MeasurementData<string>(now, "Label", string.Empty),
    };
    }

    public static IEnumerable<IMeasurementData> GetValidShutdownStateData()
    {
        DateTime now = DateTime.Now;
        return new List<IMeasurementData>
    {
        new MeasurementData<float>(now, "AxisLoadBalance", 3.184065f),
        new MeasurementData<float>(now, "InterAxisCorrelationVariance", -2.281908f),
        new MeasurementData<float>(now, "InterAxisMaxCorrelation", -1.977913f),
        new MeasurementData<float>(now, "TemporalStability", 1.253469f),
        new MeasurementData<float>(now, "GlobalSkewness", 0.315182f),
        new MeasurementData<string>(now, "Label", string.Empty),
    };
    }

    public static IEnumerable<IMeasurementData> GetValidIdleStateData()
    {
        DateTime now = DateTime.Now;
        return new List<IMeasurementData>
    {
        new MeasurementData<float>(now, "AxisLoadBalance", -1.060774f),
        new MeasurementData<float>(now, "InterAxisCorrelationVariance", -0.848838f),
        new MeasurementData<float>(now, "InterAxisMaxCorrelation", -0.555483f),
        new MeasurementData<float>(now, "TemporalStability", 1.017235f),
        new MeasurementData<float>(now, "GlobalSkewness", 1.114139f),
        new MeasurementData<string>(now, "Label", string.Empty),
    };
    }
}
