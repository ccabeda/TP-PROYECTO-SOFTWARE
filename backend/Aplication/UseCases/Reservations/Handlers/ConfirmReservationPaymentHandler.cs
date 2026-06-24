using AutoMapper;
using TP_PROYECTO_SOFTWARE.Aplication.Exceptions;
using TP_PROYECTO_SOFTWARE.Aplication.DTOs.ReservationDTOs;
using TP_PROYECTO_SOFTWARE.Aplication.IHandlers;
using TP_PROYECTO_SOFTWARE.Aplication.IRepository.ICommand;
using TP_PROYECTO_SOFTWARE.Aplication.IRepository.IQuery;
using TP_PROYECTO_SOFTWARE.Aplication.IUnitOfWork;
using TP_PROYECTO_SOFTWARE.Aplication.Services.Reservations;
using TP_PROYECTO_SOFTWARE.Aplication.UseCases.Reservations.Commands;
using TP_PROYECTO_SOFTWARE.Domain.Constants;
using TP_PROYECTO_SOFTWARE.Domain.Factories;
using TP_PROYECTO_SOFTWARE.Domain.Models;

namespace TP_PROYECTO_SOFTWARE.Aplication.UseCases.Reservations.Handlers
{
    public class ConfirmReservationPaymentHandler : IConfirmReservationPaymentHandler
    {
        private readonly IRepositoryReservationQuery _repositoryReservationQuery;
        private readonly IRepositorySeatQuery _repositorySeatQuery;
        private readonly IRepositoryReservationCommand _repositoryReservationCommand;
        private readonly IRepositorySeatCommand _repositorySeatCommand;
        private readonly IRepositoryAuditLogCommand _repositoryAuditLogCommand;
        private readonly IApplicationUnitOfWork _unitOfWork;
        private readonly IReservationExpirationService _reservationExpirationService;
        private readonly IMapper _mapper;

        public ConfirmReservationPaymentHandler(
            IRepositoryReservationQuery repositoryReservationQuery,
            IRepositorySeatQuery repositorySeatQuery,
            IRepositoryReservationCommand repositoryReservationCommand,
            IRepositorySeatCommand repositorySeatCommand,
            IRepositoryAuditLogCommand repositoryAuditLogCommand,
            IApplicationUnitOfWork unitOfWork,
            IReservationExpirationService reservationExpirationService,
            IMapper mapper)
        {
            _repositoryReservationQuery = repositoryReservationQuery;
            _repositorySeatQuery = repositorySeatQuery;
            _repositoryReservationCommand = repositoryReservationCommand;
            _repositorySeatCommand = repositorySeatCommand;
            _repositoryAuditLogCommand = repositoryAuditLogCommand;
            _unitOfWork = unitOfWork;
            _reservationExpirationService = reservationExpirationService;
            _mapper = mapper;
        }

        public async Task<ReservationGetDTO> Handle(ConfirmReservationPaymentCommand command)
        {
            await _reservationExpirationService.ExpirePendingReservations();

            var reservation = await GetReservationOrThrow(command.ReservationId);

            await ValidateUserCanPayReservation(command, reservation);
            var seat = await GetSeatOrThrow(reservation.SeatId);
            await ValidateReservationIsPending(command, reservation, seat);
            await ValidateSeatIsReserved(reservation, seat);

            MarkReservationAsPaid(reservation);
            MarkSeatAsSold(seat);

            await _unitOfWork.BeginTransactionAsync();

            try
            {
                await PersistPaymentConfirmation(reservation, seat);
                await CreateAuditLog(reservation, seat);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }

            return _mapper.Map<ReservationGetDTO>(reservation);
        }

        private async Task<Reservation> GetReservationOrThrow(Guid reservationId) => await _repositoryReservationQuery.GetById(reservationId)
            ?? throw new KeyNotFoundException("Reserva no encontrada.");

