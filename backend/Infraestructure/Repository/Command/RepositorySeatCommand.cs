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

        public async Task Create(Seat seat) => await _context.Seats.AddAsync(seat);

        public Task Delete(Seat seat)
        {
            _context.Seats.Remove(seat);
            return Task.CompletedTask;
        }

        public Task DeleteRange(IEnumerable<Seat> seats)
        {
            _context.Seats.RemoveRange(seats);
            return Task.CompletedTask;
        }

        public Task Update(Seat seat)
        {
            _context.Seats.Update(seat);
            return Task.CompletedTask;
        }

        public async Task Save() => await _context.SaveChangesAsync();
    }
}

