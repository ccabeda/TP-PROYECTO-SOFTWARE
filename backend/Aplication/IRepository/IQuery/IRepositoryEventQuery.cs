using TP_PROYECTO_SOFTWARE.Domain.Models;
using TP_PROYECTO_SOFTWARE.Aplication.UseCases.Events.Queries;

namespace TP_PROYECTO_SOFTWARE.Aplication.IRepository.IQuery
{
    public interface IRepositoryEventQuery
    {
        Task<Event?> GetById(int id);
        Task<List<Event>> GetAll(GetEventsQuery filters);
    }
}
