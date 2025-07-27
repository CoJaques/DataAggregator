namespace DataAggregator.Shared.Domain.DataType;

/// <summary>
/// Extension methods for <see cref="SensorDataType"/> to map to CLR types.
/// </summary>
public static class SensorDataTypeExtension
{
    /// <summary>
    /// Gets the CLR type corresponding to the sensor data type.
    /// </summary>
    /// <param name="dataType">The sensor data type.</param>
    /// <returns>The CLR type.</returns>
    public static Type GetClrType(this SensorDataType dataType)
        => dataType switch
        {
            SensorDataType.Undefined => throw new ArgumentException("Undefined data type cannot be mapped to CLR type"),
            SensorDataType.Boolean => typeof(bool),
            SensorDataType.Integer => typeof(int),
            SensorDataType.Double => typeof(double),
            SensorDataType.String => typeof(string),
            SensorDataType.Float => typeof(float),
            _ => throw new ArgumentOutOfRangeException(nameof(dataType), $"Unsupported sensor data type: {dataType}"),
        };
}
