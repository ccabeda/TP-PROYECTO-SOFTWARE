using TP_PROYECTO_SOFTWARE.Application.IRepository.ICommand;
using TP_PROYECTO_SOFTWARE.Domain.Models;
using TP_PROYECTO_SOFTWARE.Infrastructure.Persistence;

namespace TP_PROYECTO_SOFTWARE.Infrastructure.Repository.Command
{
    public class RepositoryReservationCommand : IRepositoryReservationCommand
    {
        private readonly ApplicationDbContext _context;

        public RepositoryReservationCommand(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Create(Reservation reservation) => await _context.Reservations.AddAsync(reservation);

        public Task Update(Reservation reservation)
        {
            _context.Reservations.Update(reservation);
            return Task.CompletedTask;
        }
    }
}

