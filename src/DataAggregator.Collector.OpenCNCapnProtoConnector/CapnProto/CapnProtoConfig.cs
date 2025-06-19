namespace DataAggregator.Collector.OpenCNCapnProtoConnector.CapnProto;

/// <summary>
/// Configuration for Cap'n Proto connection.
/// </summary>
public class CapnProtoConfig
{
    /// <summary>
    /// Gets or sets the Cap'n Proto server address.
    /// </summary>
    public string ServerAddress { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the server port.
    /// </summary>
    public int Port { get; set; } = 5000;

    /// <summary>
    /// Gets or sets the connection timeout in milliseconds.
    /// </summary>
    public int TimeoutMs { get; set; } = 5000;
}
