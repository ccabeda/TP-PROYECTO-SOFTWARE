using AutoMapper;
using TP_PROYECTO_SOFTWARE.Aplication.IHandlers;
using TP_PROYECTO_SOFTWARE.Aplication.DTOs.SeatDTOs;
using TP_PROYECTO_SOFTWARE.Aplication.IRepository.IQuery;
using TP_PROYECTO_SOFTWARE.Aplication.UseCases.Seats.Queries;

namespace TP_PROYECTO_SOFTWARE.Aplication.UseCases.Seats.Handlers
{
    public class GetSeatsBySectorHandler : IGetSeatsBySectorHandler
    {
        private readonly IRepositorySectorQuery _repositorySectorQuery;
        private readonly IRepositorySeatQuery _repositorySeatQuery;
        private readonly IMapper _mapper;

        public GetSeatsBySectorHandler(IRepositorySectorQuery repositorySectorQuery, IRepositorySeatQuery repositorySeatQuery, IMapper mapper)
        {
            _repositorySectorQuery = repositorySectorQuery;
            _repositorySeatQuery = repositorySeatQuery;
            _mapper = mapper;
        }

        public async Task<List<SeatGetDTO>> Handle(GetSeatsBySectorQuery query)
        {
            _ = await _repositorySectorQuery.GetById(query.SectorId)
                ?? throw new KeyNotFoundException("Sector no encontrado.");

            var seats = await _repositorySeatQuery.GetBySectorId(query.SectorId);
            return _mapper.Map<List<SeatGetDTO>>(seats);
        }
    }
}
