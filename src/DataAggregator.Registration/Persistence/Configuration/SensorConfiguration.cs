using System.Text.Json;
using DataAggregator.Registration.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAggregator.Registration.Persistence.Configuration;

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

        builder.Property(s => s.Metadata)
            .HasConversion(
                v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                v => DeserializeMetadata(v))
            .HasColumnType("json"); // TODO -> Test with Jsonb
    }

    private static Dictionary<string, string> DeserializeMetadata(string json)
        => JsonSerializer.Deserialize<Dictionary<string, string>>(json) ?? [];
}
