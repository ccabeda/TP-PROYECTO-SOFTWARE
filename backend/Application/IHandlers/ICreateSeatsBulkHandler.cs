using TP_PROYECTO_SOFTWARE.Application.DTOs.SeatDTOs;
using TP_PROYECTO_SOFTWARE.Application.UseCases.Seats.Commands;

namespace TP_PROYECTO_SOFTWARE.Application.IHandlers
{
    public interface ICreateSeatsBulkHandler
    {
        Task<List<SeatGetDTO>> Handle(CreateSeatsBulkCommand command);
    }
}
