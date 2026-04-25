using TP_PROYECTO_SOFTWARE.Domain.Models;

namespace TP_PROYECTO_SOFTWARE.Aplication.IRepository.ICommand
{
    public interface IRepositoryReservationCommand
    {
        Task Create(Reservation reservation);
        Task Update(Reservation reservation);
    }
}
