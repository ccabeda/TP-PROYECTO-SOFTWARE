using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TP_PROYECTO_SOFTWARE.Domain.Models;

namespace TP_PROYECTO_SOFTWARE.Infraestructure.Persistence.Configurations;

public class SectorConfiguration : IEntityTypeConfiguration<Sector>
{
    public void Configure(EntityTypeBuilder<Sector> entity)
    {
        entity.ToTable("SECTOR");
        entity.HasKey(s => s.Id);

        entity.Property(s => s.Name).IsRequired().HasMaxLength(100);
        entity.Property(s => s.Price).HasColumnType("decimal(18,2)");

        entity.HasMany(s => s.Seats)
            .WithOne(seat => seat.Sector)
            .HasForeignKey(seat => seat.SectorId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
