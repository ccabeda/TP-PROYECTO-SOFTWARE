using AutoMapper;
using TP_PROYECTO_SOFTWARE.Aplication.IHandlers;
using TP_PROYECTO_SOFTWARE.Aplication.DTOs.SeatDTOs;
using TP_PROYECTO_SOFTWARE.Aplication.IRepository.IQuery;
using TP_PROYECTO_SOFTWARE.Aplication.UseCases.Seats.Queries;
using System.Linq;

namespace TP_PROYECTO_SOFTWARE.Aplication.UseCases.Seats.Handlers
{
    public class GetSeatsBySectorHandler : IGetSeatsBySectorHandler
    {
        private readonly IRepositorySectorQuery _repositorySectorQuery;
        private readonly IRepositorySeatQuery _repositorySeatQuery;
        private readonly IRepositoryReservationQuery _repositoryReservationQuery;
        private readonly IMapper _mapper;

        public GetSeatsBySectorHandler(
            IRepositorySectorQuery repositorySectorQuery,
            IRepositorySeatQuery repositorySeatQuery,
            IRepositoryReservationQuery repositoryReservationQuery,
            IMapper mapper)
        {
            _repositorySectorQuery = repositorySectorQuery;
            _repositorySeatQuery = repositorySeatQuery;
            _repositoryReservationQuery = repositoryReservationQuery;
            _mapper = mapper;
        }

        public async Task<List<SeatGetDTO>> Handle(GetSeatsBySectorQuery query)
        {
            _ = await _repositorySectorQuery.GetById(query.SectorId)
                ?? throw new KeyNotFoundException("Sector no encontrado.");

            var seats = await _repositorySeatQuery.GetBySectorId(query.SectorId);
            var seatDtos = _mapper.Map<List<SeatGetDTO>>(seats);
            var activeReservations = await _repositoryReservationQuery.GetBySeatIds(seats.Select(seat => seat.Id).ToList());
            var activeReservationsBySeatId = activeReservations
                .Where(reservation => reservation.Status == "Pending")
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
