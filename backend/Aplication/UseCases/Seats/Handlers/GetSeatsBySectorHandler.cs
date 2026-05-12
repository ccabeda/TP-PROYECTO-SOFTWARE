using AutoMapper;
using TP_PROYECTO_SOFTWARE.Aplication.IHandlers;
using TP_PROYECTO_SOFTWARE.Aplication.DTOs.SeatDTOs;
using TP_PROYECTO_SOFTWARE.Aplication.IRepository.IQuery;
using TP_PROYECTO_SOFTWARE.Aplication.Services.Reservations;
using TP_PROYECTO_SOFTWARE.Aplication.UseCases.Seats.Queries;

namespace TP_PROYECTO_SOFTWARE.Aplication.UseCases.Seats.Handlers
{
    public class GetSeatsBySectorHandler : IGetSeatsBySectorHandler
    {
        private readonly IRepositorySectorQuery _repositorySectorQuery;
        private readonly IRepositorySeatQuery _repositorySeatQuery;
        private readonly IRepositoryReservationQuery _repositoryReservationQuery;
        private readonly IReservationExpirationService _reservationExpirationService;
        private readonly IMapper _mapper;

        public GetSeatsBySectorHandler(
            IRepositorySectorQuery repositorySectorQuery,
            IRepositorySeatQuery repositorySeatQuery,
            IRepositoryReservationQuery repositoryReservationQuery,
            IReservationExpirationService reservationExpirationService,
            IMapper mapper)
        {
            _repositorySectorQuery = repositorySectorQuery;
            _repositorySeatQuery = repositorySeatQuery;
            _repositoryReservationQuery = repositoryReservationQuery;
            _reservationExpirationService = reservationExpirationService;
            _mapper = mapper;
        }

        public async Task<List<SeatGetDTO>> Handle(GetSeatsBySectorQuery query)
        {
            await _reservationExpirationService.ExpirePendingReservations();

            _ = await _repositorySectorQuery.GetById(query.SectorId)
                ?? throw new KeyNotFoundException("Sector no encontrado.");

            var seats = await _repositorySeatQuery.GetBySectorId(query.SectorId);
            var seatDtos = _mapper.Map<List<SeatGetDTO>>(seats);
            var activeReservations = await _repositoryReservationQuery.GetBySeatIds(seats.Select(seat => seat.Id).ToList());
            var activeReservationsBySeatId = activeReservations
                .GroupBy(reservation => reservation.SeatId)
                .ToDictionary(group => group.Key, group => group.First());

            foreach (var seatDto in seatDtos)
            {
                if (!activeReservationsBySeatId.TryGetValue(seatDto.Id, out var reservation))
                {
                    continue;
                }

                seatDto.ActiveReservationId = reservation.Id;
                seatDto.ReservedByCurrentUser = query.CurrentUserId.HasValue && reservation.UserId == query.CurrentUserId.Value;
            }

            return seatDtos;
        }
    }
}
