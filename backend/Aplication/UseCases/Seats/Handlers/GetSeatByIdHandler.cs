using AutoMapper;
using TP_PROYECTO_SOFTWARE.Aplication.DTOs.SeatDTOs;
using TP_PROYECTO_SOFTWARE.Aplication.IHandlers;
using TP_PROYECTO_SOFTWARE.Aplication.IRepository.IQuery;
using TP_PROYECTO_SOFTWARE.Aplication.UseCases.Seats.Queries;

namespace TP_PROYECTO_SOFTWARE.Aplication.UseCases.Seats.Handlers
{
    public class GetSeatByIdHandler : IGetSeatByIdHandler
    {
        private readonly IRepositorySeatQuery _repositorySeatQuery;
        private readonly IMapper _mapper;

        public GetSeatByIdHandler(IRepositorySeatQuery repositorySeatQuery, IMapper mapper)
        {
            _repositorySeatQuery = repositorySeatQuery;
            _mapper = mapper;
        }

        public async Task<SeatGetDTO> Handle(GetSeatByIdQuery query)
        {
            var seat = await _repositorySeatQuery.GetById(query.SeatId)
                ?? throw new KeyNotFoundException("Butaca no encontrada.");

            if (seat.SectorId != query.SectorId)
            {
                throw new KeyNotFoundException("Butaca no encontrada para el sector indicado.");
            }

            return _mapper.Map<SeatGetDTO>(seat);
        }
    }
}