        private async Task ValidateUserCanPayReservation(ConfirmReservationPaymentCommand command, Reservation reservation)
        {
            if (!command.IsAdmin && reservation.UserId != command.CurrentUserId)
            {
                await CreateRejectedPaymentAuditLog(
                    reservation.UserId,
                    reservation.Id.ToString(),
                    $"Intento de pago rechazado por permisos. ReservationId={reservation.Id}, CurrentUserId={command.CurrentUserId}, ReservationUserId={reservation.UserId}");
                throw new ForbiddenAccessException("No tiene permisos para pagar esta reserva.");
            }
        }

        private async Task ValidateReservationIsPending(ConfirmReservationPaymentCommand command, Reservation reservation, Seat seat)
        {
            if (reservation.Status == ReservationStatuses.Pending && reservation.ExpiresAt <= DateTime.UtcNow)
            {
                reservation.Status = ReservationStatuses.Expired;

                if (seat.Status == SeatStatuses.Reserved)
                {
                    seat.Status = SeatStatuses.Available;
                    seat.Version += 1;
                    await _repositorySeatCommand.Update(seat);
                }

                await _repositoryReservationCommand.Update(reservation);
                await CreateRejectedPaymentAuditLog(
                    command.CurrentUserId,
                    reservation.Id.ToString(),
                    $"Intento de pago rechazado por expiración. ReservationId={reservation.Id}, ExpiresAt={reservation.ExpiresAt:O}, CurrentTime={DateTime.UtcNow:O}");
                throw new InvalidOperationException("La reserva expiró. Selecciona otra butaca.");
            }

            if (reservation.Status != ReservationStatuses.Pending)
            {
                await CreateRejectedPaymentAuditLog(
                    reservation.UserId,
                    reservation.Id.ToString(),
                    $"Intento de pago rechazado por estado de reserva. ReservationId={reservation.Id}, ReservationStatus={reservation.Status}");
                throw new InvalidOperationException("La reserva no se encuentra pendiente de pago.");
            }
        }

        private async Task<Seat> GetSeatOrThrow(Guid seatId) => await _repositorySeatQuery.GetById(seatId)
            ?? throw new KeyNotFoundException("Butaca no encontrada.");

        private async Task ValidateSeatIsReserved(Reservation reservation, Seat seat)
        {
            if (seat.Status != SeatStatuses.Reserved)
            {
                await CreateRejectedPaymentAuditLog(
                    reservation.UserId,
                    reservation.Id.ToString(),
                    $"Intento de pago rechazado por estado de butaca. ReservationId={reservation.Id}, SeatId={seat.Id}, SeatStatus={seat.Status}");
                throw new InvalidOperationException("La butaca no se encuentra reservada.");
            }
        }

        private static void MarkReservationAsPaid(Reservation reservation)
        {
            reservation.Status = ReservationStatuses.Paid;
        }

        private static void MarkSeatAsSold(Seat seat)
        {
            seat.Status = SeatStatuses.Sold;
            seat.Version += 1;
        }

        private async Task PersistPaymentConfirmation(Reservation reservation, Seat seat)
        {
            await _repositoryReservationCommand.Update(reservation);
            await _repositorySeatCommand.Update(seat);
        }

        private async Task CreateAuditLog(Reservation reservation, Seat seat)
        {
            var auditLog = AuditLogFactory.Create(
                reservation.UserId,
                AuditActions.ConfirmReservationPayment,
                AuditEntityTypes.Reservation,
                reservation.Id.ToString(),
                $"Pago confirmado. ReservationId={reservation.Id}, SeatId={seat.Id}, UserId={reservation.UserId}, ReservationStatus={reservation.Status}, SeatStatus={seat.Status}");

            await _repositoryAuditLogCommand.Create(auditLog);
        }

        private async Task CreateRejectedPaymentAuditLog(int userId, string reservationId, string details)
        {
            var auditLog = AuditLogFactory.Create(
                userId,
                AuditActions.ConfirmReservationPaymentRejected,
                AuditEntityTypes.Reservation,
                reservationId,
                details);

            await _repositoryAuditLogCommand.Create(auditLog);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
