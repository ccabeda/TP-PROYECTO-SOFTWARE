using Microsoft.EntityFrameworkCore;
using TP_PROYECTO_SOFTWARE.Application.IRepository.IQuery;
using TP_PROYECTO_SOFTWARE.Application.UseCases.Events.Queries;
using TP_PROYECTO_SOFTWARE.Domain.Models;
using TP_PROYECTO_SOFTWARE.Infrastructure.Persistence;

namespace TP_PROYECTO_SOFTWARE.Infrastructure.Repository.Query
{
    public class RepositoryEventQuery : IRepositoryEventQuery
    {
        private readonly ApplicationDbContext _context;

        public RepositoryEventQuery(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<(List<Event> Items, int TotalCount)> GetAll(GetEventsQuery filters)
        {
            var query = _context.Events
                .AsNoTracking()
                .AsSplitQuery()
                .Include(e => e.Sectors)
                    .ThenInclude(s => s.Seats)
                .Where(e => e.EventDate >= DateTime.UtcNow)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(filters.Name))
            {
                var normalizedName = filters.Name.Trim();
                query = query.Where(e => e.Name.Contains(normalizedName));
            }

            if (filters.EventDate.HasValue)
            {
                var filterDate = filters.EventDate.Value.Date;
                query = query.Where(e => e.EventDate.Date == filterDate);
            }

            var totalCount = await query.CountAsync();
            var normalizedPage = filters.Page < 1 ? 1 : filters.Page; //verifica que exista al menos 1 pag
            var normalizedPageSize = Math.Clamp(filters.PageSize, 1, 50); //limita a 50 el tama�o de la pag

            var items = await query //aplica la paginacion
                .OrderBy(e => e.EventDate)
                .Skip((normalizedPage - 1) * normalizedPageSize)
                .Take(normalizedPageSize)
                .ToListAsync(); //aca recien arma la lista paginada

            return (items, totalCount); //retorna la lista de eventos y el total de eventos que cumplen con los filtros
        }

        public async Task<bool> ExistsDuplicate(string name, string venue, DateTime eventDate)
        {
            var normalizedName = name.Trim();
            var normalizedVenue = venue.Trim();

            return await _context.Events
                .AsNoTracking()
                .AnyAsync(e =>
                    e.EventDate == eventDate &&
                    e.Name == normalizedName &&
                    e.Venue == normalizedVenue);
        }

        public async Task<Event?> GetById(int id) => await _context.Events
            .AsNoTracking()
            .AsSplitQuery()
            .Include(e => e.Sectors)
                .ThenInclude(s => s.Seats)
            .FirstOrDefaultAsync(e => e.Id == id);
    }
}

