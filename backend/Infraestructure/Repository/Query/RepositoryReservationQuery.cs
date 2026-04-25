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

        public async Task<Reservation?> GetById(Guid id) => await _context.Reservations
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.Id == id);

        public async Task<Reservation?> GetActiveBySeatId(Guid seatId) => await _context.Reservations
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.SeatId == seatId && r.Status == "Pending");

        public async Task<bool> AnyByEventId(int eventId) => await _context.Reservations
            .AnyAsync(r => r.Seat.Sector.EventId == eventId);

        public async Task<bool> AnyBySectorId(int sectorId) => await _context.Reservations
            .AnyAsync(r => r.Seat.SectorId == sectorId);

        public async Task<bool> AnyBySeatId(Guid seatId) => await _context.Reservations
            .AnyAsync(r => r.SeatId == seatId);

        public async Task<List<Reservation>> GetBySeatIds(List<Guid> seatIds) => await _context.Reservations
            .AsNoTracking()
            .Where(r => seatIds.Contains(r.SeatId))
            .ToListAsync();
    }
}

