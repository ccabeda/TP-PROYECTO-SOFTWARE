using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TP_PROYECTO_SOFTWARE.Domain.Models;

namespace TP_PROYECTO_SOFTWARE.Infraestructure.Persistence.Configurations;

public class SeatConfiguration : IEntityTypeConfiguration<Seat>
{
    public void Configure(EntityTypeBuilder<Seat> entity)
    {
        entity.ToTable("SEAT");
        entity.HasKey(s => s.Id);

        entity.Property(s => s.RowIdentifier).IsRequired().HasMaxLength(10);
        entity.Property(s => s.Status).IsRequired().HasMaxLength(50);
        entity.Property(s => s.Version);

        entity.HasIndex(s => new { s.SectorId, s.RowIdentifier, s.SeatNumber }).IsUnique();

        entity.HasMany(s => s.Reservations)
            .WithOne(r => r.Seat)
            .HasForeignKey(r => r.SeatId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
