using TP_PROYECTO_SOFTWARE.Aplication.UseCases.Events.Commands;

namespace TP_PROYECTO_SOFTWARE.Aplication.IHandlers
{
    public interface IDeleteEventHandler
    {
        Task Handle(DeleteEventCommand command);
    }
}
