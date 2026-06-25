using TP_PROYECTO_SOFTWARE.Application.DTOs.ReservationDTOs;
using TP_PROYECTO_SOFTWARE.Application.IHandlers;
using TP_PROYECTO_SOFTWARE.Application.IRepository.IQuery;
using TP_PROYECTO_SOFTWARE.Application.UseCases.Reservations.Queries;

namespace TP_PROYECTO_SOFTWARE.Application.UseCases.Reservations.Handlers
{
    public class GetMyReservationsHandler : IGetMyReservationsHandler
    {
        private readonly IRepositoryReservationQuery _repositoryReservationQuery;

        public GetMyReservationsHandler(IRepositoryReservationQuery repositoryReservationQuery)
        {
            _repositoryReservationQuery = repositoryReservationQuery;
        }

        public async Task<List<ReservationTicketGetDTO>> Handle(GetMyReservationsQuery query)
        {
            var reservations = await _repositoryReservationQuery.GetPaidByUserId(query.CurrentUserId);

            return reservations
                .Select(MapReservationToTicket)
                .OrderBy(ticket => ticket.EventDate)
                .ToList();
        }

        private static ReservationTicketGetDTO MapReservationToTicket(Domain.Models.Reservation reservation)
        {
            var seat = reservation.Seat;
            var sector = seat.Sector;
            var eventItem = sector.Event;

            return new ReservationTicketGetDTO
            {
                Id = reservation.Id,
                Status = reservation.Status,
                ReservedAt = reservation.ReservedAt,
                EventId = eventItem.Id,
                EventName = eventItem.Name,
                EventDate = eventItem.EventDate,
                Venue = eventItem.Venue,
                ImageUrl = eventItem.ImageUrl,
                SectorName = sector.Name,
                SectorPrice = sector.Price,
                RowIdentifier = seat.RowIdentifier,
                SeatNumber = seat.SeatNumber
            };
        }
    }
}
