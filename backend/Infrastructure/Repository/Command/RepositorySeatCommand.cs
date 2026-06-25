using TP_PROYECTO_SOFTWARE.Application.IRepository.ICommand;
using TP_PROYECTO_SOFTWARE.Domain.Models;
using TP_PROYECTO_SOFTWARE.Infrastructure.Persistence;

namespace TP_PROYECTO_SOFTWARE.Infrastructure.Repository.Command
{
    public class RepositorySeatCommand : IRepositorySeatCommand
    {
        private readonly ApplicationDbContext _context;

        public RepositorySeatCommand(ApplicationDbContext context)
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

