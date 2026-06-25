using TP_PROYECTO_SOFTWARE.Application.DTOs.ReservationDTOs;
using TP_PROYECTO_SOFTWARE.Application.UseCases.Reservations.Commands;

namespace TP_PROYECTO_SOFTWARE.Application.IHandlers
{
    public interface ICreateReservationHandler
    {
        Task<ReservationGetDTO> Handle(CreateReservationCommand command);
    }
}
