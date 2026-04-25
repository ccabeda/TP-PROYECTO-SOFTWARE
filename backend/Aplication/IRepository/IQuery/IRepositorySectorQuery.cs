using TP_PROYECTO_SOFTWARE.Domain.Models;

namespace TP_PROYECTO_SOFTWARE.Aplication.IRepository.IQuery
{
    public interface IRepositorySectorQuery
    {
        Task<List<Sector>> GetByEventId(int eventId);
        Task<int> CountByEventId(int eventId);
        Task<Sector?> GetById(int id);
    }
}
