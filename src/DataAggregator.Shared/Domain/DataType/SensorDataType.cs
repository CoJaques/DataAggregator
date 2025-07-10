namespace DataAggregator.Shared.Domain.DataType;

/// <summary>
/// Enum which defines the types of data that can be produced by sensors.
/// </summary>
public enum SensorDataType
{
    /// <summary>
    /// Boolean type.
    /// </summary>
    Boolean,

    /// <summary>
    /// Integer type.
    /// </summary>
    Integer,

    /// <summary>
    /// Double-precision floating-point type.
    /// </summary>
    Double,

    /// <summary>
    /// String type.
    /// </summary>
    String,

    /// <summary>
    /// Single-precision floating-point type.
    /// </summary>
    Float,
}
