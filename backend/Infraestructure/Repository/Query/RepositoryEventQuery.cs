using Microsoft.EntityFrameworkCore;
using TP_PROYECTO_SOFTWARE.Aplication.IRepository.IQuery;
using TP_PROYECTO_SOFTWARE.Aplication.UseCases.Events.Queries;
using TP_PROYECTO_SOFTWARE.Domain.Models;
using TP_PROYECTO_SOFTWARE.Infraestructure.Persistence;

namespace TP_PROYECTO_SOFTWARE.Infraestructure.Repository.Query
{
    public class RepositoryEventQuery : IRepositoryEventQuery
    {
        private readonly AplicationDbContext _context;

        public RepositoryEventQuery(AplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<EVENT>> GetAll(GetEventsQuery filters)
        {
            var query = _context.EVENT
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(filters.Name))
            {
                var normalizedName = filters.Name.Trim().ToLower();
                query = query.Where(e => e.Name.ToLower().Contains(normalizedName));
            }

            if (filters.EventDate.HasValue)
            {
                var filterDate = filters.EventDate.Value.Date;
                query = query.Where(e => e.EventDate.Date == filterDate);
            }

            return await query.ToListAsync();
        }

        public async Task<EVENT?> GetById(int id) => await _context.EVENT
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == id);
    }
}
