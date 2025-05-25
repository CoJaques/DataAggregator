using DataAggregator.Registration.Entities;

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
    public Task<Device> GetByNameAsync(string deviceId) => throw new NotImplementedException();

    /// <inheritdoc/>
    public Task<Device> CreateAsync(Device device) => throw new NotImplementedException();

    /// <inheritdoc/>
    public Task UpdateAsync(Device device) => throw new NotImplementedException();

    /// <inheritdoc/>
    public Task<bool> ExistsAsync(string deviceId) => throw new NotImplementedException();

    /// <inheritdoc/>
    public Task<IEnumerable<Device>> GetAllAsync() => throw new NotImplementedException();
}
