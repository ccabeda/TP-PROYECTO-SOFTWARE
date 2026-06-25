using TP_PROYECTO_SOFTWARE.Domain.Models;
using TP_PROYECTO_SOFTWARE.Application.UseCases.Events.Queries;

namespace TP_PROYECTO_SOFTWARE.Application.IRepository.IQuery
{
    public interface IRepositoryEventQuery
    {
        Task<Event?> GetById(int id);
        Task<(List<Event> Items, int TotalCount)> GetAll(GetEventsQuery filters);
        Task<bool> ExistsDuplicate(string name, string venue, DateTime eventDate);
    }
}
