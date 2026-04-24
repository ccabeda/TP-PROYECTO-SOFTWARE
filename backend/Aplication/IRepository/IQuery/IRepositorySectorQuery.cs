using TP_PROYECTO_SOFTWARE.Domain.Models;

namespace TP_PROYECTO_SOFTWARE.Aplication.IRepository.IQuery
{
    public interface IRepositorySectorQuery
    {
        Task<List<SECTOR>> GetByEventId(int eventId);
        Task<int> CountByEventId(int eventId);
        Task<SECTOR?> GetById(int id);
    }
}
