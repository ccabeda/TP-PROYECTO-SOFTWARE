using AutoMapper;
using TP_PROYECTO_SOFTWARE.Aplication.DTOs.ReservationDTOs;
using TP_PROYECTO_SOFTWARE.Aplication.IHandlers;
using TP_PROYECTO_SOFTWARE.Aplication.IRepository.IQuery;
using TP_PROYECTO_SOFTWARE.Aplication.IUnitOfWork;
using TP_PROYECTO_SOFTWARE.Aplication.UseCases.AuditLogs.Commands;
using TP_PROYECTO_SOFTWARE.Aplication.UseCases.Reservations.Commands;
using TP_PROYECTO_SOFTWARE.Domain.Models;

namespace TP_PROYECTO_SOFTWARE.Aplication.UseCases.Reservations.Handlers
{
    public class CreateReservationHandler : ICreateReservationHandler
    {
        private readonly IRepositorySeatQuery _repositorySeatQuery;
        private readonly IRepositoryReservationQuery _repositoryReservationQuery;
        private readonly IRepositoryUserQuery _repositoryUserQuery;
        private readonly IUnitOfWorkReservationCommand _unitOfWorkReservationCommand;
        private readonly ICreateAuditLogHandler _createAuditLogHandler;
        private readonly IMapper _mapper;

        public CreateReservationHandler(
            IRepositorySeatQuery repositorySeatQuery,
            IRepositoryReservationQuery repositoryReservationQuery,
            IRepositoryUserQuery repositoryUserQuery,
            IUnitOfWorkReservationCommand unitOfWorkReservationCommand,
            ICreateAuditLogHandler createAuditLogHandler,
            IMapper mapper)
        {
            _repositorySeatQuery = repositorySeatQuery;
            _repositoryReservationQuery = repositoryReservationQuery;
            _repositoryUserQuery = repositoryUserQuery;
            _unitOfWorkReservationCommand = unitOfWorkReservationCommand;
            _createAuditLogHandler = createAuditLogHandler;
            _mapper = mapper;
        }

        public async Task<ReservationGetDTO> Handle(CreateReservationCommand command)
        {
            var user = await GetUserOrThrow(command.CurrentUserId);
            var seat = await GetSeatOrThrow(command.SeatId);

            EnsureEventHasNotStarted(seat);
            await EnsureSeatIsAvailable(user.Id, command.SeatId, seat);

            MarkSeatAsReserved(seat);
            var reservation = BuildReservation(user.Id, seat.Id);

            await PersistReservation(seat, reservation);
            await CreateAuditLog(user.Id, seat.Id, reservation);
            await _unitOfWorkReservationCommand.Save();

            return _mapper.Map<ReservationGetDTO>(reservation);
        }

        private async Task<USER> GetUserOrThrow(int userId) => await _repositoryUserQuery.GetById(userId)
            ?? throw new KeyNotFoundException("Usuario no encontrado.");

        private async Task<SEAT> GetSeatOrThrow(Guid seatId) => await _repositorySeatQuery.GetById(seatId)
            ?? throw new KeyNotFoundException("Butaca no encontrada.");

        private static void EnsureEventHasNotStarted(SEAT seat)
        {
            if (seat.Sector.Event.EventDate < DateTime.UtcNow)
            {
                throw new InvalidOperationException("No se pueden reservar butacas de eventos ya finalizados.");
            }
        }

        private async Task EnsureSeatIsAvailable(int userId, Guid seatId, SEAT seat)
        {
            var activeReservation = await _repositoryReservationQuery.GetActiveBySeatId(seatId);
            if (seat.Status != "Available" || activeReservation is not null)
            {
                await CreateRejectedReservationAuditLog(userId, seat, activeReservation is not null);
                throw new InvalidOperationException("La butaca no se encuentra disponible.");
            }
        }

        private static void MarkSeatAsReserved(SEAT seat)
        {
            seat.Status = "Reserved";
            seat.Version += 1;
        }

        private static RESERVATION BuildReservation(int userId, Guid seatId) => new()
        {
            UserId = userId,
            SeatId = seatId,
            Status = "Pending",
            ReservedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddMinutes(5)
        };

        private async Task PersistReservation(SEAT seat, RESERVATION reservation)
        {
            await _unitOfWorkReservationCommand.RepositorySeatCommand.Update(seat);
            await _unitOfWorkReservationCommand.RepositoryReservationCommand.Create(reservation);
        }

        private async Task CreateAuditLog(int userId, Guid seatId, RESERVATION reservation)
        {
            await _createAuditLogHandler.Handle(new CreateAuditLogCommand
            {
                UserId = userId,
                Action = "CreateReservation",
                EntityType = "RESERVATION",
                EntityId = reservation.Id.ToString(),
                Details = $"Reserva creada. SeatId={seatId}, UserId={userId}, Status={reservation.Status}, ExpiresAt={reservation.ExpiresAt:O}"
            });
        }

        private async Task CreateRejectedReservationAuditLog(int userId, SEAT seat, bool hasActiveReservation)
        {
            await _createAuditLogHandler.Handle(new CreateAuditLogCommand
            {
                UserId = userId,
                Action = "CreateReservationRejected",
                EntityType = "SEAT",
                EntityId = seat.Id.ToString(),
                Details = $"Intento de reserva rechazado. UserId={userId}, SeatId={seat.Id}, Status={seat.Status}, HasActiveReservation={hasActiveReservation}"
            });
        }
    }
}
