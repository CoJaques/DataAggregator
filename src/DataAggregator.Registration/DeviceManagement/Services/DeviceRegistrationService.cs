using DataAggregator.Registration.DeviceManagement.Domain;
using DataAggregator.Registration.DeviceManagement.Domain.Extension;
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
    #region Interface implementation

    /// <inheritdoc/>
    public async Task<DeviceRegistrationResponse> RegisterCollectorAsync(DeviceRegistrationRequest request)
    {
        Log.Information("Registering or updating collector: {DeviceName}", request.DeviceName);

        Collector? existingDevice = await deviceRepository.GetByNameAsync(request.DeviceName);

        if (existingDevice != null)
        {
            return await HandleExistingDeviceAsync(existingDevice, request.DeviceName);
        }

        return await RegisterNewDeviceAsync(request);
    }

    /// <inheritdoc/>
    public async Task<CollectorInfoDto?> GetCollectorInfoAsync(string collectorName)
    {
        Log.Information("Fetching collector info: {CollectorName}", collectorName);

        Collector? device = await deviceRepository.GetByNameAsync(collectorName);

        if (device is null)
        {
            Log.Warning("Collector not found: {CollectorName}", collectorName);
            return null;
        }

        Log.Information("Collector info retrieved successfully: {CollectorName}", collectorName);
        return device.ToDto();
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
        IEnumerable<Collector> devices = await deviceRepository.GetAllAsync();
        return devices.Select(device => device.ToDto());
    }
    #endregion

    #region private methods

    private async Task<DeviceRegistrationResponse> HandleExistingDeviceAsync(Collector existingDevice, string deviceName)
    {
        Log.Information("Device already exists: {DeviceName}", deviceName);

        if (!await influxEndpointProviderService.CheckEndPointValidityAsync(existingDevice.AssignedInfluxEndpoint))
        {
            Log.Warning("Current endpoint is not functional for device: {DeviceName}", deviceName);

            existingDevice.EndpointHistories.Add(
                new EndpointHistory(existingDevice.AssignedInfluxEndpoint, existingDevice.RegistrationDate, DateTime.UtcNow));

            InfluxEndpoint newEndpoint = await influxEndpointProviderService.GetAvailableEndpointAsync();
            existingDevice.AssignedInfluxEndpoint = newEndpoint;
            existingDevice.RegistrationDate = DateTime.UtcNow;

            await deviceRepository.UpdateAsync(existingDevice);

            Log.Information("Assigned new endpoint to device: {DeviceName}", deviceName);
            return new DeviceRegistrationResponse(true, newEndpoint.Endpoint, newEndpoint.Token);
        }

        return new DeviceRegistrationResponse(false, existingDevice.AssignedInfluxEndpoint.Endpoint, existingDevice.AssignedInfluxEndpoint.Token);
    }

    private async Task<DeviceRegistrationResponse> RegisterNewDeviceAsync(DeviceRegistrationRequest request)
    {
        InfluxEndpoint defaultEndpoint = await influxEndpointProviderService.GetAvailableEndpointAsync();

        var device = new Collector
        {
            DeviceName = request.DeviceName,
            Location = request.Location,
            HealthCheckEndpoint = request.HealthCheckEndpoint,
            RegistrationDate = DateTime.UtcNow,
            AssignedInfluxEndpoint = defaultEndpoint,
            EndpointHistories = [],
        };

        IEnumerable<Sensor> sensors = request.Sensors.Select(sensor => new Sensor
        {
            SensorName = sensor.SensorName,
            SensorType = sensor.Type,
            Unit = sensor.Unit,
            Metadata = sensor.Metadata,
            Device = device,
        });

        device.Sensors = [.. sensors];

        await deviceRepository.CreateAsync(device);

        Log.Information("Collector registered successfully: {DeviceName}", request.DeviceName);
        return new DeviceRegistrationResponse(true, defaultEndpoint.Endpoint, defaultEndpoint.Token);
    }
    #endregion
}
