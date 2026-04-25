using TP_PROYECTO_SOFTWARE.Aplication.IRepository.ICommand;
using TP_PROYECTO_SOFTWARE.Domain.Models;
using TP_PROYECTO_SOFTWARE.Infraestructure.Persistence;

namespace TP_PROYECTO_SOFTWARE.Infraestructure.Repository.Command
{
    public class RepositorySectorCommand : IRepositorySectorCommand
    {
        private readonly AplicationDbContext _context;

        public RepositorySectorCommand(AplicationDbContext context)
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

