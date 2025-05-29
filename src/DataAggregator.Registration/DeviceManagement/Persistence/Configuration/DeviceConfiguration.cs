using DataAggregator.Registration.DeviceManagement.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAggregator.Registration.DeviceManagement.Persistence.Configuration;

/// <summary>
/// Configuration for the Device entity in the database.
/// </summary>
public class DeviceConfiguration : IEntityTypeConfiguration<Collector>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<Collector> builder)
    {
        builder.HasKey(d => d.Id);
        builder.Property(d => d.Id).HasColumnType("uuid");

        builder.Property(d => d.DeviceName).IsRequired().HasMaxLength(100);
        builder.Property(d => d.Location).HasMaxLength(200);
        builder.Property(d => d.HealthCheckEndpoint).HasMaxLength(200);
        builder.Property(d => d.RegistrationDate).IsRequired();
        builder.Property(d => d.AssignedInfluxEndpoint).HasMaxLength(200);

        builder.HasMany(d => d.Sensors)
               .WithOne(s => s.Device)
               .IsRequired()
               .OnDelete(DeleteBehavior.Cascade);
    }
}
