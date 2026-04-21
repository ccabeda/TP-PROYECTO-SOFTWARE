using TP_PROYECTO_SOFTWARE.Aplication.IRepository.ICommand;
using TP_PROYECTO_SOFTWARE.Domain.Models;
using TP_PROYECTO_SOFTWARE.Infraestructure.Persistence;

namespace TP_PROYECTO_SOFTWARE.Infraestructure.Repository.Command
{
    public class RepositoryReservationCommand : IRepositoryReservationCommand
    {
        private readonly AplicationDbContext _context;

        public RepositoryReservationCommand(AplicationDbContext context)
        {
            _context = context;
        }

        public async Task Create(RESERVATION reservation) => await _context.RESERVATION.AddAsync(reservation);

        public Task Update(RESERVATION reservation)
        {
            _context.RESERVATION.Update(reservation);
            return Task.CompletedTask;
        }
    }
}
