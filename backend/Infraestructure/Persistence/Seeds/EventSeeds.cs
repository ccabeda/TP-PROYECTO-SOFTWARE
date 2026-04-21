using Microsoft.EntityFrameworkCore;
using TP_PROYECTO_SOFTWARE.Domain.Models;

namespace TP_PROYECTO_SOFTWARE.Infraestructure.Persistence.Seeds;

public static class EventSeeds
{
    public static void Seed(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EVENT>().HasData(
            new EVENT
            {
                Id = 1,
                Name = "Concierto de Rock",
                EventDate = new DateTime(2026, 7, 15, 21, 0, 0),
                Venue = "Estadio Central",
                Status = "Published"
            }
        );
    }
}
