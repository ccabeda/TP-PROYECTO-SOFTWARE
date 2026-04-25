using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using TP_PROYECTO_SOFTWARE.Domain.Models;
using TP_PROYECTO_SOFTWARE.Infraestructure.Persistence.Seeds;

namespace TP_PROYECTO_SOFTWARE.Infraestructure.Persistence;

public class AplicationDbContext : IdentityDbContext<User, IdentityRole<int>, int>
{
    public AplicationDbContext(DbContextOptions<AplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Event> Events { get; set; }
    public DbSet<Sector> Sectors { get; set; }
    public DbSet<Seat> Seats { get; set; }
    public DbSet<Reservation> Reservations { get; set; }
    public DbSet<AuditLog> AuditLogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        EventSeeds.Seed(modelBuilder);
        SectorSeeds.Seed(modelBuilder);
        SeatSeeds.Seed(modelBuilder);

        modelBuilder.Entity<IdentityRole<int>>().ToTable("IDENTITY_ROLE");
        modelBuilder.Entity<IdentityUserRole<int>>().ToTable("IDENTITY_USER_ROLE");
        modelBuilder.Entity<IdentityUserClaim<int>>().ToTable("IDENTITY_USER_CLAIM");
        modelBuilder.Entity<IdentityUserLogin<int>>().ToTable("IDENTITY_USER_LOGIN");
        modelBuilder.Entity<IdentityRoleClaim<int>>().ToTable("IDENTITY_ROLE_CLAIM");
        modelBuilder.Entity<IdentityUserToken<int>>().ToTable("IDENTITY_USER_TOKEN");

        modelBuilder.Entity<Event>(entity =>
        {
            entity.ToTable("EVENT");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Name).IsRequired().HasMaxLength(150);
            entity.Property(e => e.Venue).IsRequired().HasMaxLength(150);
            entity.Property(e => e.Status).IsRequired().HasMaxLength(50);

            entity.HasMany(e => e.Sectors)
                .WithOne(s => s.Event)
                .HasForeignKey(s => s.EventId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Sector>(entity =>
        {
            entity.ToTable("SECTOR");
            entity.HasKey(s => s.Id);

            entity.Property(s => s.Name).IsRequired().HasMaxLength(100);
            entity.Property(s => s.Price).HasColumnType("decimal(18,2)");

            entity.HasMany(s => s.Seats)
                .WithOne(seat => seat.Sector)
                .HasForeignKey(seat => seat.SectorId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Seat>(entity =>
        {
            entity.ToTable("SEAT");
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

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("USER");
            entity.Property(u => u.Name).IsRequired().HasMaxLength(100);
            entity.Property(u => u.Email).HasMaxLength(150);
            entity.Property(u => u.UserName).HasMaxLength(150);
            entity.Property(u => u.NormalizedEmail).HasMaxLength(150);
            entity.Property(u => u.NormalizedUserName).HasMaxLength(150);

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

        modelBuilder.Entity<Reservation>(entity =>
        {
            entity.ToTable("RESERVATION");
            entity.HasKey(r => r.Id);

            entity.Property(r => r.Status).IsRequired().HasMaxLength(50);
        });

        modelBuilder.Entity<AuditLog>(entity =>
        {
            entity.ToTable("AUDIT_LOG");
            entity.HasKey(a => a.Id);

            entity.Property(a => a.Action).IsRequired().HasMaxLength(100);
            entity.Property(a => a.EntityType).IsRequired().HasMaxLength(100);
            entity.Property(a => a.EntityId).IsRequired().HasMaxLength(100);
            entity.Property(a => a.Details).IsRequired().HasMaxLength(1000);
        });
    }
}


