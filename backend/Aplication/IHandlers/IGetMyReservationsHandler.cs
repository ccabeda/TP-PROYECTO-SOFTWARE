using TP_PROYECTO_SOFTWARE.Aplication.DTOs.ReservationDTOs;
using TP_PROYECTO_SOFTWARE.Aplication.UseCases.Reservations.Queries;

namespace TP_PROYECTO_SOFTWARE.Aplication.IHandlers
{
    public interface IGetMyReservationsHandler
    {
        Task<List<ReservationTicketGetDTO>> Handle(GetMyReservationsQuery query);
    }
}
