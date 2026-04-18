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
            .Include(s => s.Sector)
            .Where(s => s.Sector.EventId == eventId)
            .ToListAsync();

        public async Task<List<SEAT>> GetBySectorId(int sectorId) => await _context.SEAT
            .Where(s => s.SectorId == sectorId)
            .ToListAsync();

        public async Task<SEAT?> GetById(Guid id) => await _context.SEAT.FirstOrDefaultAsync(s => s.Id == id);
    }
}
