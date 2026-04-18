using AutoMapper;
using TP_PROYECTO_SOFTWARE.Aplication.DTOs.SeatDTOs;
using TP_PROYECTO_SOFTWARE.Aplication.IRepository.IQuery;
using TP_PROYECTO_SOFTWARE.Aplication.UseCases.Seats.Queries;

namespace TP_PROYECTO_SOFTWARE.Aplication.UseCases.Seats.Handlers
{
    public class GetSeatsBySectorHandler
    {
        private readonly IRepositorySeatQuery _repositorySeatQuery;
        private readonly IMapper _mapper;

        public GetSeatsBySectorHandler(IRepositorySeatQuery repositorySeatQuery, IMapper mapper)
        {
            _repositorySeatQuery = repositorySeatQuery;
            _mapper = mapper;
        }

        public async Task<List<SeatGetDTO>> Handle(GetSeatsBySectorQuery query)
        {
            var seats = await _repositorySeatQuery.GetBySectorId(query.SectorId);
            return _mapper.Map<List<SeatGetDTO>>(seats);
        }
    }
}
