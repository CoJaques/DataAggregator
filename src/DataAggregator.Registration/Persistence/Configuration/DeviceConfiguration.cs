using DataAggregator.Registration.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAggregator.Registration.Persistence.Configuration;

/// <summary>
/// Configuration for the Device entity in the database.
/// </summary>
public class DeviceConfiguration : IEntityTypeConfiguration<Device>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<Device> builder)
    {
        builder.HasKey(d => d.Id);
        builder.Property(d => d.Id).HasColumnType("uuid");

        builder.Property(d => d.DeviceName).IsRequired().HasMaxLength(100);
        builder.Property(d => d.Location).HasMaxLength(200);
        builder.Property(d => d.HealthCheckEndpoint).HasMaxLength(200);
        builder.Property(d => d.RegistrationDate).IsRequired();
        builder.Property(d => d.AssignedTimeSeriesEndpoint).HasMaxLength(200);

        builder.HasMany(d => d.Sensors)
               .WithOne(s => s.Device)
               .IsRequired()
               .OnDelete(DeleteBehavior.Cascade);
    }
}
