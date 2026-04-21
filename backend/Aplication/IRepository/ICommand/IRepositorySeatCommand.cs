using TP_PROYECTO_SOFTWARE.Domain.Models;

namespace TP_PROYECTO_SOFTWARE.Aplication.IRepository.ICommand
{
    public interface IRepositorySeatCommand
    {
        Task Create(SEAT seat);
        Task Delete(SEAT seat);
        Task DeleteRange(IEnumerable<SEAT> seats);
        Task Update(SEAT seat);
        Task Save();
    }
}
