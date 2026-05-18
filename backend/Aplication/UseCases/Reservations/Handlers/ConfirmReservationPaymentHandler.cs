using AutoMapper;
using TP_PROYECTO_SOFTWARE.Aplication.Exceptions;
using TP_PROYECTO_SOFTWARE.Aplication.DTOs.ReservationDTOs;
using TP_PROYECTO_SOFTWARE.Aplication.IHandlers;
using TP_PROYECTO_SOFTWARE.Aplication.IRepository.IQuery;
using TP_PROYECTO_SOFTWARE.Aplication.IUnitOfWork;
using TP_PROYECTO_SOFTWARE.Aplication.Services.Reservations;
using TP_PROYECTO_SOFTWARE.Aplication.UseCases.AuditLogs.Commands;
using TP_PROYECTO_SOFTWARE.Aplication.UseCases.Reservations.Commands;
using TP_PROYECTO_SOFTWARE.Domain.Constants;
using TP_PROYECTO_SOFTWARE.Domain.Models;

namespace TP_PROYECTO_SOFTWARE.Aplication.UseCases.Reservations.Handlers
{
    public class ConfirmReservationPaymentHandler : IConfirmReservationPaymentHandler
    {
        private readonly IRepositoryReservationQuery _repositoryReservationQuery;
        private readonly IRepositorySeatQuery _repositorySeatQuery;
        private readonly IUnitOfWorkReservationCommand _unitOfWorkReservationCommand;
        private readonly ICreateAuditLogHandler _createAuditLogHandler;
        private readonly IReservationExpirationService _reservationExpirationService;
        private readonly IMapper _mapper;

        public ConfirmReservationPaymentHandler(
            IRepositoryReservationQuery repositoryReservationQuery,
            IRepositorySeatQuery repositorySeatQuery,
            IUnitOfWorkReservationCommand unitOfWorkReservationCommand,
            ICreateAuditLogHandler createAuditLogHandler,
            IReservationExpirationService reservationExpirationService,
            IMapper mapper)
        {
            _repositoryReservationQuery = repositoryReservationQuery;
            _repositorySeatQuery = repositorySeatQuery;
            _unitOfWorkReservationCommand = unitOfWorkReservationCommand;
            _createAuditLogHandler = createAuditLogHandler;
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

            await PersistPaymentConfirmation(reservation, seat);
            await _unitOfWorkReservationCommand.Save();
            await CreateAuditLog(reservation, seat);

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
                    await _unitOfWorkReservationCommand.RepositorySeatCommand.Update(seat);
                }

                await _unitOfWorkReservationCommand.RepositoryReservationCommand.Update(reservation);
                await CreateRejectedPaymentAuditLog(
                    command.CurrentUserId,
                    reservation.Id.ToString(),
                    $"Intento de pago rechazado por expiración. ReservationId={reservation.Id}, ExpiresAt={reservation.ExpiresAt:O}, CurrentTime={DateTime.UtcNow:O}");
                await _unitOfWorkReservationCommand.Save();
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
            await _unitOfWorkReservationCommand.RepositoryReservationCommand.Update(reservation);
            await _unitOfWorkReservationCommand.RepositorySeatCommand.Update(seat);
        }

        private async Task CreateAuditLog(Reservation reservation, Seat seat)
        {
            await _createAuditLogHandler.Handle(new CreateAuditLogCommand
            {
                UserId = reservation.UserId,
                Action = AuditActions.ConfirmReservationPayment,
                EntityType = AuditEntityTypes.Reservation,
                EntityId = reservation.Id.ToString(),
                Details = $"Pago confirmado. ReservationId={reservation.Id}, SeatId={seat.Id}, UserId={reservation.UserId}, ReservationStatus={reservation.Status}, SeatStatus={seat.Status}"
            });
        }

        private async Task CreateRejectedPaymentAuditLog(int userId, string reservationId, string details)
        {
            await _createAuditLogHandler.Handle(new CreateAuditLogCommand
            {
                UserId = userId,
                Action = AuditActions.ConfirmReservationPaymentRejected,
                EntityType = AuditEntityTypes.Reservation,
                EntityId = reservationId,
                Details = details
            });
        }
    }
}
