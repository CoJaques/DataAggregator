using DataAggregator.Registration.DeviceManagement.Domain;
using DataAggregator.Registration.DeviceManagement.Persistence.Exceptions;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace DataAggregator.Registration.DeviceManagement.Persistence.Repositories;

/// <summary>
/// Repository for managing device-related database operations.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="DeviceRepository"/> class.
/// </remarks>
/// <param name="context">The database context for device operations.</param>
public class DeviceRepository(RegistrationDbContext context) : IDeviceRepository
{
    /// <inheritdoc/>
    public async Task<Collector?> GetByNameAsync(string deviceName)
    {
        try
        {
            Log.Information("Fetching device by name: {DeviceName}", deviceName);
            return await context.Devices.Include(d => d.Sensors).FirstOrDefaultAsync(d => d.DeviceName == deviceName);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Failed to retrieve device by name: {DeviceName}", deviceName);
            throw new DatabaseAccessException("Failed to retrieve device by name.", ex);
        }
    }

    /// <inheritdoc/>
    public async Task<Collector> CreateAsync(Collector device)
    {
        try
        {
            Log.Information("Creating a new device: {DeviceName}", device.DeviceName);
            context.Devices.Add(device);
            await context.SaveChangesAsync();
            Log.Information("Device created successfully: {DeviceName}", device.DeviceName);
            return device;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Failed to create device: {DeviceName}", device.DeviceName);
            throw new DatabaseAccessException("Failed to create device.", ex);
        }
    }

    /// <inheritdoc/>
    public async Task UpdateAsync(Collector device)
    {
        try
        {
            Log.Information("Updating device: {DeviceName}", device.DeviceName);
            context.Devices.Update(device);
            await context.SaveChangesAsync();
            Log.Information("Device updated successfully: {DeviceName}", device.DeviceName);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Failed to update device: {DeviceName}", device.DeviceName);
            throw new DatabaseAccessException("Failed to update device.", ex);
        }
    }

    /// <inheritdoc/>
    public async Task<bool> ExistsAsync(string deviceName)
    {
        try
        {
            Log.Information("Checking if device exists: {DeviceId}", deviceName);
            return await context.Devices.AnyAsync(d => d.DeviceName == deviceName);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Failed to check if device exists: {DeviceId}", deviceName);
            throw new DatabaseAccessException("Failed to check if device exists.", ex);
        }
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Collector>> GetAllAsync()
    {
        try
        {
            Log.Information("Fetching all devices.");
            return await context.Devices.Include(d => d.Sensors).ToListAsync();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Failed to retrieve all devices.");
            throw new DatabaseAccessException("Failed to retrieve all devices.", ex);
        }
    }
}
