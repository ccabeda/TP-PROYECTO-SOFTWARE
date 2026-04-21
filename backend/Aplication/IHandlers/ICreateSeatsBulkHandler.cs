using TP_PROYECTO_SOFTWARE.Aplication.DTOs.SeatDTOs;
using TP_PROYECTO_SOFTWARE.Aplication.UseCases.Seats.Commands;

namespace TP_PROYECTO_SOFTWARE.Aplication.IHandlers
{
    public interface ICreateSeatsBulkHandler
    {
        Task<List<SeatGetDTO>> Handle(CreateSeatsBulkCommand command);
    }
}
