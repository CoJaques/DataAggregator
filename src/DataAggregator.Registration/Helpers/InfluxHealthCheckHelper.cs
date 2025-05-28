using DataAggregator.Shared;
using Serilog;

namespace DataAggregator.Registration.Helpers;

/// <summary>
/// Helper class for checking the health of InfluxDB endpoints.
/// </summary>
public static class InfluxHealthCheckHelper
{
    /// <summary>
    /// Checks the health of the specified InfluxDB endpoint.
    /// </summary>
    /// <param name="endpoint">The InfluxDB endpoint configuration.</param>
    /// <returns>True if the endpoint is healthy, false otherwise.</returns>
    public static async Task<bool> CheckHealthAsync(InfluxEndpoint endpoint)
    {
        using var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {endpoint.Token}");

        try
        {
            HttpResponseMessage response = await httpClient.GetAsync($"{endpoint.Endpoint}/health");
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Log.Warning(ex, "Failed to check health for InfluxDB endpoint: {Endpoint}", endpoint.Endpoint);
            return false;
        }
    }
}
