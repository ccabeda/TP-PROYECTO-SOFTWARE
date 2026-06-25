using TP_PROYECTO_SOFTWARE.Application.DTOs.SectorDTOs;
using TP_PROYECTO_SOFTWARE.Application.UseCases.Sectors.Queries;

namespace TP_PROYECTO_SOFTWARE.Application.IHandlers
{
    public interface IGetSectorByIdHandler
    {
        Task<SectorGetDTO> Handle(GetSectorByIdQuery query);
    }
}
