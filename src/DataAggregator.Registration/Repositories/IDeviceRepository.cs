using DataAggregator.Registration.Domain;

namespace DataAggregator.Registration.Repositories;

/// <summary>
/// Interface defining the contract for device repository operations.
/// </summary>
public interface IDeviceRepository
{
    /// <summary>
    /// Retrieves a device by its name asynchronously.
    /// </summary>
    /// <param name="deviceName">The name of the device.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the device.</returns>
    public Task<Device?> GetByNameAsync(string deviceName);

    /// <summary>
    /// Creates a new device entry in the store asynchronously.
    /// </summary>
    /// <param name="device">The device to create.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the created device.</returns>
    public Task<Device> CreateAsync(Device device);

    /// <summary>
    /// Updates an existing device entry in the store asynchronously.
    /// </summary>
    /// <param name="device">The device to update.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public Task UpdateAsync(Device device);

    /// <summary>
    /// Checks if a device exists in the store asynchronously.
    /// </summary>
    /// <param name="deviceId">The unique identifier of the device.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains a boolean indicating whether the device exists.</returns>
    public Task<bool> ExistsAsync(string deviceId);

    /// <summary>
    /// Retrieves all devices from the store asynchronously.
    /// </summary>
    /// <returns>A task representing the asynchronous operation. The task result contains a collection of devices.</returns>
    public Task<IEnumerable<Device>> GetAllAsync();
}
