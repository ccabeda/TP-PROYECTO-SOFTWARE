using AutoMapper;
using TP_PROYECTO_SOFTWARE.Aplication.DTOs.ReservationDTOs;
using TP_PROYECTO_SOFTWARE.Aplication.IHandlers;
using TP_PROYECTO_SOFTWARE.Aplication.IRepository.ICommand;
using TP_PROYECTO_SOFTWARE.Aplication.IRepository.IQuery;
using TP_PROYECTO_SOFTWARE.Aplication.IUnitOfWork;
using TP_PROYECTO_SOFTWARE.Aplication.Services.Reservations;
using TP_PROYECTO_SOFTWARE.Aplication.UseCases.AuditLogs.Commands;
using TP_PROYECTO_SOFTWARE.Aplication.UseCases.Reservations.Commands;
using TP_PROYECTO_SOFTWARE.Domain.Constants;
using TP_PROYECTO_SOFTWARE.Domain.Factories;
using TP_PROYECTO_SOFTWARE.Domain.Models;

namespace TP_PROYECTO_SOFTWARE.Aplication.UseCases.Reservations.Handlers
{
    public class CreateReservationHandler : ICreateReservationHandler
    {
        private readonly IRepositorySeatQuery _repositorySeatQuery;
        private readonly IRepositoryReservationQuery _repositoryReservationQuery;
        private readonly IRepositoryUserQuery _repositoryUserQuery;
        private readonly IRepositoryReservationCommand _repositoryReservationCommand;
        private readonly IRepositorySeatCommand _repositorySeatCommand;
        private readonly IRepositoryAuditLogCommand _repositoryAuditLogCommand;
        private readonly IApplicationUnitOfWork _unitOfWork;
        private readonly ICreateAuditLogHandler _createAuditLogHandler;
        private readonly IReservationExpirationService _reservationExpirationService;
        private readonly IMapper _mapper;

        public CreateReservationHandler(
            IRepositorySeatQuery repositorySeatQuery,
            IRepositoryReservationQuery repositoryReservationQuery,
            IRepositoryUserQuery repositoryUserQuery,
            IRepositoryReservationCommand repositoryReservationCommand,
            IRepositorySeatCommand repositorySeatCommand,
            IRepositoryAuditLogCommand repositoryAuditLogCommand,
            IApplicationUnitOfWork unitOfWork,
            ICreateAuditLogHandler createAuditLogHandler,
            IReservationExpirationService reservationExpirationService,
            IMapper mapper)
        {
            _repositorySeatQuery = repositorySeatQuery;
            _repositoryReservationQuery = repositoryReservationQuery;
            _repositoryUserQuery = repositoryUserQuery;
            _repositoryReservationCommand = repositoryReservationCommand;
            _repositorySeatCommand = repositorySeatCommand;
            _repositoryAuditLogCommand = repositoryAuditLogCommand;
            _unitOfWork = unitOfWork;
            _createAuditLogHandler = createAuditLogHandler;
            _reservationExpirationService = reservationExpirationService;
            _mapper = mapper;
        }

        public async Task<ReservationGetDTO> Handle(CreateReservationCommand command)
        {
            await _reservationExpirationService.ExpirePendingReservations();

            var user = await GetUserOrThrow(command.CurrentUserId);
            var seat = await GetSeatOrThrow(command.SeatId);

            EnsureEventHasNotStarted(seat);
            await EnsureSeatIsAvailable(user.Id, command.SeatId, seat);

            MarkSeatAsReserved(seat);
            var reservation = BuildReservation(user.Id, seat.Id);

            try
            {
                await PersistReservation(seat, reservation);
                await CreateAuditLog(user.Id, seat.Id, reservation);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex) when (ex.GetType().Name == "DbUpdateConcurrencyException")
            {
                await CreateConcurrencyRejectedReservationAuditLog(user.Id, seat.Id);
                throw new InvalidOperationException("La butaca acaba de ser reservada por otro usuario.");
            }

            return _mapper.Map<ReservationGetDTO>(reservation);
        }

        private async Task<User> GetUserOrThrow(int userId) => await _repositoryUserQuery.GetById(userId)
            ?? throw new KeyNotFoundException("Usuario no encontrado.");

        private async Task<Seat> GetSeatOrThrow(Guid seatId) => await _repositorySeatQuery.GetById(seatId)
            ?? throw new KeyNotFoundException("Butaca no encontrada.");

        private static void EnsureEventHasNotStarted(Seat seat)
        {
            if (seat.Sector.Event.EventDate < DateTime.UtcNow)
            {
                throw new InvalidOperationException("No se pueden reservar butacas de eventos ya finalizados.");
            }
        }

        private async Task EnsureSeatIsAvailable(int userId, Guid seatId, Seat seat)
        {
            var activeReservation = await _repositoryReservationQuery.GetActiveBySeatId(seatId);
            if (seat.Status != SeatStatuses.Available || activeReservation is not null)
            {
                await CreateRejectedReservationAuditLog(userId, seat, activeReservation is not null);
                throw new InvalidOperationException("La butaca no se encuentra disponible.");
            }
        }

        private static void MarkSeatAsReserved(Seat seat)
        {
            seat.Status = SeatStatuses.Reserved;
            seat.Version += 1;
        }

        private static Reservation BuildReservation(int userId, Guid seatId)
        {
            var reservedAt = DateTime.UtcNow;

            return new Reservation
            {
                UserId = userId,
                SeatId = seatId,
                Status = ReservationStatuses.Pending,
                ReservedAt = reservedAt,
                ExpiresAt = reservedAt.AddMinutes(5)
            };
        }

        private async Task PersistReservation(Seat seat, Reservation reservation)
        {
            await _repositorySeatCommand.Update(seat);
            await _repositoryReservationCommand.Create(reservation);
        }

        private async Task CreateAuditLog(int userId, Guid seatId, Reservation reservation)
        {
            var auditLog = AuditLogFactory.Create(
                userId,
                AuditActions.CreateReservation,
                AuditEntityTypes.Reservation,
                reservation.Id.ToString(),
                $"Reserva creada. SeatId={seatId}, UserId={userId}, Status={reservation.Status}, ExpiresAt={reservation.ExpiresAt:O}");

            await _repositoryAuditLogCommand.Create(auditLog);
        }

        private async Task CreateRejectedReservationAuditLog(int userId, Seat seat, bool hasActiveReservation)
        {
            await _createAuditLogHandler.Handle(new CreateAuditLogCommand
            {
                UserId = userId,
                Action = AuditActions.CreateReservationRejected,
                EntityType = AuditEntityTypes.Seat,
                EntityId = seat.Id.ToString(),
                Details = $"Intento de reserva rechazado. UserId={userId}, SeatId={seat.Id}, Status={seat.Status}, HasActiveReservation={hasActiveReservation}"
            });
        }

        private async Task CreateConcurrencyRejectedReservationAuditLog(int userId, Guid seatId)
        {
            await _createAuditLogHandler.Handle(new CreateAuditLogCommand
            {
                UserId = userId,
                Action = AuditActions.CreateReservationRejectedConcurrency,
                EntityType = AuditEntityTypes.Seat,
                EntityId = seatId.ToString(),
                Details = $"Intento de reserva rechazado por concurrencia. UserId={userId}, SeatId={seatId}"
            });
        }
    }
}
