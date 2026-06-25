using TP_PROYECTO_SOFTWARE.Domain.Models;

namespace TP_PROYECTO_SOFTWARE.Application.IRepository.ICommand
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
