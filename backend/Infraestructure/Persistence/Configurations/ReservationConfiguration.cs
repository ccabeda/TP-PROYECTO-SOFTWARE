using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TP_PROYECTO_SOFTWARE.Domain.Models;

namespace TP_PROYECTO_SOFTWARE.Infraestructure.Persistence.Configurations;

public class ReservationConfiguration : IEntityTypeConfiguration<Reservation>
{
    public void Configure(EntityTypeBuilder<Reservation> entity)
    {
        entity.ToTable("RESERVATION");
        entity.HasKey(r => r.Id);

        entity.Property(r => r.Status).IsRequired().HasMaxLength(50);
    }
}
