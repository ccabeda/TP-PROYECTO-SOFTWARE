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

        public async Task<List<USER>> GetAll() => await _context.USER.ToListAsync();

        public async Task<USER?> GetById(int id) => await _context.USER.FirstOrDefaultAsync(u => u.Id == id);

        public async Task<USER?> GetByEmail(string email) => await _context.USER.FirstOrDefaultAsync(u => u.Email == email);
    }
}
