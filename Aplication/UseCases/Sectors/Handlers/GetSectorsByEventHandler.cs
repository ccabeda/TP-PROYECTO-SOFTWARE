using AutoMapper;
using TP_PROYECTO_SOFTWARE.Aplication.IHandlers;
using TP_PROYECTO_SOFTWARE.Aplication.DTOs.SectorDTOs;
using TP_PROYECTO_SOFTWARE.Aplication.IRepository.IQuery;
using TP_PROYECTO_SOFTWARE.Aplication.UseCases.Sectors.Queries;

namespace TP_PROYECTO_SOFTWARE.Aplication.UseCases.Sectors.Handlers
{
    public class GetSectorsByEventHandler : IGetSectorsByEventHandler
    {
        private readonly IRepositoryEventQuery _repositoryEventQuery;
        private readonly IRepositorySectorQuery _repositorySectorQuery;
        private readonly IMapper _mapper;

        public GetSectorsByEventHandler(IRepositoryEventQuery repositoryEventQuery, IRepositorySectorQuery repositorySectorQuery, IMapper mapper)
        {
            _repositoryEventQuery = repositoryEventQuery;
            _repositorySectorQuery = repositorySectorQuery;
            _mapper = mapper;
        }

        public async Task<List<SectorGetDTO>> Handle(GetSectorsByEventQuery query)
        {
            _ = await _repositoryEventQuery.GetById(query.EventId)
                ?? throw new KeyNotFoundException("Evento no encontrado.");

            var sectors = await _repositorySectorQuery.GetByEventId(query.EventId);
            return _mapper.Map<List<SectorGetDTO>>(sectors);
        }
    }
}
