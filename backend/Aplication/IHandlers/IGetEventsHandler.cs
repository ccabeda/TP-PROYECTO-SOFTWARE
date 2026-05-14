using TP_PROYECTO_SOFTWARE.Aplication.DTOs.EventDTOs;
using TP_PROYECTO_SOFTWARE.Aplication.DTOs;
using TP_PROYECTO_SOFTWARE.Aplication.UseCases.Events.Queries;

namespace TP_PROYECTO_SOFTWARE.Aplication.IHandlers
{
    public interface IGetEventsHandler
    {
        Task<PagedResultDTO<EventGetDTO>> Handle(GetEventsQuery query);
    }
}
