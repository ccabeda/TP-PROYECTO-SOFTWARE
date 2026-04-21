using TP_PROYECTO_SOFTWARE.Domain.Models;

namespace TP_PROYECTO_SOFTWARE.Aplication.IRepository.ICommand
{
    public interface IRepositoryEventCommand
    {
        Task Create(EVENT eventEntity);
        Task Delete(EVENT eventEntity);
        Task Save();
    }
}
