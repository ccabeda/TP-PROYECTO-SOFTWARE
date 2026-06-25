using TP_PROYECTO_SOFTWARE.Domain.Models;

namespace TP_PROYECTO_SOFTWARE.Application.IRepository.ICommand
{
    public interface IRepositoryReservationCommand
    {
        Task Create(Reservation reservation);
        Task Update(Reservation reservation);
    }
}
