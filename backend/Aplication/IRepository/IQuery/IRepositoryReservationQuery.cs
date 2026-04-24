using TP_PROYECTO_SOFTWARE.Domain.Models;

namespace TP_PROYECTO_SOFTWARE.Aplication.IRepository.IQuery
{
    public interface IRepositoryReservationQuery
    {
        Task<RESERVATION?> GetById(Guid id);
        Task<RESERVATION?> GetActiveBySeatId(Guid seatId);
        Task<bool> AnyByEventId(int eventId);
        Task<bool> AnyBySectorId(int sectorId);
        Task<bool> AnyBySeatId(Guid seatId);
        Task<List<RESERVATION>> GetBySeatIds(List<Guid> seatIds);
    }
}
