using DataAggregator.Shared.DTOs;
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
    public async Task<CollectorInfoDto?> GetCollectorInfoAsync(string deviceName)
    {
        try
        {
            HttpResponseMessage response = await httpClient.GetAsync($"/api/DeviceRegistration/collector/{deviceName}");

            if (response.IsSuccessStatusCode)
            {
                CollectorInfoDto? collectorInfo = await response.Content.ReadFromJsonAsync<CollectorInfoDto>();
                if (collectorInfo != null)
                {
                    Log.Debug(
                        "Retrieved collector info for {DeviceName}: {Endpoint}",
                        deviceName,
                        collectorInfo.AssignedInfluxEndpoint.Endpoint);

                    return collectorInfo;
                }
            }

            Log.Warning("Failed to retrieve collector info for {DeviceName}. Status: {StatusCode}", deviceName, response.StatusCode);
            return null;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error retrieving collector info for {DeviceName}", deviceName);
            throw;
        }
    }
}
