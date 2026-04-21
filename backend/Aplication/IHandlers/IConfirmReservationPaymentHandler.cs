using TP_PROYECTO_SOFTWARE.Aplication.DTOs.ReservationDTOs;
using TP_PROYECTO_SOFTWARE.Aplication.UseCases.Reservations.Commands;

namespace TP_PROYECTO_SOFTWARE.Aplication.IHandlers
{
    public interface IConfirmReservationPaymentHandler
    {
        Task<ReservationGetDTO> Handle(ConfirmReservationPaymentCommand command);
    }
}
