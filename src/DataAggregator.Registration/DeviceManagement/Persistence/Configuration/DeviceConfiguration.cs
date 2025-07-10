using System.Text.Json;
using DataAggregator.Registration.DeviceManagement.Domain;
using DataAggregator.Shared.Configuration.TimeSeries;
using DataAggregator.Shared.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
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

        // Serialize AssignedInfluxEndpoint as JSON
        builder.Property(d => d.AssignedInfluxEndpoint)
               .HasConversion(
                   v => JsonSerializer.Serialize(v, _serializeOptions),
                   v => JsonSerializer.Deserialize<InfluxEndpoint>(v, _deserializeOptions)!)
               .HasColumnType("jsonb"); // Use JSONB for PostgreSQL

        // Serialize EndpointHistories as JSON with ValueComparer
        var endpointHistoriesComparer = new ValueComparer<List<EndpointHistory>>(
            (c1, c2) => c1!.SequenceEqual(c2!),
            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
            c => c.ToList());

        builder.Property(d => d.EndpointHistories)
               .HasConversion(
                   v => JsonSerializer.Serialize(v, _serializeOptions),
                   v => string.IsNullOrWhiteSpace(v)
                        ? new List<EndpointHistory>()
                        : JsonSerializer.Deserialize<List<EndpointHistory>>(v, _deserializeOptions) ?? new List<EndpointHistory>())
               .HasColumnType("jsonb")
               .Metadata.SetValueComparer(endpointHistoriesComparer);

        builder.HasMany(d => d.Sensors)
               .WithOne(s => s.Device)
               .IsRequired()
               .OnDelete(DeleteBehavior.Cascade);
    }

    private static readonly JsonSerializerOptions _serializeOptions = new()
    {
        WriteIndented = false,
    };

    private static readonly JsonSerializerOptions _deserializeOptions = new()
    {
        PropertyNameCaseInsensitive = true,
    };
}
