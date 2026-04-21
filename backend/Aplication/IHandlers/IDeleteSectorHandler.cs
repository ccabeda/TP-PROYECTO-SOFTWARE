using TP_PROYECTO_SOFTWARE.Aplication.UseCases.Sectors.Commands;

namespace TP_PROYECTO_SOFTWARE.Aplication.IHandlers
{
    public interface IDeleteSectorHandler
    {
        Task Handle(DeleteSectorCommand command);
    }
}
