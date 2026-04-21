using TP_PROYECTO_SOFTWARE.Aplication.DTOs.SectorDTOs;
using TP_PROYECTO_SOFTWARE.Aplication.UseCases.Sectors.Commands;

namespace TP_PROYECTO_SOFTWARE.Aplication.IHandlers
{
    public interface ICreateSectorHandler
    {
        Task<SectorGetDTO> Handle(CreateSectorCommand command);
    }
}
