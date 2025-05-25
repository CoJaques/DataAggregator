using DataAggregator.Registration.Entities;
using DataAggregator.Registration.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace DataAggregator.Registration.Repositories;

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
    public async Task<Device?> GetByNameAsync(string deviceName)
    {
        try
        {
            return await context.Devices.Include(d => d.Sensors).FirstOrDefaultAsync(d => d.DeviceName == deviceName);
        }
        catch (Exception ex)
        {
            // TODO: Add logging for database access error
            throw new DatabaseAccessException("Failed to retrieve device by name.", ex);
        }
    }

    /// <inheritdoc/>
    public async Task<Device> CreateAsync(Device device)
    {
        try
        {
            context.Devices.Add(device);
            await context.SaveChangesAsync();
            return device;
        }
        catch (Exception ex)
        {
            // TODO: Add logging for database access error
            throw new DatabaseAccessException("Failed to create device.", ex);
        }
    }

    /// <inheritdoc/>
    public async Task UpdateAsync(Device device)
    {
        try
        {
            context.Devices.Update(device);
            await context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            // TODO: Add logging for database access error
            throw new DatabaseAccessException("Failed to update device.", ex);
        }
    }

    /// <inheritdoc/>
    public async Task<bool> ExistsAsync(string deviceId)
    {
        try
        {
            return await context.Devices.AnyAsync(d => d.DeviceId == deviceId);
        }
        catch (Exception ex)
        {
            // TODO: Add logging for database access error
            throw new DatabaseAccessException("Failed to check if device exists.", ex);
        }
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Device>> GetAllAsync()
    {
        try
        {
            return await context.Devices.Include(d => d.Sensors).ToListAsync();
        }
        catch (Exception ex)
        {
            // TODO: Add logging for database access error
            throw new DatabaseAccessException("Failed to retrieve all devices.", ex);
        }
    }
}
