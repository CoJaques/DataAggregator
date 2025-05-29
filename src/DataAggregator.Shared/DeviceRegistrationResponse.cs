namespace DataAggregator.Shared;

/// <summary>
/// This record represents the response from a device registration request.
/// </summary>
/// <param name="IsSuccess">Boolean which is true when success, false otherwise. Could be false if device is already registered.</param>
/// <param name="AssignedTimeSeriesEndpoint">The endpoint of the timeseries DB where the device must store data.</param>
/// <param name="DeviceToken">A token if needed (for example, to register to the time series db.)</param>
public record DeviceRegistrationResponse(bool IsSuccess, string AssignedTimeSeriesEndpoint, string DeviceToken);
