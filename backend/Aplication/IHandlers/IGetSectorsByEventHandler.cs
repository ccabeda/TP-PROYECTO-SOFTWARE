using TP_PROYECTO_SOFTWARE.Aplication.DTOs.SectorDTOs;
using TP_PROYECTO_SOFTWARE.Aplication.UseCases.Sectors.Queries;

namespace TP_PROYECTO_SOFTWARE.Aplication.IHandlers
{
    public interface IGetSectorsByEventHandler
    {
        Task<List<SectorGetDTO>> Handle(GetSectorsByEventQuery query);
    }
}
