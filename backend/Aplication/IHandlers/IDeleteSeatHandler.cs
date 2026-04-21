using TP_PROYECTO_SOFTWARE.Aplication.UseCases.Seats.Commands;

namespace TP_PROYECTO_SOFTWARE.Aplication.IHandlers
{
    public interface IDeleteSeatHandler
    {
        Task Handle(DeleteSeatCommand command);
    }
}
