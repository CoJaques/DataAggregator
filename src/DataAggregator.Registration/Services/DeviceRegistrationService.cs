using DataAggregator.Registration.Configuration;
using DataAggregator.Registration.Domain;
using DataAggregator.Registration.Helpers;
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
public class DeviceRegistrationService(IDeviceRepository deviceRepository, IOptions<InfluxEndpointsConfiguration> influxEndpoints) : IDeviceRegistrationService
{
    private readonly InfluxEndpointsConfiguration _influxEndpoints = influxEndpoints.Value;

    /// <inheritdoc/>
    public async Task<DeviceRegistrationResponse> RegisterCollectorAsync(DeviceRegistrationRequest request)
    {
        Log.Information("Registering or updating collector: {DeviceName}", request.Config.DeviceName);

        Device? existingDevice = await deviceRepository.GetByNameAsync(request.Config.DeviceName);

        if (existingDevice != null)
        {
            Log.Information("Device already exists: {DeviceName}", existingDevice.DeviceName);

            if (!await InfluxHealthCheckHelper.CheckHealthAsync(existingDevice.AssignedInfluxEndpoint))
            {
                Log.Warning("Current endpoint is not functional for device: {DeviceName}", request.Config.DeviceName);

                // Update endpoint history
                existingDevice.EndpointHistories.Add(
                    new EndpointHistory(existingDevice.AssignedInfluxEndpoint, existingDevice.RegistrationDate, DateTime.UtcNow));

                // Assign a new endpoint
                InfluxEndpoint newEndpoint = await GetAvailableInfluxEndpoint();
                existingDevice.AssignedInfluxEndpoint = newEndpoint;
                existingDevice.RegistrationDate = DateTime.UtcNow;

                await deviceRepository.UpdateAsync(existingDevice);

                Log.Information("Assigned new endpoint to device: {DeviceName}", request.Config.DeviceName);
                return new DeviceRegistrationResponse(true, newEndpoint.Endpoint, newEndpoint.Token);
            }

            return new DeviceRegistrationResponse(false, existingDevice.AssignedInfluxEndpoint.Endpoint, existingDevice.AssignedInfluxEndpoint.Token);
        }

        // Register new device logic...
        InfluxEndpoint defaultEndpoint = await GetAvailableInfluxEndpoint();

        var device = new Device
        {
            DeviceName = request.Config.DeviceName,
            Location = request.Config.Location,
            HealthCheckEndpoint = request.Config.HealthCheckEndpoint,
            RegistrationDate = DateTime.UtcNow,
            AssignedInfluxEndpoint = defaultEndpoint,
            EndpointHistories = [],
        };

        IEnumerable<Sensor> sensors = request.Config.Sensors.Select(sensor => new Sensor
        {
            SensorName = sensor.SensorName,
            SensorType = sensor.Type,
            Unit = sensor.Unit,
            Metadata = sensor.Metadata,
            Device = device,
        });

        device.Sensors = sensors.ToList();

        await deviceRepository.CreateAsync(device);

        Log.Information("Collector registered successfully: {DeviceName}", request.Config.DeviceName);
        return new DeviceRegistrationResponse(true, defaultEndpoint.Endpoint, defaultEndpoint.Token);
    }

    /// <inheritdoc/>
    public async Task<CollectorInfoDto?> GetCollectorInfoAsync(string collectorName)
    {
        Log.Information("Fetching collector info: {CollectorName}", collectorName);

        Device? device = await deviceRepository.GetByNameAsync(collectorName);

        if (device is null)
        {
            Log.Warning("Collector not found: {CollectorName}", collectorName);
            return null;
        }

        var sensors = device.Sensors.Select(
            sensor =>
            new SensorInfoDto(
                sensor.SensorName,
                sensor.SensorType,
                sensor.Unit,
                sensor.Metadata)).ToList();

        Log.Information("Collector info retrieved successfully: {CollectorName}", collectorName);
        return new CollectorInfoDto(
            device.DeviceName,
            device.Location,
            device.HealthCheckEndpoint,
            sensors,
            device.EndpointHistories);
    }

    /// <inheritdoc/>
    public async Task<bool> IsCollectorRegisteredAsync(string collectorName)
    {
        Log.Information("Checking if collector is registered: {CollectorName}", collectorName);
        return await deviceRepository.GetByNameAsync(collectorName) != null;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<CollectorInfoDto>> GetAllCollectorInfoAsync()
    {
        Log.Information("Fetching all collector info.");

        IEnumerable<Device> devices = await deviceRepository.GetAllAsync();

        Log.Information("All collector info retrieved successfully.");
        return devices.Select(device => new CollectorInfoDto(
            device.DeviceName,
            device.Location,
            device.HealthCheckEndpoint,
            [.. device.Sensors.Select(sensor => new SensorInfoDto(
                sensor.SensorName,
                sensor.SensorType,
                sensor.Unit,
                sensor.Metadata))],
            device.EndpointHistories));
    }

    /// <summary>
    /// Gets the available InfluxDB endpoint configuration.
    /// This method uses the InfluxHealthCheckHelper to check the health of endpoints.
    /// </summary>
    /// <returns>The healthy InfluxDB endpoint configuration.</returns>
    public async Task<InfluxEndpoint> GetAvailableInfluxEndpoint()
    {
        foreach (InfluxEndpoint endpoint in _influxEndpoints.Endpoints)
        {
            if (await InfluxHealthCheckHelper.CheckHealthAsync(endpoint))
            {
                Log.Information("Selected healthy InfluxDB endpoint: {Endpoint}", endpoint.Endpoint);
                return endpoint;
            }
            else
            {
                Log.Warning("InfluxDB endpoint is not healthy: {Endpoint}", endpoint.Endpoint);
            }
        }

        throw new InvalidOperationException("No healthy InfluxDB endpoint is available.");
    }
}
