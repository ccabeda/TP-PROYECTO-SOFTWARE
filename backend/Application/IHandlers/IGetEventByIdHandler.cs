using TP_PROYECTO_SOFTWARE.Application.DTOs.EventDTOs;
using TP_PROYECTO_SOFTWARE.Application.UseCases.Events.Queries;

namespace TP_PROYECTO_SOFTWARE.Application.IHandlers
{
    public interface IGetEventByIdHandler
    {
        Task<EventGetDTO> Handle(GetEventByIdQuery query);
    }
}
