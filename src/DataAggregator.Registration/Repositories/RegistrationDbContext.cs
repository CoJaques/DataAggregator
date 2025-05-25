using DataAggregator.Registration.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAggregator.Registration.Repositories;

/// <summary>
/// Database context for managing device and sensor entities.
/// </summary>
public class RegistrationDbContext(DbContextOptions<RegistrationDbContext> options) : DbContext(options)
{
    /// <summary>
    /// Gets or sets the DbSet for devices.
    /// </summary>
    public DbSet<Device> Devices { get; set; } = null!;

    /// <summary>
    /// Gets or sets the DbSet for sensors.
    /// </summary>
    public DbSet<Sensor> Sensors { get; set; } = null!;

    /// <inheritdoc/>
    /// <summary>
    /// Configures the entity relationships and constraints.
    /// </summary>
    /// <param name="builder">The model builder used to configure the entities.</param>
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Configure relationships for Device entity
        builder.Entity<Device>(entity =>
        {
            entity.HasKey(d => d.DeviceId);
            entity.HasMany(d => d.Sensors)
                  .WithOne(s => s.Device)
                  .HasForeignKey(s => s.DeviceId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // Configure relationships for Sensor entity
        builder.Entity<Sensor>(entity => entity.HasKey(s => s.SensorId));
    }
}
