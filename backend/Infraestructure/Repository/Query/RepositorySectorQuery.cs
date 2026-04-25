using Microsoft.EntityFrameworkCore;
using TP_PROYECTO_SOFTWARE.Aplication.IRepository.IQuery;
using TP_PROYECTO_SOFTWARE.Domain.Models;
using TP_PROYECTO_SOFTWARE.Infraestructure.Persistence;

namespace TP_PROYECTO_SOFTWARE.Infraestructure.Repository.Query
{
    public class RepositorySectorQuery : IRepositorySectorQuery
    {
        private readonly AplicationDbContext _context;

        public RepositorySectorQuery(AplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Sector>> GetByEventId(int eventId) => await _context.Sectors
            .AsNoTracking()
            .Where(s => s.EventId == eventId)
            .ToListAsync();

        public async Task<int> CountByEventId(int eventId) => await _context.Sectors
            .CountAsync(s => s.EventId == eventId);

        public async Task<Sector?> GetById(int id) => await _context.Sectors
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == id);
    }
}

