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

        public async Task Create(SEAT seat) => await _context.SEAT.AddAsync(seat);

        public Task Delete(SEAT seat)
        {
            _context.SEAT.Remove(seat);
            return Task.CompletedTask;
        }

        public Task DeleteRange(IEnumerable<SEAT> seats)
        {
            _context.SEAT.RemoveRange(seats);
            return Task.CompletedTask;
        }

        public Task Update(SEAT seat)
        {
            _context.SEAT.Update(seat);
            return Task.CompletedTask;
        }

        public async Task Save() => await _context.SaveChangesAsync();
    }
}
