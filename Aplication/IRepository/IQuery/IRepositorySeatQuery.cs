using TP_PROYECTO_SOFTWARE.Domain.Models;

namespace TP_PROYECTO_SOFTWARE.Aplication.IRepository.IQuery
{
    public interface IRepositorySeatQuery
    {
        Task<List<SEAT>> GetByEventId(int eventId);
        Task<List<SEAT>> GetBySectorId(int sectorId);
        Task<SEAT?> GetById(Guid id);
    }
}
