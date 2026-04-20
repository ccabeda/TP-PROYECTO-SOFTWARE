using AutoMapper;
using TP_PROYECTO_SOFTWARE.Aplication.IHandlers;
using TP_PROYECTO_SOFTWARE.Aplication.DTOs.SeatDTOs;
using TP_PROYECTO_SOFTWARE.Aplication.IRepository.IQuery;
using TP_PROYECTO_SOFTWARE.Aplication.UseCases.Seats.Queries;

namespace TP_PROYECTO_SOFTWARE.Aplication.UseCases.Seats.Handlers
{
    public class GetSeatsByEventHandler : IGetSeatsByEventHandler
    {
        private readonly IRepositoryEventQuery _repositoryEventQuery;
        private readonly IRepositorySeatQuery _repositorySeatQuery;
        private readonly IMapper _mapper;

        public GetSeatsByEventHandler(IRepositoryEventQuery repositoryEventQuery, IRepositorySeatQuery repositorySeatQuery, IMapper mapper)
        {
            _repositoryEventQuery = repositoryEventQuery;
            _repositorySeatQuery = repositorySeatQuery;
            _mapper = mapper;
        }

        public async Task<List<SeatGetDTO>> Handle(GetSeatsByEventQuery query)
        {
            _ = await _repositoryEventQuery.GetById(query.EventId)
                ?? throw new KeyNotFoundException("Evento no encontrado.");

            var seats = await _repositorySeatQuery.GetByEventId(query.EventId);
            return _mapper.Map<List<SeatGetDTO>>(seats);
        }
    }
}
