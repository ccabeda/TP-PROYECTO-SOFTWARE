using Microsoft.EntityFrameworkCore;
using TP_PROYECTO_SOFTWARE.Aplication.IRepository.IQuery;
using TP_PROYECTO_SOFTWARE.Domain.Models;
using TP_PROYECTO_SOFTWARE.Infraestructure.Persistence;

namespace TP_PROYECTO_SOFTWARE.Infraestructure.Repository.Query
{
    public class RepositorySeatQuery : IRepositorySeatQuery
    {
        private readonly AplicationDbContext _context;

        public RepositorySeatQuery(AplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<SEAT>> GetByEventId(int eventId) => await _context.SEAT
            .AsNoTracking()
            .Include(s => s.Sector)
            .Where(s => s.Sector.EventId == eventId)
            .ToListAsync();

        public async Task<List<SEAT>> GetBySectorId(int sectorId) => await _context.SEAT
            .AsNoTracking()
            .Where(s => s.SectorId == sectorId)
            .ToListAsync();

        public async Task<int> CountBySectorId(int sectorId) => await _context.SEAT
            .CountAsync(s => s.SectorId == sectorId);

        public async Task<bool> ExistsInSector(int sectorId, string rowIdentifier, int seatNumber) => await _context.SEAT
            .AnyAsync(s =>
                s.SectorId == sectorId &&
                s.RowIdentifier == rowIdentifier &&
                s.SeatNumber == seatNumber);

        public async Task<List<SEAT>> GetExistingInSector(int sectorId, List<string> rowIdentifiers, int maxSeatNumber) => await _context.SEAT
            .AsNoTracking()
            .Where(s =>
                s.SectorId == sectorId &&
                rowIdentifiers.Contains(s.RowIdentifier) &&
                s.SeatNumber >= 1 &&
                s.SeatNumber <= maxSeatNumber)
            .ToListAsync();

        public async Task<SEAT?> GetById(Guid id) => await _context.SEAT
            .AsNoTracking()
            .Include(s => s.Sector)
            .ThenInclude(sector => sector.Event)
            .FirstOrDefaultAsync(s => s.Id == id);
    }
}
