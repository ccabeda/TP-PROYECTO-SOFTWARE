using TP_PROYECTO_SOFTWARE.Aplication.IRepository.ICommand;
using TP_PROYECTO_SOFTWARE.Domain.Models;
using TP_PROYECTO_SOFTWARE.Infraestructure.Persistence;

namespace TP_PROYECTO_SOFTWARE.Infraestructure.Repository.Command
{
    public class RepositoryEventCommand : IRepositoryEventCommand
    {
        private readonly AplicationDbContext _context;

        public RepositoryEventCommand(AplicationDbContext context)
        {
            _context = context;
        }

        public async Task Create(Event eventEntity) => await _context.Events.AddAsync(eventEntity);

        public Task Delete(Event eventEntity)
        {
            _context.Events.Remove(eventEntity);
            return Task.CompletedTask;
        }

        public async Task Save() => await _context.SaveChangesAsync();
    }
}

