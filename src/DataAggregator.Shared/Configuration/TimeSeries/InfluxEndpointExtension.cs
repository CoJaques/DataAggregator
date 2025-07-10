namespace DataAggregator.Shared.Configuration.TimeSeries;

/// <summary>
/// Extension methods for InfluxDB endpoint configuration.
/// </summary>
public static class InfluxEndpointExtension
{
    /// <summary>
    /// Check if the InfluxDB endpoint is valid by checking its health status.
    /// </summary>
    /// <param name="endpoint">The endpoint to check.</param>
    /// <returns>True if healthy false otherwise.</returns>
    public static async Task<bool> CheckEndPointValidityAsync(this InfluxEndpoint endpoint)
    {
        using var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {endpoint.Token}");

        try
        {
            HttpResponseMessage response = await httpClient.GetAsync($"{endpoint.Endpoint}/health");
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }
}
