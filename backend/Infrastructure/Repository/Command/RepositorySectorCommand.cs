using TP_PROYECTO_SOFTWARE.Application.IRepository.ICommand;
using TP_PROYECTO_SOFTWARE.Domain.Models;
using TP_PROYECTO_SOFTWARE.Infrastructure.Persistence;

namespace TP_PROYECTO_SOFTWARE.Infrastructure.Repository.Command
{
    public class RepositorySectorCommand : IRepositorySectorCommand
    {
        private readonly ApplicationDbContext _context;

        public RepositorySectorCommand(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Create(Sector sector) => await _context.Sectors.AddAsync(sector);

        public Task Delete(Sector sector)
        {
            _context.Sectors.Remove(sector);
            return Task.CompletedTask;
        }

        public Task DeleteRange(IEnumerable<Sector> sectors)
        {
            _context.Sectors.RemoveRange(sectors);
            return Task.CompletedTask;
        }

        public async Task Save() => await _context.SaveChangesAsync();
    }
}

