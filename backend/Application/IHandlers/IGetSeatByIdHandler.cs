using TP_PROYECTO_SOFTWARE.Application.DTOs.SeatDTOs;
using TP_PROYECTO_SOFTWARE.Application.UseCases.Seats.Queries;

namespace TP_PROYECTO_SOFTWARE.Application.IHandlers
{
    public interface IGetSeatByIdHandler
    {
        Task<SeatGetDTO> Handle(GetSeatByIdQuery query);
    }
}
