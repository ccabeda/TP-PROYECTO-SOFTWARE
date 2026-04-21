using TP_PROYECTO_SOFTWARE.Aplication.DTOs.SeatDTOs;
using TP_PROYECTO_SOFTWARE.Aplication.UseCases.Seats.Commands;

namespace TP_PROYECTO_SOFTWARE.Aplication.IHandlers
{
    public interface ICreateSeatHandler
    {
        Task<SeatGetDTO> Handle(CreateSeatCommand command);
    }
}
