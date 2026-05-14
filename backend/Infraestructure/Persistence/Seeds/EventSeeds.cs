using Microsoft.EntityFrameworkCore;
using TP_PROYECTO_SOFTWARE.Domain.Models;

namespace TP_PROYECTO_SOFTWARE.Infraestructure.Persistence.Seeds;

public static class EventSeeds
{
    public static IReadOnlyList<Event> GetSeedData() =>
        new[]
        {
            new Event
            {
                Id = 1,
                Name = "Noches en Vivo 2026",
                EventDate = new DateTime(2026, 7, 15, 21, 0, 0),
                Venue = "Microestadio UNAJ",
                Status = "Published"
            }
        };

    public static void Seed(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Event>().HasData(GetSeedData());
    }
}
