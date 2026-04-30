using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
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

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AplicationDbContext).Assembly);
    }
}
