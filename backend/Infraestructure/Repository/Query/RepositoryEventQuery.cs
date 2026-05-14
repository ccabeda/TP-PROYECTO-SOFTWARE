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

        public async Task<(List<Event> Items, int TotalCount)> GetAll(GetEventsQuery filters)
        {
            var query = _context.Events
                .AsNoTracking()
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
            var normalizedPage = filters.Page < 1 ? 1 : filters.Page;
            var normalizedPageSize = Math.Clamp(filters.PageSize, 1, 50);

            var items = await query
                .OrderBy(e => e.EventDate)
                .Skip((normalizedPage - 1) * normalizedPageSize)
                .Take(normalizedPageSize)
                .ToListAsync();

            return (items, totalCount);
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
            .Include(e => e.Sectors)
                .ThenInclude(s => s.Seats)
            .FirstOrDefaultAsync(e => e.Id == id);
    }
}

