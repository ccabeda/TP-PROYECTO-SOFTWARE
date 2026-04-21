using Microsoft.EntityFrameworkCore;
using TP_PROYECTO_SOFTWARE.Domain.Models;

namespace TP_PROYECTO_SOFTWARE.Infraestructure.Persistence.Seeds;

public static class SeatSeeds
{
    public static void Seed(ModelBuilder modelBuilder)
    {
        var seats = new List<SEAT>();
        var rows = new[] { "A", "B", "C", "D", "E" };

        foreach (var sectorId in new[] { 1, 2 })
        {
            var sequence = 1;

            foreach (var row in rows)
            {
                for (var seatNumber = 1; seatNumber <= 10; seatNumber++)
                {
                    seats.Add(new SEAT
                    {
                        Id = BuildGuid(sectorId, sequence),
                        SectorId = sectorId,
                        RowIdentifier = row,
                        SeatNumber = seatNumber,
                        Status = "Available",
                        Version = 1
                    });

                    sequence++;
                }
            }
        }

        modelBuilder.Entity<SEAT>().HasData(seats);
    }

    private static Guid BuildGuid(int sectorId, int sequence)
    {
        var suffix = $"{sectorId:D2}{sequence:D10}";
        return Guid.Parse($"00000000-0000-0000-0000-{suffix}");
    }
}
