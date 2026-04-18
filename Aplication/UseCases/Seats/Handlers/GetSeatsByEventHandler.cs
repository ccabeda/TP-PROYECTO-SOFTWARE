using AutoMapper;
using TP_PROYECTO_SOFTWARE.Aplication.DTOs.SeatDTOs;
using TP_PROYECTO_SOFTWARE.Aplication.IRepository.IQuery;
using TP_PROYECTO_SOFTWARE.Aplication.UseCases.Seats.Queries;

namespace TP_PROYECTO_SOFTWARE.Aplication.UseCases.Seats.Handlers
{
    public class GetSeatsByEventHandler
    {
        private readonly IRepositorySeatQuery _repositorySeatQuery;
        private readonly IMapper _mapper;

        public GetSeatsByEventHandler(IRepositorySeatQuery repositorySeatQuery, IMapper mapper)
        {
            _repositorySeatQuery = repositorySeatQuery;
            _mapper = mapper;
        }

        public async Task<List<SeatGetDTO>> Handle(GetSeatsByEventQuery query)
        {
            var seats = await _repositorySeatQuery.GetByEventId(query.EventId);
            return _mapper.Map<List<SeatGetDTO>>(seats);
        }
    }
}
