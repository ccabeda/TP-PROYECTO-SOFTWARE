using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TP_PROYECTO_SOFTWARE.Domain.Models;

namespace TP_PROYECTO_SOFTWARE.Infraestructure.Persistence.Configurations;

public class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
{
    public void Configure(EntityTypeBuilder<AuditLog> entity)
    {
        entity.ToTable("AUDIT_LOG");
        entity.HasKey(a => a.Id);

        entity.Property(a => a.Action).IsRequired().HasMaxLength(100);
        entity.Property(a => a.EntityType).IsRequired().HasMaxLength(100);
        entity.Property(a => a.EntityId).IsRequired().HasMaxLength(100);
        entity.Property(a => a.Details).IsRequired().HasMaxLength(1000);
    }
}
