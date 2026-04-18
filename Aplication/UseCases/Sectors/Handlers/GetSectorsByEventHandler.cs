using AutoMapper;
using TP_PROYECTO_SOFTWARE.Aplication.DTOs.SectorDTOs;
using TP_PROYECTO_SOFTWARE.Aplication.IRepository.IQuery;
using TP_PROYECTO_SOFTWARE.Aplication.UseCases.Sectors.Queries;

namespace TP_PROYECTO_SOFTWARE.Aplication.UseCases.Sectors.Handlers
{
    public class GetSectorsByEventHandler
    {
        private readonly IRepositorySectorQuery _repositorySectorQuery;
        private readonly IMapper _mapper;

        public GetSectorsByEventHandler(IRepositorySectorQuery repositorySectorQuery, IMapper mapper)
        {
            _repositorySectorQuery = repositorySectorQuery;
            _mapper = mapper;
        }

        public async Task<List<SectorGetDTO>> Handle(GetSectorsByEventQuery query)
        {
            var sectors = await _repositorySectorQuery.GetByEventId(query.EventId);
            return _mapper.Map<List<SectorGetDTO>>(sectors);
        }
    }
}
