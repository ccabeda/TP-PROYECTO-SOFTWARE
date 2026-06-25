using TP_PROYECTO_SOFTWARE.Application.DTOs.SeatDTOs;
using TP_PROYECTO_SOFTWARE.Application.UseCases.Seats.Commands;

namespace TP_PROYECTO_SOFTWARE.Application.IHandlers
{
    public interface ICreateSeatHandler
    {
        Task<SeatGetDTO> Handle(CreateSeatCommand command);
    }
}
