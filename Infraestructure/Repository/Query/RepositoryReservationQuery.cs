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
            .FirstOrDefaultAsync(r => r.Id == id);

        public async Task<RESERVATION?> GetActiveBySeatId(Guid seatId) => await _context.RESERVATION
            .FirstOrDefaultAsync(r => r.SeatId == seatId && r.Status == "Pending");
    }
}
