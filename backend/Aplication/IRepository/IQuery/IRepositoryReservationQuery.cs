using TP_PROYECTO_SOFTWARE.Domain.Models;

namespace TP_PROYECTO_SOFTWARE.Aplication.IRepository.IQuery
{
    public interface IRepositoryReservationQuery
    {
        Task<Reservation?> GetById(Guid id);
        Task<Reservation?> GetActiveBySeatId(Guid seatId);
        Task<bool> AnyByEventId(int eventId);
        Task<bool> AnyBySectorId(int sectorId);
        Task<bool> AnyBySeatId(Guid seatId);
        Task<List<Reservation>> GetBySeatIds(List<Guid> seatIds);
    }
}
