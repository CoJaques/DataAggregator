using DataAggregator.Registration.DeviceManagement.Domain;
using DataAggregator.Registration.DeviceManagement.Persistence.Configuration;
using Microsoft.EntityFrameworkCore;

namespace DataAggregator.Registration.DeviceManagement.Persistence;

/// <summary>
/// Database context for managing device and sensor entities.
/// </summary>
public class RegistrationDbContext(DbContextOptions<RegistrationDbContext> options) : DbContext(options)
{
    /// <summary>
    /// Gets or sets the collection of devices in the database.
    /// </summary>
    public DbSet<Device> Devices { get; set; } = null!;

    /// <summary>
    /// Gets or sets the collection of sensors in the database.
    /// </summary>
    public DbSet<Sensor> Sensors { get; set; } = null!;

    /// <inheritdoc/>
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfiguration(new DeviceConfiguration());
        builder.ApplyConfiguration(new SensorConfiguration());
    }
}
