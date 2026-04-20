using AutoMapper;
using TP_PROYECTO_SOFTWARE.Aplication.DTOs.ReservationDTOs;
using TP_PROYECTO_SOFTWARE.Aplication.IHandlers;
using TP_PROYECTO_SOFTWARE.Aplication.IRepository.IQuery;
using TP_PROYECTO_SOFTWARE.Aplication.UseCases.Reservations.Queries;

namespace TP_PROYECTO_SOFTWARE.Aplication.UseCases.Reservations.Handlers
{
    public class GetReservationByIdHandler : IGetReservationByIdHandler
    {
        private readonly IRepositoryReservationQuery _repositoryReservationQuery;
        private readonly IMapper _mapper;

        public GetReservationByIdHandler(IRepositoryReservationQuery repositoryReservationQuery, IMapper mapper)
        {
            _repositoryReservationQuery = repositoryReservationQuery;
            _mapper = mapper;
        }

        public async Task<ReservationGetDTO> Handle(GetReservationByIdQuery query)
        {
            var reservation = await _repositoryReservationQuery.GetById(query.Id)
                ?? throw new KeyNotFoundException("Reserva no encontrada.");

            return _mapper.Map<ReservationGetDTO>(reservation);
        }
    }
}
