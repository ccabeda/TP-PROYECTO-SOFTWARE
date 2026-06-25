using AutoMapper;
using TP_PROYECTO_SOFTWARE.Application.Exceptions;
using TP_PROYECTO_SOFTWARE.Application.DTOs.ReservationDTOs;
using TP_PROYECTO_SOFTWARE.Application.IHandlers;
using TP_PROYECTO_SOFTWARE.Application.IRepository.IQuery;
using TP_PROYECTO_SOFTWARE.Application.UseCases.Reservations.Queries;

namespace TP_PROYECTO_SOFTWARE.Application.UseCases.Reservations.Handlers
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

            if (!query.IsAdmin && reservation.UserId != query.CurrentUserId)
            {
                throw new ForbiddenAccessException("No tiene permisos para ver esta reserva.");
            }

            return _mapper.Map<ReservationGetDTO>(reservation);
        }
    }
}
