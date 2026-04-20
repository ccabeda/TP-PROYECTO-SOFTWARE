using TP_PROYECTO_SOFTWARE.Aplication.DTOs.SeatDTOs;
using TP_PROYECTO_SOFTWARE.Aplication.UseCases.Seats.Queries;

namespace TP_PROYECTO_SOFTWARE.Aplication.IHandlers
{
    public interface IGetSeatsByEventHandler
    {
        Task<List<SeatGetDTO>> Handle(GetSeatsByEventQuery query);
    }
}
