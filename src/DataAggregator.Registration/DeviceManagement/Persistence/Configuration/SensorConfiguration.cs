using System.Text.Json;
using DataAggregator.Registration.DeviceManagement.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAggregator.Registration.DeviceManagement.Persistence.Configuration;

/// <summary>
/// Configuration for the Sensor entity in the database.
/// </summary>
public class SensorConfiguration : IEntityTypeConfiguration<Sensor>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<Sensor> builder)
    {
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id).HasColumnType("uuid");

        builder.Property(s => s.SensorName).IsRequired().HasMaxLength(100);
        builder.Property(s => s.SensorType).HasMaxLength(50);
        builder.Property(s => s.Unit).HasMaxLength(50);

        // Configure SensorDataType as enum
        builder.Property(s => s.DataType)
            .HasConversion<int>()
            .HasDefaultValue(DataAggregator.Shared.Domain.DataType.SensorDataType.Float)
            .HasSentinel(DataAggregator.Shared.Domain.DataType.SensorDataType.Undefined);

        // Serialize Metadata as JSON with ValueComparer
        var metadataComparer = new ValueComparer<Dictionary<string, string>>(
            (d1, d2) => d1!.SequenceEqual(d2!),
            d => d.Aggregate(0, (a, v) => HashCode.Combine(a, v.Key.GetHashCode(), v.Value.GetHashCode())),
            d => d.ToDictionary(entry => entry.Key, entry => entry.Value));

        builder.Property(s => s.Metadata)
            .HasConversion(
                v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                v => DeserializeMetadata(v))
            .HasColumnType("json")
            .Metadata.SetValueComparer(metadataComparer);
    }

    private static Dictionary<string, string> DeserializeMetadata(string json)
        => JsonSerializer.Deserialize<Dictionary<string, string>>(json) ?? [];
}
