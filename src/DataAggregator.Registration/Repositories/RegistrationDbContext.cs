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
    protected override void OnModelCreating(ModelBuilder builder) => base.OnModelCreating(builder); // Configure entity relationships and constraints here
}
