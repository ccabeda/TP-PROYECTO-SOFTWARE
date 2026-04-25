using TP_PROYECTO_SOFTWARE.Domain.Models;

namespace TP_PROYECTO_SOFTWARE.Aplication.IRepository.IQuery
{
    public interface IRepositorySeatQuery
    {
        Task<List<Seat>> GetByEventId(int eventId);
        Task<List<Seat>> GetBySectorId(int sectorId);
        Task<int> CountBySectorId(int sectorId);
        Task<bool> ExistsInSector(int sectorId, string rowIdentifier, int seatNumber);
        Task<List<Seat>> GetExistingInSector(int sectorId, List<string> rowIdentifiers, int maxSeatNumber);
        Task<Seat?> GetById(Guid id);
    }
}
