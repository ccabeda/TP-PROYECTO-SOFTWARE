using TP_PROYECTO_SOFTWARE.Domain.Models;

namespace TP_PROYECTO_SOFTWARE.Application.IRepository.ICommand
{
    public interface IRepositoryEventCommand
    {
        Task Create(Event eventEntity);
        Task Delete(Event eventEntity);
        Task Save();
    }
}
