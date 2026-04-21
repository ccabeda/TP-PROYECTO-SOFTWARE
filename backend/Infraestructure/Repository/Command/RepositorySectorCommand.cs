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

        public async Task Create(SECTOR sector) => await _context.SECTOR.AddAsync(sector);

        public Task Delete(SECTOR sector)
        {
            _context.SECTOR.Remove(sector);
            return Task.CompletedTask;
        }

        public Task DeleteRange(IEnumerable<SECTOR> sectors)
        {
            _context.SECTOR.RemoveRange(sectors);
            return Task.CompletedTask;
        }

        public async Task Save() => await _context.SaveChangesAsync();
    }
}
