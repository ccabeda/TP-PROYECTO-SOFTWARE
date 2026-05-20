using Microsoft.EntityFrameworkCore;
using TP_PROYECTO_SOFTWARE.Infraestructure.Persistence.Seeds;

namespace TP_PROYECTO_SOFTWARE.Infraestructure.Persistence;

public static class ReferenceDataInitializer
{
    public static async Task InitializeAsync(AplicationDbContext dbContext)
    {
        var eventCount = await dbContext.Events.CountAsync();
        var sectorCount = await dbContext.Sectors.CountAsync();
        var seatCount = await dbContext.Seats.CountAsync();

        if (eventCount == 0 && sectorCount == 0 && seatCount == 0)
        {
            await dbContext.Events.AddRangeAsync(EventSeeds.GetSeedData());
            await dbContext.Sectors.AddRangeAsync(SectorSeeds.GetSeedData());
            await dbContext.Seats.AddRangeAsync(SeatSeeds.GetSeedData());
            await dbContext.SaveChangesAsync();
            return;
        }

        if (eventCount > 0 && sectorCount > 0 && seatCount > 0)
        {
            return;
        }

        throw new InvalidOperationException(
            $"La base quedó con datos semilla incompletos (EVENT={eventCount}, SECTOR={sectorCount}, SEAT={seatCount}). Para evitar inconsistencias, usar una base nueva o vaciarla antes de volver a iniciar la API.");
    }
}
