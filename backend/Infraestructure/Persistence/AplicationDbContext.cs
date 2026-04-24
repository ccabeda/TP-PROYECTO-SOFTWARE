using Microsoft.EntityFrameworkCore;
using TP_PROYECTO_SOFTWARE.Domain.Models;
using TP_PROYECTO_SOFTWARE.Infraestructure.Persistence.Seeds;

namespace TP_PROYECTO_SOFTWARE.Infraestructure.Persistence;

public class AplicationDbContext : DbContext
{
    public AplicationDbContext(DbContextOptions<AplicationDbContext> options) : base(options)
    {
    }

    public DbSet<EVENT> EVENT { get; set; }
    public DbSet<SECTOR> SECTOR { get; set; }
    public DbSet<SEAT> SEAT { get; set; }
    public DbSet<USER> USER { get; set; }
    public DbSet<RESERVATION> RESERVATION { get; set; }
    public DbSet<AUDIT_LOG> AUDIT_LOG { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        EventSeeds.Seed(modelBuilder);
        SectorSeeds.Seed(modelBuilder);
        SeatSeeds.Seed(modelBuilder);

        modelBuilder.Entity<EVENT>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Name).IsRequired().HasMaxLength(150);
            entity.Property(e => e.Venue).IsRequired().HasMaxLength(150);
            entity.Property(e => e.Status).IsRequired().HasMaxLength(50);

            entity.HasMany(e => e.Sectors)
                .WithOne(s => s.Event)
                .HasForeignKey(s => s.EventId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<SECTOR>(entity =>
        {
            entity.HasKey(s => s.Id);

            entity.Property(s => s.Name).IsRequired().HasMaxLength(100);
            entity.Property(s => s.Price).HasColumnType("decimal(18,2)");

            entity.HasMany(s => s.Seats)
                .WithOne(seat => seat.Sector)
                .HasForeignKey(seat => seat.SectorId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<SEAT>(entity =>
        {
            entity.HasKey(s => s.Id);

            entity.Property(s => s.RowIdentifier).IsRequired().HasMaxLength(10);
            entity.Property(s => s.Status).IsRequired().HasMaxLength(50);
            entity.Property(s => s.Version); //poner concurrence token para luego manejar concurrencia (entrega 2)

            entity.HasIndex(s => new { s.SectorId, s.RowIdentifier, s.SeatNumber }).IsUnique();

            entity.HasMany(s => s.Reservations)
                .WithOne(r => r.Seat)
                .HasForeignKey(r => r.SeatId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<USER>(entity =>
        {
            entity.HasKey(u => u.Id);

            entity.Property(u => u.Name).IsRequired().HasMaxLength(100);
            entity.Property(u => u.Email).IsRequired().HasMaxLength(150);
            entity.Property(u => u.PasswordHash).IsRequired().HasMaxLength(255);

            entity.HasIndex(u => u.Email).IsUnique();

            entity.HasMany(u => u.Reservations)
                .WithOne(r => r.User)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasMany(u => u.AuditLogs)
                .WithOne(a => a.User)
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<RESERVATION>(entity =>
        {
            entity.HasKey(r => r.Id);

            entity.Property(r => r.Status).IsRequired().HasMaxLength(50);
        });

        modelBuilder.Entity<AUDIT_LOG>(entity =>
        {
            entity.HasKey(a => a.Id);

            entity.Property(a => a.Action).IsRequired().HasMaxLength(100);
            entity.Property(a => a.EntityType).IsRequired().HasMaxLength(100);
            entity.Property(a => a.EntityId).IsRequired().HasMaxLength(100);
            entity.Property(a => a.Details).IsRequired().HasMaxLength(1000);
        });
    }
}
