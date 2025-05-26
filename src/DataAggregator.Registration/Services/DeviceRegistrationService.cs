using DataAggregator.Registration.Domain;
using DataAggregator.Registration.Repositories;
using DataAggregator.Shared;
using Microsoft.Extensions.Options;
using Serilog;

namespace DataAggregator.Registration.Services;

/// <summary>
/// Class for handling device registration operations.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="DeviceRegistrationService"/> class.
/// </remarks>
/// <param name="deviceRepository">The device repository.</param>
/// <param name="influxEndpoints">The list of InfluxDB endpoint configurations.</param>
public class DeviceRegistrationService(IDeviceRepository deviceRepository, IOptions<List<InfluxEndpointConfiguration>> influxEndpoints) : IDeviceRegistrationService
{
    private readonly IDeviceRepository _deviceRepository = deviceRepository;
    private readonly List<InfluxEndpointConfiguration> _influxEndpoints = influxEndpoints.Value;

    /// <inheritdoc/>
    public async Task<DeviceRegistrationResponse> RegisterCollectorAsync(DeviceRegistrationRequest request)
    {
        Log.Information("Registering a new collector: {DeviceName}", request.Config.DeviceName);

        if (await _deviceRepository.GetByNameAsync(request.Config.DeviceName) != null)
        {
            Log.Warning("Device already exists: {DeviceName}", request.Config.DeviceName);
            return new DeviceRegistrationResponse(false, string.Empty, string.Empty);
        }

        InfluxEndpointConfiguration defaultEndpoint = await GetAvailableInfluxEndpoint();

        var device = new Device
        {
            DeviceName = request.Config.DeviceName,
            Location = request.Config.Location,
            HealthCheckEndpoint = request.Config.HealthCheckEndpoint,
            RegistrationDate = DateTime.UtcNow,
            AssignedTimeSeriesEndpoint = defaultEndpoint.Endpoint,
        };

        device.Sensors = [.. request.Config.Sensors.Select(sensor => new Sensor
            {
                SensorName = sensor.SensorName,
                SensorType = sensor.Type,
                Unit = sensor.Unit,
                Metadata = sensor.Metadata,
                Device = device,
            })];

        await _deviceRepository.CreateAsync(device);

        Log.Information("Collector registered successfully: {DeviceName}", request.Config.DeviceName);
        return new DeviceRegistrationResponse(true, defaultEndpoint.Endpoint, defaultEndpoint.Token);
    }

    /// <inheritdoc/>
    public async Task<CollectorInfoDto?> GetCollectorInfoAsync(string collectorName)
    {
        Log.Information("Fetching collector info: {CollectorName}", collectorName);

        Device? device = await _deviceRepository.GetByNameAsync(collectorName);

        if (device is null)
        {
            Log.Warning("Collector not found: {CollectorName}", collectorName);
            return null;
        }

        var sensors = device.Sensors.Select(sensor => new SensorInfoDto(sensor.SensorName, sensor.SensorType, sensor.Unit, sensor.Metadata)).ToList();

        Log.Information("Collector info retrieved successfully: {CollectorName}", collectorName);
        return new CollectorInfoDto(
                device.DeviceName,
                device.Location,
                device.HealthCheckEndpoint,
                sensors);
    }

    /// <inheritdoc/>
    public async Task<bool> IsCollectorRegisteredAsync(string collectorName)
    {
        Log.Information("Checking if collector is registered: {CollectorName}", collectorName);
        return await _deviceRepository.GetByNameAsync(collectorName) != null;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<CollectorInfoDto>> GetAllCollectorInfoAsync()
    {
        Log.Information("Fetching all collector info.");

        IEnumerable<Device> devices = await _deviceRepository.GetAllAsync();

        Log.Information("All collector info retrieved successfully.");
        return devices.Select(device => new CollectorInfoDto(
            device.DeviceName,
            device.Location,
            device.HealthCheckEndpoint,
            device.Sensors.Select(sensor => new SensorInfoDto(
                sensor.SensorName,
                sensor.SensorType,
                sensor.Unit,
                sensor.Metadata))
            .ToList()))
        .ToList();
    }

    /// <summary>
    /// Gets the available InfluxDB endpoint configuration.
    /// This method can be enhanced to define a specific logic for selecting the endpoint.
    /// </summary>
    /// <returns>The default InfluxDB endpoint configuration.</returns>
    public Task<InfluxEndpointConfiguration> GetAvailableInfluxEndpoint()
    {
        InfluxEndpointConfiguration? defaultEndpoint = _influxEndpoints.FirstOrDefault(e => e.IsDefault);
        return defaultEndpoint == null
            ? throw new InvalidOperationException("No default InfluxDB endpoint is configured.")
            : Task.FromResult(defaultEndpoint);
    }
}
