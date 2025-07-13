namespace DataAggregator.Processor.Configuration;

/// <summary>
/// Configuration for the prediction service.
/// </summary>
public class PredictionServiceConfiguration
{
    /// <summary>
    /// Gets or sets the registration service URL.
    /// </summary>
    public string RegistrationServiceUrl { get; set; } = "http://localhost:5001";

    /// <summary>
    /// Gets or sets the global cycle interval in seconds.
    /// </summary>
    public int GlobalCycleIntervalSeconds { get; set; } = 1;

    /// <summary>
    /// Gets or sets the list of machine prediction configurations.
    /// </summary>
    public List<MachinePredictionConfig> Machines { get; set; } = [];
}
