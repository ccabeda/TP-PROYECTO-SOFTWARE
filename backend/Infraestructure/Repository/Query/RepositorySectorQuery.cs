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

        public async Task<List<SECTOR>> GetByEventId(int eventId) => await _context.SECTOR
            .AsNoTracking()
            .Where(s => s.EventId == eventId)
            .ToListAsync();

        public async Task<int> CountByEventId(int eventId) => await _context.SECTOR
            .CountAsync(s => s.EventId == eventId);

        public async Task<SECTOR?> GetById(int id) => await _context.SECTOR
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == id);
    }
}
