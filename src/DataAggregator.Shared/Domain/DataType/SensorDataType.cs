namespace DataAggregator.Shared.Domain.DataType;

/// <summary>
/// Enum which defines the types of data that can be produced by sensors.
/// </summary>
public enum SensorDataType
{
    /// <summary>
    /// Undefined type (used as sentinel value).
    /// </summary>
    Undefined = -1,

    /// <summary>
    /// Boolean type.
    /// </summary>
    Boolean = 0,

    /// <summary>
    /// Integer type.
    /// </summary>
    Integer = 1,

    /// <summary>
    /// Double-precision floating-point type.
    /// </summary>
    Double = 2,

    /// <summary>
    /// String type.
    /// </summary>
    String = 3,

    /// <summary>
    /// Single-precision floating-point type.
    /// </summary>
    Float = 4,
}
