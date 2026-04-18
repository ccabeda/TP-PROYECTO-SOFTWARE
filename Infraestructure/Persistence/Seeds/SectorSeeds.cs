using Microsoft.EntityFrameworkCore;
using TP_PROYECTO_SOFTWARE.Domain.Models;

namespace TP_PROYECTO_SOFTWARE.Infraestructure.Persistence.Seeds;

public static class SectorSeeds
{
    public static void Seed(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SECTOR>().HasData(
            new SECTOR
            {
                Id = 1,
                EventId = 1,
                Name = "Sector A",
                Price = 12000m,
                Capacity = 50
            },
            new SECTOR
            {
                Id = 2,
                EventId = 1,
                Name = "Sector B",
                Price = 18000m,
                Capacity = 50
            }
        );
    }
}
