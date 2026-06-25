using TP_PROYECTO_SOFTWARE.Application.DTOs.SeatDTOs;
using TP_PROYECTO_SOFTWARE.Application.UseCases.Seats.Queries;

namespace TP_PROYECTO_SOFTWARE.Application.IHandlers
{
    public interface IGetSeatsBySectorHandler
    {
        Task<List<SeatGetDTO>> Handle(GetSeatsBySectorQuery query);
    }
}
