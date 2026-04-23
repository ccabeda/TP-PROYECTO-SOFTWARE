using AutoMapper;
using TP_PROYECTO_SOFTWARE.Aplication.DTOs.SectorDTOs;
using TP_PROYECTO_SOFTWARE.Aplication.IHandlers;
using TP_PROYECTO_SOFTWARE.Aplication.IRepository.IQuery;
using TP_PROYECTO_SOFTWARE.Aplication.UseCases.Sectors.Queries;

namespace TP_PROYECTO_SOFTWARE.Aplication.UseCases.Sectors.Handlers
{
    public class GetSectorByIdHandler : IGetSectorByIdHandler
    {
        private readonly IRepositorySectorQuery _repositorySectorQuery;
        private readonly IMapper _mapper;

        public GetSectorByIdHandler(IRepositorySectorQuery repositorySectorQuery, IMapper mapper)
        {
            _repositorySectorQuery = repositorySectorQuery;
            _mapper = mapper;
        }

        public async Task<SectorGetDTO> Handle(GetSectorByIdQuery query)
        {
            var sector = await _repositorySectorQuery.GetById(query.SectorId)
                ?? throw new KeyNotFoundException("Sector no encontrado.");

            if (sector.EventId != query.EventId)
            {
                throw new KeyNotFoundException("Sector no encontrado para el evento indicado.");
            }

            return _mapper.Map<SectorGetDTO>(sector);
        }
    }
}
