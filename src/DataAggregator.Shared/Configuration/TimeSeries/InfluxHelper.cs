namespace DataAggregator.Shared.Configuration.TimeSeries;

/// <summary>
/// Static containing helper informations for InfluxDB operations.
/// </summary>
public static class InfluxHelper
{
    /// <summary>
    /// Constants for InfluxDB database name.
    /// </summary>
    public const string DatabaseName = "Dataggregator";

    /// <summary>
    /// Constant for influxDb measurement table name for predictions.
    /// </summary>
    public const string PredictionTableName = "Predictions";

    /// <summary>
    /// Constant for influxDb tag for machine name.
    /// </summary>
    public const string MachineNameTag = "MachineName";
}
