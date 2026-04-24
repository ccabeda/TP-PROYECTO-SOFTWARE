using TP_PROYECTO_SOFTWARE.Domain.Models;

namespace TP_PROYECTO_SOFTWARE.Aplication.IRepository.IQuery
{
    public interface IRepositorySeatQuery
    {
        Task<List<SEAT>> GetByEventId(int eventId);
        Task<List<SEAT>> GetBySectorId(int sectorId);
        Task<int> CountBySectorId(int sectorId);
        Task<bool> ExistsInSector(int sectorId, string rowIdentifier, int seatNumber);
        Task<List<SEAT>> GetExistingInSector(int sectorId, List<string> rowIdentifiers, int maxSeatNumber);
        Task<SEAT?> GetById(Guid id);
    }
}
