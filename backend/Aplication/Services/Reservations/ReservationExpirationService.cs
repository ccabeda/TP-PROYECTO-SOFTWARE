using TP_PROYECTO_SOFTWARE.Aplication.IHandlers;
using TP_PROYECTO_SOFTWARE.Aplication.IRepository.IQuery;
using TP_PROYECTO_SOFTWARE.Aplication.IUnitOfWork;
using TP_PROYECTO_SOFTWARE.Aplication.UseCases.AuditLogs.Commands;

namespace TP_PROYECTO_SOFTWARE.Aplication.Services.Reservations
{
    public class ReservationExpirationService : IReservationExpirationService
    {
        private readonly IRepositoryReservationQuery _repositoryReservationQuery;
        private readonly IUnitOfWorkReservationCommand _unitOfWorkReservationCommand;
        private readonly ICreateAuditLogHandler _createAuditLogHandler;

        public ReservationExpirationService(
            IRepositoryReservationQuery repositoryReservationQuery,
            IUnitOfWorkReservationCommand unitOfWorkReservationCommand,
            ICreateAuditLogHandler createAuditLogHandler)
        {
            _repositoryReservationQuery = repositoryReservationQuery;
            _unitOfWorkReservationCommand = unitOfWorkReservationCommand;
            _createAuditLogHandler = createAuditLogHandler;
        }

        public async Task ExpirePendingReservations()
        {
            var utcNow = DateTime.UtcNow;
            var expiredReservations = await _repositoryReservationQuery.GetExpiredPendingReservations(utcNow);
            if (expiredReservations.Count == 0)
            {
                return;
            }

            foreach (var reservation in expiredReservations)
            {
                reservation.Status = "Expired";

                var seat = reservation.Seat;
                if (seat.Status == "Reserved")
                {
                    seat.Status = "Available";
                    seat.Version += 1;
                    await _unitOfWorkReservationCommand.RepositorySeatCommand.Update(seat);
                }

                await _unitOfWorkReservationCommand.RepositoryReservationCommand.Update(reservation);
                await CreateExpirationAuditLog(reservation, seat);
            }

            await _unitOfWorkReservationCommand.Save();
        }

        private async Task CreateExpirationAuditLog(Domain.Models.Reservation reservation, Domain.Models.Seat seat)
        {
            await _createAuditLogHandler.Handle(new CreateAuditLogCommand
            {
                UserId = reservation.UserId,
                Action = "ExpireReservation",
                EntityType = "Reservation",
                EntityId = reservation.Id.ToString(),
                Details = $"Reserva expirada. ReservationId={reservation.Id}, SeatId={seat.Id}, UserId={reservation.UserId}, ExpiresAt={reservation.ExpiresAt:O}"
            });
        }
    }
}
