using TP_PROYECTO_SOFTWARE.Application.UseCases.Events.Commands;

namespace TP_PROYECTO_SOFTWARE.Application.IHandlers
{
    public interface IDeleteEventHandler
    {
        Task Handle(DeleteEventCommand command);
    }
}
