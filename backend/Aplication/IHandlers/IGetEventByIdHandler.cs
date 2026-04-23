using TP_PROYECTO_SOFTWARE.Aplication.DTOs.EventDTOs;
using TP_PROYECTO_SOFTWARE.Aplication.UseCases.Events.Queries;

namespace TP_PROYECTO_SOFTWARE.Aplication.IHandlers
{
    public interface IGetEventByIdHandler
    {
        Task<EventGetDTO> Handle(GetEventByIdQuery query);
    }
}
