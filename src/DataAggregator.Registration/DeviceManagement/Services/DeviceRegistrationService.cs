using DataAggregator.Registration.DeviceManagement.Domain;
using DataAggregator.Registration.DeviceManagement.Persistence.Repositories;
using DataAggregator.Registration.Domain;
using DataAggregator.Registration.InfluxService.Services;
using DataAggregator.Shared;
using Serilog;

namespace DataAggregator.Registration.DeviceManagement.Services;

/// <summary>
/// Class for handling device registration operations.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="DeviceRegistrationService"/> class.
/// </remarks>
/// <param name="deviceRepository">The device repository.</param>
/// <param name="influxEndpointProviderService">The InfluxDB endpoint provider service.</param>
public class DeviceRegistrationService(IDeviceRepository deviceRepository, IInfluxEndpointProviderService influxEndpointProviderService) : IDeviceRegistrationService
{
    private readonly IDeviceRepository _deviceRepository = deviceRepository;
    private readonly IInfluxEndpointProviderService _influxEndpointProviderService = influxEndpointProviderService;

    /// <inheritdoc/>
    public async Task<DeviceRegistrationResponse> RegisterCollectorAsync(DeviceRegistrationRequest request)
    {
        Log.Information("Registering or updating collector: {DeviceName}", request.Config.DeviceName);

        Device? existingDevice = await _deviceRepository.GetByNameAsync(request.Config.DeviceName);

        if (existingDevice != null)
        {
            Log.Information("Device already exists: {DeviceName}", existingDevice.DeviceName);

            if (!await _influxEndpointProviderService.CheckEndPointValidityAsync(existingDevice.AssignedInfluxEndpoint))
            {
                Log.Warning("Current endpoint is not functional for device: {DeviceName}", request.Config.DeviceName);

                // Update endpoint history
                existingDevice.EndpointHistories.Add(
                    new EndpointHistory(existingDevice.AssignedInfluxEndpoint, existingDevice.RegistrationDate, DateTime.UtcNow));

                // Assign a new endpoint
                InfluxEndpoint newEndpoint = await _influxEndpointProviderService.GetAvailableEndpointAsync();
                existingDevice.AssignedInfluxEndpoint = newEndpoint;
                existingDevice.RegistrationDate = DateTime.UtcNow;

                await _deviceRepository.UpdateAsync(existingDevice);

                Log.Information("Assigned new endpoint to device: {DeviceName}", request.Config.DeviceName);
                return new DeviceRegistrationResponse(true, newEndpoint.Endpoint, newEndpoint.Token);
            }

            return new DeviceRegistrationResponse(false, existingDevice.AssignedInfluxEndpoint.Endpoint, existingDevice.AssignedInfluxEndpoint.Token);
        }

        // Register new device logic...
        InfluxEndpoint defaultEndpoint = await _influxEndpointProviderService.GetAvailableEndpointAsync();

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
            [.. device.Sensors.Select(sensor => new SensorInfoDto(
                sensor.SensorName,
                sensor.SensorType,
                sensor.Unit,
                sensor.Metadata))],
            device.EndpointHistories));
    }
}
