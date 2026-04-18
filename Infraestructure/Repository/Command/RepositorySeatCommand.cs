using TP_PROYECTO_SOFTWARE.Aplication.IRepository.ICommand;
using TP_PROYECTO_SOFTWARE.Domain.Models;
using TP_PROYECTO_SOFTWARE.Infraestructure.Persistence;

namespace TP_PROYECTO_SOFTWARE.Infraestructure.Repository.Command
{
    public class RepositorySeatCommand : IRepositorySeatCommand
    {
        private readonly AplicationDbContext _context;

        public RepositorySeatCommand(AplicationDbContext context)
        {
            _context = context;
        }

        public Task Update(SEAT seat)
        {
            _context.SEAT.Update(seat);
            return Task.CompletedTask;
        }
    }
}
