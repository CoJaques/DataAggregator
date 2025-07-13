using DataAggregator.Shared;
using Serilog;

namespace DataAggregator.Processor.Services.Registration;

/// <summary>
/// Implementation of the registration service client.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="RegistrationServiceClient"/> class.
/// </remarks>
/// <param name="httpClient">The HTTP client.</param>
public class RegistrationServiceClient(HttpClient httpClient) : IRegistrationServiceClient
{
    /// <inheritdoc/>
    public async Task<DeviceRegistrationResponse> GetDeviceInfoAsync(string deviceName)
    {
        try
        {
            HttpResponseMessage response = await httpClient.GetAsync($"/api/DeviceRegistration/device/{deviceName}");

            if (response.IsSuccessStatusCode)
            {
                DeviceRegistrationResponse? deviceInfo = await response.Content.ReadFromJsonAsync<DeviceRegistrationResponse>();
                if (deviceInfo != null)
                {
                    Log.Debug(
                        "Retrieved device info for {DeviceName}: {Endpoint}",
                        deviceName,
                        deviceInfo.AssignedTimeSeriesEndpoint);

                    return deviceInfo;
                }
            }

            Log.Warning("Failed to retrieve device info for {DeviceName}. Status: {StatusCode}", deviceName, response.StatusCode);
            throw new InvalidOperationException($"Failed to retrieve device info for {deviceName}. Status: {response.StatusCode}");
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error retrieving device info for {DeviceName}", deviceName);
            throw;
        }
    }
}
