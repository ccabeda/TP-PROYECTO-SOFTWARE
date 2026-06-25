using TP_PROYECTO_SOFTWARE.Application.DTOs.SectorDTOs;
using TP_PROYECTO_SOFTWARE.Application.UseCases.Sectors.Queries;

namespace TP_PROYECTO_SOFTWARE.Application.IHandlers
{
    public interface IGetSectorsByEventHandler
    {
        Task<List<SectorGetDTO>> Handle(GetSectorsByEventQuery query);
    }
}
