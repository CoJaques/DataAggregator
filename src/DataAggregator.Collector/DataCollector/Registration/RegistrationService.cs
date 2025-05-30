using DataAggregator.Collector.DataCollector.Abstraction.Configuration;
using DataAggregator.Shared;
using DataAggregator.Shared.DTOs;
using Serilog;

namespace DataAggregator.Collector.DataCollector.Registration;

/// <summary>
/// Service for handling registration with the central registration service.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="RegistrationService"/> class.
/// </remarks>
/// <param name="httpClient">The HTTP client.</param>
/// <param name="registrationEndpoint">The registration service endpoint.</param>
public class RegistrationService(HttpClient httpClient, string registrationEndpoint)
{
    /// <summary>
    /// Registers a collector with the central registration service asynchronously.
    /// </summary>
    /// <param name="config">The collector configuration.</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation. The task result contains the registration response.</returns>
    public async Task<DeviceRegistrationResponse> RegisterCollectorAsync(CollectorConfiguration config)
    {
        try
        {
            Log.Information("Registering collector {DeviceId} with registration service", config.DeviceName);

            var sensorDtos = config.Sensors.Select(s => new SensorInfoDto(s.Name, s.Type, s.Unit, s.Metadata)).ToList();
            var request = new DeviceRegistrationRequest(config.DeviceName, config.Location, config.HealthCheckEndpoint, sensorDtos);

            HttpResponseMessage response = await httpClient.PostAsJsonAsync(registrationEndpoint, request);

            if (!response.IsSuccessStatusCode)
            {
                Log.Error("Failed to register collector. Status code: {StatusCode}", response.StatusCode);
                return new DeviceRegistrationResponse(false, string.Empty, string.Empty);
            }

            DeviceRegistrationResponse? result = await response.Content.ReadFromJsonAsync<DeviceRegistrationResponse>();
            if (result == null)
            {
                Log.Error("Failed to deserialize registration response");
                return new DeviceRegistrationResponse(false, string.Empty, string.Empty);
            }

            Log.Information("Collector registration result: {IsSuccess}", result.IsSuccess);
            return result;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error registering collector");
            return new DeviceRegistrationResponse(false, string.Empty, string.Empty);
        }
    }
}
