using TP_PROYECTO_SOFTWARE.Aplication.DTOs.SeatDTOs;
using TP_PROYECTO_SOFTWARE.Aplication.UseCases.Seats.Queries;

namespace TP_PROYECTO_SOFTWARE.Aplication.IHandlers
{
    public interface IGetSeatByIdHandler
    {
        Task<SeatGetDTO> Handle(GetSeatByIdQuery query);
    }
}
