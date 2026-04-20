using TP_PROYECTO_SOFTWARE.Aplication.DTOs.ReservationDTOs;
using TP_PROYECTO_SOFTWARE.Aplication.UseCases.Reservations.Queries;

namespace TP_PROYECTO_SOFTWARE.Aplication.IHandlers
{
    public interface IGetReservationByIdHandler
    {
        Task<ReservationGetDTO> Handle(GetReservationByIdQuery query);
    }
}
