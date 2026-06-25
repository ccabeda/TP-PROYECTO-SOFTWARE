using TP_PROYECTO_SOFTWARE.Application.DTOs.SectorDTOs;
using TP_PROYECTO_SOFTWARE.Application.UseCases.Sectors.Commands;

namespace TP_PROYECTO_SOFTWARE.Application.IHandlers
{
    public interface ICreateSectorHandler
    {
        Task<SectorGetDTO> Handle(CreateSectorCommand command);
    }
}
