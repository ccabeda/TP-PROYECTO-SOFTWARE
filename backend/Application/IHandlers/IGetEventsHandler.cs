using TP_PROYECTO_SOFTWARE.Application.DTOs.EventDTOs;
using TP_PROYECTO_SOFTWARE.Application.DTOs;
using TP_PROYECTO_SOFTWARE.Application.UseCases.Events.Queries;

namespace TP_PROYECTO_SOFTWARE.Application.IHandlers
{
    public interface IGetEventsHandler
    {
        Task<PagedResultDTO<EventGetDTO>> Handle(GetEventsQuery query);
    }
}
