using Microsoft.EntityFrameworkCore;
using TP_PROYECTO_SOFTWARE.Aplication.IRepository.IQuery;
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

        public async Task<List<EVENT>> GetAll() => await _context.EVENT.ToListAsync();

        public async Task<EVENT?> GetById(int id) => await _context.EVENT.FirstOrDefaultAsync(e => e.Id == id);
    }
}
