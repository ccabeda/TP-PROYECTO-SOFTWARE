using Microsoft.EntityFrameworkCore;
using TP_PROYECTO_SOFTWARE.Application.IRepository.IQuery;
using TP_PROYECTO_SOFTWARE.Application.UseCases.Users.Queries;
using TP_PROYECTO_SOFTWARE.Domain.Models;
using TP_PROYECTO_SOFTWARE.Infrastructure.Persistence;

namespace TP_PROYECTO_SOFTWARE.Infrastructure.Repository.Query
{
    public class RepositoryUserQuery : IRepositoryUserQuery
    {
        private readonly ApplicationDbContext _context;

        public RepositoryUserQuery(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<(List<User> Items, int TotalCount)> GetAll(GetUsersQuery query)
        {
            var usersQuery = _context.Users
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.Name))
            {
                var normalizedName = query.Name.Trim();
                usersQuery = usersQuery.Where(u => u.Name != null && u.Name.Contains(normalizedName));
            }

            if (!string.IsNullOrWhiteSpace(query.Email))
            {
                var normalizedEmail = query.Email.Trim();
                usersQuery = usersQuery.Where(u => u.Email != null && u.Email.Contains(normalizedEmail));
            }

            var totalCount = await usersQuery.CountAsync();
            var items = await usersQuery
                .OrderBy(u => u.Name)
                .ThenBy(u => u.Email)
                .Skip((query.Page - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        public async Task<User?> GetById(int id) => await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

        public async Task<User?> GetByEmail(string email) => await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

        public async Task<User?> GetByValidRefreshTokenHash(string refreshTokenHash, DateTime utcNow) =>
            await _context.Users.FirstOrDefaultAsync(user =>
                user.RefreshTokenHash == refreshTokenHash &&
                user.RefreshTokenExpiresAt > utcNow);
    }
}

