using Microsoft.EntityFrameworkCore;
using TP_PROYECTO_SOFTWARE.Aplication.IRepository.IQuery;
using TP_PROYECTO_SOFTWARE.Domain.Models;
using TP_PROYECTO_SOFTWARE.Infraestructure.Persistence;

namespace TP_PROYECTO_SOFTWARE.Infraestructure.Repository.Query
{
    public class RepositoryUserQuery : IRepositoryUserQuery
    {
        private readonly AplicationDbContext _context;

        public RepositoryUserQuery(AplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<User>> GetAll() => await _context.Users
            .AsNoTracking()
            .ToListAsync();

        public async Task<User?> GetById(int id) => await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

        public async Task<User?> GetByEmail(string email) => await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }
}

