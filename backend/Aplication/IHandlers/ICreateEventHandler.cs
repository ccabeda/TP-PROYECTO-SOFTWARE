using TP_PROYECTO_SOFTWARE.Aplication.DTOs.EventDTOs;
using TP_PROYECTO_SOFTWARE.Aplication.UseCases.Events.Commands;

namespace TP_PROYECTO_SOFTWARE.Aplication.IHandlers
{
    public interface ICreateEventHandler
    {
        Task<EventGetDTO> Handle(CreateEventCommand command);
    }
}
