using TP_PROYECTO_SOFTWARE.Domain.Models;

namespace TP_PROYECTO_SOFTWARE.Aplication.IRepository.ICommand
{
    public interface IRepositorySeatCommand
    {
        Task Create(Seat seat);
        Task Delete(Seat seat);
        Task DeleteRange(IEnumerable<Seat> seats);
        Task Update(Seat seat);
        Task Save();
    }
}
