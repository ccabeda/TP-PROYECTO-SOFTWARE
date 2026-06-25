using Microsoft.EntityFrameworkCore;
using TP_PROYECTO_SOFTWARE.Application.IRepository.IQuery;
using TP_PROYECTO_SOFTWARE.Domain.Constants;
using TP_PROYECTO_SOFTWARE.Domain.Models;
using TP_PROYECTO_SOFTWARE.Infrastructure.Persistence;

namespace TP_PROYECTO_SOFTWARE.Infrastructure.Repository.Query
{
    public class RepositoryReservationQuery : IRepositoryReservationQuery
    {
        private readonly ApplicationDbContext _context;

        public RepositoryReservationQuery(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Reservation?> GetById(Guid id) => await _context.Reservations
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.Id == id);

        public async Task<Reservation?> GetActiveBySeatId(Guid seatId) => await _context.Reservations
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.SeatId == seatId && r.Status == ReservationStatuses.Pending && r.ExpiresAt > DateTime.UtcNow);

        public async Task<List<Reservation>> GetExpiredPendingReservations(DateTime utcNow) => await _context.Reservations
            .Include(r => r.Seat)
            .Where(r => r.Status == ReservationStatuses.Pending && r.ExpiresAt <= utcNow)
            .ToListAsync();

        public async Task<bool> AnyByEventId(int eventId) => await _context.Reservations
            .AnyAsync(r => r.Seat.Sector.EventId == eventId);

        public async Task<bool> AnyBySectorId(int sectorId) => await _context.Reservations
            .AnyAsync(r => r.Seat.SectorId == sectorId);

        public async Task<bool> AnyBySeatId(Guid seatId) => await _context.Reservations
            .AnyAsync(r => r.SeatId == seatId);

        public async Task<List<Reservation>> GetBySeatIds(List<Guid> seatIds) => await _context.Reservations
            .AsNoTracking()
            .Where(r => seatIds.Contains(r.SeatId) && r.Status == ReservationStatuses.Pending && r.ExpiresAt > DateTime.UtcNow)
            .ToListAsync();

        public async Task<List<Reservation>> GetPaidByUserId(int userId) => await _context.Reservations
            .AsNoTracking()
            .Include(r => r.Seat)
                .ThenInclude(seat => seat.Sector)
                    .ThenInclude(sector => sector.Event)
            .Where(r => r.UserId == userId && r.Status == ReservationStatuses.Paid)
            .ToListAsync();
    }
}

