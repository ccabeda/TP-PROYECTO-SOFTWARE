using Microsoft.EntityFrameworkCore;
using TP_PROYECTO_SOFTWARE.Aplication.IRepository.IQuery;
using TP_PROYECTO_SOFTWARE.Domain.Models;
using TP_PROYECTO_SOFTWARE.Infraestructure.Persistence;

namespace TP_PROYECTO_SOFTWARE.Infraestructure.Repository.Query
{
    public class RepositoryReservationQuery : IRepositoryReservationQuery
    {
        private readonly AplicationDbContext _context;

        public RepositoryReservationQuery(AplicationDbContext context)
        {
            _context = context;
        }

        public async Task<RESERVATION?> GetById(Guid id) => await _context.RESERVATION
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.Id == id);

        public async Task<RESERVATION?> GetActiveBySeatId(Guid seatId) => await _context.RESERVATION
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.SeatId == seatId && r.Status == "Pending");

        public async Task<bool> AnyByEventId(int eventId) => await _context.RESERVATION
            .AnyAsync(r => r.Seat.Sector.EventId == eventId);

        public async Task<bool> AnyBySectorId(int sectorId) => await _context.RESERVATION
            .AnyAsync(r => r.Seat.SectorId == sectorId);

        public async Task<bool> AnyBySeatId(Guid seatId) => await _context.RESERVATION
            .AnyAsync(r => r.SeatId == seatId);

        public async Task<List<RESERVATION>> GetBySeatIds(List<Guid> seatIds) => await _context.RESERVATION
            .AsNoTracking()
            .Where(r => seatIds.Contains(r.SeatId))
            .ToListAsync();
    }
}
