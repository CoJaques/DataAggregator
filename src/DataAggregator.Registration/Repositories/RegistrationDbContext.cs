using DataAggregator.Registration.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAggregator.Registration.Repositories;

/// <summary>
/// Database context for managing device and sensor entities.
/// </summary>
public class RegistrationDbContext : DbContext
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

        // Configure Device entity
        builder.Entity<Device>(entity =>
        {
            entity.HasKey(d => d.DeviceId);
            entity.Property(d => d.DeviceName).IsRequired().HasMaxLength(100);
            entity.Property(d => d.Location).HasMaxLength(200);
            entity.Property(d => d.HealthCheckEndpoint).HasMaxLength(200);
            entity.Property(d => d.AssignedTimeSeriesEndpoint).HasMaxLength(200);
            entity.HasMany(d => d.Sensors)
                  .WithOne(s => s.Device)
                  .HasForeignKey(s => s.DeviceId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // Configure Sensor entity
        builder.Entity<Sensor>(entity =>
        {
            entity.HasKey(s => s.SensorId);
            entity.Property(s => s.SensorName).IsRequired().HasMaxLength(100);
            entity.Property(s => s.SensorType).HasMaxLength(50);
            entity.Property(s => s.Unit).HasMaxLength(50);
        });
    }
}
