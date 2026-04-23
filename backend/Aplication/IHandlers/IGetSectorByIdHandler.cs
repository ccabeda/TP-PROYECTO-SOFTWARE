using TP_PROYECTO_SOFTWARE.Aplication.DTOs.SectorDTOs;
using TP_PROYECTO_SOFTWARE.Aplication.UseCases.Sectors.Queries;

namespace TP_PROYECTO_SOFTWARE.Aplication.IHandlers
{
    public interface IGetSectorByIdHandler
    {
        Task<SectorGetDTO> Handle(GetSectorByIdQuery query);
    }
}
