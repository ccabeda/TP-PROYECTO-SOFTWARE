using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TP_PROYECTO_SOFTWARE.Domain.Models;

namespace TP_PROYECTO_SOFTWARE.Infraestructure.Persistence.Configurations;

public class EventConfiguration : IEntityTypeConfiguration<Event>
{
    public void Configure(EntityTypeBuilder<Event> entity)
    {
        entity.ToTable("EVENT");
        entity.HasKey(e => e.Id);

        entity.Property(e => e.Name).IsRequired().HasMaxLength(150);
        entity.Property(e => e.Venue).IsRequired().HasMaxLength(150);
        entity.Property(e => e.Status).IsRequired().HasMaxLength(50);
        entity.Property(e => e.ImageUrl).HasMaxLength(500);
        entity.Property(e => e.Description).HasMaxLength(2000);

        entity.HasMany(e => e.Sectors)
            .WithOne(s => s.Event)
            .HasForeignKey(s => s.EventId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
