using TP_PROYECTO_SOFTWARE.Application.DTOs.EventDTOs;
using TP_PROYECTO_SOFTWARE.Application.UseCases.Events.Commands;

namespace TP_PROYECTO_SOFTWARE.Application.IHandlers
{
    public interface ICreateEventHandler
    {
        Task<EventGetDTO> Handle(CreateEventCommand command);
    }
}
