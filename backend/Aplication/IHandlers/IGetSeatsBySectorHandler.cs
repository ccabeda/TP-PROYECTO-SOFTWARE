using TP_PROYECTO_SOFTWARE.Aplication.DTOs.SeatDTOs;
using TP_PROYECTO_SOFTWARE.Aplication.UseCases.Seats.Queries;

namespace TP_PROYECTO_SOFTWARE.Aplication.IHandlers
{
    public interface IGetSeatsBySectorHandler
    {
        Task<List<SeatGetDTO>> Handle(GetSeatsBySectorQuery query);
    }
}
