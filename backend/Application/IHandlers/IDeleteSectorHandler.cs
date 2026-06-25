using TP_PROYECTO_SOFTWARE.Application.UseCases.Sectors.Commands;

namespace TP_PROYECTO_SOFTWARE.Application.IHandlers
{
    public interface IDeleteSectorHandler
    {
        Task Handle(DeleteSectorCommand command);
    }
}
