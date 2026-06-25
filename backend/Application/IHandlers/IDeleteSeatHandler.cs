using TP_PROYECTO_SOFTWARE.Application.UseCases.Seats.Commands;

namespace TP_PROYECTO_SOFTWARE.Application.IHandlers
{
    public interface IDeleteSeatHandler
    {
        Task Handle(DeleteSeatCommand command);
    }
}
