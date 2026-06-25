using TP_PROYECTO_SOFTWARE.Application.DTOs.ReservationDTOs;
using TP_PROYECTO_SOFTWARE.Application.UseCases.Reservations.Queries;

namespace TP_PROYECTO_SOFTWARE.Application.IHandlers
{
    public interface IGetReservationByIdHandler
    {
        Task<ReservationGetDTO> Handle(GetReservationByIdQuery query);
    }
}
