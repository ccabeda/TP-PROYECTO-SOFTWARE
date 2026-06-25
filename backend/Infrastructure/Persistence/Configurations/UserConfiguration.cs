using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TP_PROYECTO_SOFTWARE.Domain.Models;

namespace TP_PROYECTO_SOFTWARE.Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> entity)
    {
        entity.ToTable("USER");
        entity.Property(u => u.Name).IsRequired().HasMaxLength(100);
        entity.Property(u => u.Email).HasMaxLength(150);
        entity.Property(u => u.UserName).HasMaxLength(150);
        entity.Property(u => u.NormalizedEmail).HasMaxLength(150);
        entity.Property(u => u.NormalizedUserName).HasMaxLength(150);
        entity.Property(u => u.RefreshTokenHash).HasMaxLength(256);

        entity.HasIndex(u => u.Email).IsUnique();
        entity.HasIndex(u => u.RefreshTokenHash);

        entity.HasMany(u => u.Reservations)
            .WithOne(r => r.User)
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasMany(u => u.AuditLogs)
            .WithOne(a => a.User)
            .HasForeignKey(a => a.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
