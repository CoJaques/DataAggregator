using DataAggregator.Registration.Entities;
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
        => await context.Devices.Include(d => d.Sensors).FirstOrDefaultAsync(d => d.DeviceName == deviceName);

    /// <inheritdoc/>
    public async Task<Device> CreateAsync(Device device)
    {
        context.Devices.Add(device);
        await context.SaveChangesAsync();
        return device;
    }

    /// <inheritdoc/>
    public async Task UpdateAsync(Device device)
    {
        context.Devices.Update(device);
        await context.SaveChangesAsync();
    }

    /// <inheritdoc/>
    public async Task<bool> ExistsAsync(string deviceId)
        => await context.Devices.AnyAsync(d => d.DeviceId == deviceId);

    /// <inheritdoc/>
    public async Task<IEnumerable<Device>> GetAllAsync()
        => await context.Devices.Include(d => d.Sensors).ToListAsync();
}
