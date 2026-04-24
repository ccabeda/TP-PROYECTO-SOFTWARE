using AutoMapper;
using TP_PROYECTO_SOFTWARE.Aplication.Exceptions;
using TP_PROYECTO_SOFTWARE.Aplication.DTOs.ReservationDTOs;
using TP_PROYECTO_SOFTWARE.Aplication.IHandlers;
using TP_PROYECTO_SOFTWARE.Aplication.IRepository.IQuery;
using TP_PROYECTO_SOFTWARE.Aplication.IUnitOfWork;
using TP_PROYECTO_SOFTWARE.Aplication.UseCases.AuditLogs.Commands;
using TP_PROYECTO_SOFTWARE.Aplication.UseCases.Reservations.Commands;

namespace TP_PROYECTO_SOFTWARE.Aplication.UseCases.Reservations.Handlers
{
    public class ConfirmReservationPaymentHandler : IConfirmReservationPaymentHandler
    {
        private readonly IRepositoryReservationQuery _repositoryReservationQuery;
        private readonly IRepositorySeatQuery _repositorySeatQuery;
        private readonly IUnitOfWorkReservationCommand _unitOfWorkReservationCommand;
        private readonly ICreateAuditLogHandler _createAuditLogHandler;
        private readonly IMapper _mapper;

        public ConfirmReservationPaymentHandler(
            IRepositoryReservationQuery repositoryReservationQuery,
            IRepositorySeatQuery repositorySeatQuery,
            IUnitOfWorkReservationCommand unitOfWorkReservationCommand,
            ICreateAuditLogHandler createAuditLogHandler,
            IMapper mapper)
        {
            _repositoryReservationQuery = repositoryReservationQuery;
            _repositorySeatQuery = repositorySeatQuery;
            _unitOfWorkReservationCommand = unitOfWorkReservationCommand;
            _createAuditLogHandler = createAuditLogHandler;
            _mapper = mapper;
        }

        public async Task<ReservationGetDTO> Handle(ConfirmReservationPaymentCommand command)
        {
            var reservation = await GetReservationOrThrow(command.ReservationId);

            await ValidateUserCanPayReservation(command, reservation);
            await ValidateReservationIsPending(reservation);

            var seat = await GetSeatOrThrow(reservation.SeatId);
            await ValidateSeatIsReserved(reservation, seat);

            MarkReservationAsPaid(reservation);
            MarkSeatAsSold(seat);

            await PersistPaymentConfirmation(reservation, seat);
            await CreateAuditLog(reservation, seat);
            await _unitOfWorkReservationCommand.Save();

            return _mapper.Map<ReservationGetDTO>(reservation);
        }

        private async Task<Domain.Models.RESERVATION> GetReservationOrThrow(Guid reservationId) => await _repositoryReservationQuery.GetById(reservationId)
            ?? throw new KeyNotFoundException("Reserva no encontrada.");

        private async Task ValidateUserCanPayReservation(ConfirmReservationPaymentCommand command, Domain.Models.RESERVATION reservation)
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

        private async Task ValidateReservationIsPending(Domain.Models.RESERVATION reservation)  //esto es mas para la segunda entrega, ya que aun no se implemento la expiracion de reservas.
        {
            if (reservation.Status != "Pending")
            {
                await CreateRejectedPaymentAuditLog(
                    reservation.UserId,
                    reservation.Id.ToString(),
                    $"Intento de pago rechazado por estado de reserva. ReservationId={reservation.Id}, ReservationStatus={reservation.Status}");
                throw new InvalidOperationException("La reserva no se encuentra pendiente de pago.");
            }
        }

        private async Task<Domain.Models.SEAT> GetSeatOrThrow(Guid seatId) => await _repositorySeatQuery.GetById(seatId)
            ?? throw new KeyNotFoundException("Butaca no encontrada.");

        private async Task ValidateSeatIsReserved(Domain.Models.RESERVATION reservation, Domain.Models.SEAT seat)
        {
            if (seat.Status != "Reserved")
            {
                await CreateRejectedPaymentAuditLog(
                    reservation.UserId,
                    reservation.Id.ToString(),
                    $"Intento de pago rechazado por estado de butaca. ReservationId={reservation.Id}, SeatId={seat.Id}, SeatStatus={seat.Status}");
                throw new InvalidOperationException("La butaca no se encuentra reservada.");
            }
        }

        private static void MarkReservationAsPaid(Domain.Models.RESERVATION reservation)
        {
            reservation.Status = "Paid";
        }

        private static void MarkSeatAsSold(Domain.Models.SEAT seat)
        {
            seat.Status = "Sold";
            seat.Version += 1;
        }

        private async Task PersistPaymentConfirmation(Domain.Models.RESERVATION reservation, Domain.Models.SEAT seat)
        {
            await _unitOfWorkReservationCommand.RepositoryReservationCommand.Update(reservation);
            await _unitOfWorkReservationCommand.RepositorySeatCommand.Update(seat);
        }

        private async Task CreateAuditLog(Domain.Models.RESERVATION reservation, Domain.Models.SEAT seat)
        {
            await _createAuditLogHandler.Handle(new CreateAuditLogCommand
            {
                UserId = reservation.UserId,
                Action = "ConfirmReservationPayment",
                EntityType = "RESERVATION",
                EntityId = reservation.Id.ToString(),
                Details = $"Pago confirmado. ReservationId={reservation.Id}, SeatId={seat.Id}, UserId={reservation.UserId}, ReservationStatus={reservation.Status}, SeatStatus={seat.Status}"
            });
        }

        private async Task CreateRejectedPaymentAuditLog(int userId, string reservationId, string details)
        {
            await _createAuditLogHandler.Handle(new CreateAuditLogCommand
            {
                UserId = userId,
                Action = "ConfirmReservationPaymentRejected",
                EntityType = "RESERVATION",
                EntityId = reservationId,
                Details = details
            });
        }
    }
}
