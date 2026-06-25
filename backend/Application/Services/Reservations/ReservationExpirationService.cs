using Microsoft.Extensions.Logging;
using TP_PROYECTO_SOFTWARE.Application.IRepository.ICommand;
using TP_PROYECTO_SOFTWARE.Application.IRepository.IQuery;
using TP_PROYECTO_SOFTWARE.Application.IUnitOfWork;
using TP_PROYECTO_SOFTWARE.Domain.Constants;
using TP_PROYECTO_SOFTWARE.Domain.Factories;

namespace TP_PROYECTO_SOFTWARE.Application.Services.Reservations
{
    public class ReservationExpirationService : IReservationExpirationService
    {
        private readonly IRepositoryReservationQuery _repositoryReservationQuery;
        private readonly IRepositoryReservationCommand _repositoryReservationCommand;
        private readonly IRepositorySeatCommand _repositorySeatCommand;
        private readonly IRepositoryAuditLogCommand _repositoryAuditLogCommand;
        private readonly IApplicationUnitOfWork _unitOfWork;
        private readonly ILogger<ReservationExpirationService> _logger;

        public ReservationExpirationService(
            IRepositoryReservationQuery repositoryReservationQuery,
            IRepositoryReservationCommand repositoryReservationCommand,
            IRepositorySeatCommand repositorySeatCommand,
            IRepositoryAuditLogCommand repositoryAuditLogCommand,
            IApplicationUnitOfWork unitOfWork,
            ILogger<ReservationExpirationService> logger)
        {
            _repositoryReservationQuery = repositoryReservationQuery;
            _repositoryReservationCommand = repositoryReservationCommand;
            _repositorySeatCommand = repositorySeatCommand;
            _repositoryAuditLogCommand = repositoryAuditLogCommand;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task ExpirePendingReservations()
        {
            var utcNow = DateTime.UtcNow;
            var expiredReservations = await _repositoryReservationQuery.GetExpiredPendingReservations(utcNow);
            if (expiredReservations.Count == 0)
            {
                _logger.LogDebug("No se encontraron reservas pendientes vencidas para expirar.");
                return;
            }

            foreach (var reservation in expiredReservations)
            {
                reservation.Status = ReservationStatuses.Expired;

                var seat = reservation.Seat;
                if (seat.Status == SeatStatuses.Reserved)
                {
                    seat.Status = SeatStatuses.Available;
                    seat.Version += 1;
                    await _repositorySeatCommand.Update(seat);
                }

                await _repositoryReservationCommand.Update(reservation);
                await CreateExpirationAuditLog(reservation, seat);
            }

            await _unitOfWork.SaveChangesAsync();
            _logger.LogInformation(
                "Se expiraron {ExpiredReservationCount} reservas pendientes vencidas.",
                expiredReservations.Count);
        }

        private async Task CreateExpirationAuditLog(Domain.Models.Reservation reservation, Domain.Models.Seat seat)
        {
            var auditLog = AuditLogFactory.Create(
                reservation.UserId,
                AuditActions.ExpireReservation,
                AuditEntityTypes.Reservation,
                reservation.Id.ToString(),
                $"Reserva expirada. ReservationId={reservation.Id}, SeatId={seat.Id}, UserId={reservation.UserId}, ExpiresAt={reservation.ExpiresAt:O}");

            await _repositoryAuditLogCommand.Create(auditLog);
        }
    }
}
