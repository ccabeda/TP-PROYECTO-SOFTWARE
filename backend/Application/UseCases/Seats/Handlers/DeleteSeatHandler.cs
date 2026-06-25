using TP_PROYECTO_SOFTWARE.Application.IHandlers;
using TP_PROYECTO_SOFTWARE.Application.IRepository.ICommand;
using TP_PROYECTO_SOFTWARE.Application.IRepository.IQuery;
using TP_PROYECTO_SOFTWARE.Application.UseCases.AuditLogs.Commands;
using TP_PROYECTO_SOFTWARE.Application.UseCases.Seats.Commands;
using TP_PROYECTO_SOFTWARE.Domain.Constants;
using TP_PROYECTO_SOFTWARE.Domain.Models;

namespace TP_PROYECTO_SOFTWARE.Application.UseCases.Seats.Handlers
{
    public class DeleteSeatHandler : IDeleteSeatHandler
    {
        private readonly IRepositorySeatQuery _repositorySeatQuery;
        private readonly IRepositoryReservationQuery _repositoryReservationQuery;
        private readonly IRepositorySeatCommand _repositorySeatCommand;
        private readonly ICreateAuditLogHandler _createAuditLogHandler;

        public DeleteSeatHandler(
            IRepositorySeatQuery repositorySeatQuery,
            IRepositoryReservationQuery repositoryReservationQuery,
            IRepositorySeatCommand repositorySeatCommand,
            ICreateAuditLogHandler createAuditLogHandler)
        {
            _repositorySeatQuery = repositorySeatQuery;
            _repositoryReservationQuery = repositoryReservationQuery;
            _repositorySeatCommand = repositorySeatCommand;
            _createAuditLogHandler = createAuditLogHandler;
        }

        public async Task Handle(DeleteSeatCommand command)
        {
            var seat = await GetSeatOrThrow(command.SeatId);

            ValidateSeatBelongsToSector(seat, command.SectorId);
            await EnsureSeatHasNoReservations(seat.Id);

            await _repositorySeatCommand.Delete(seat);
            await _repositorySeatCommand.Save();
            await CreateDeleteAuditLog(command.UserId, seat);
        }

        private async Task<Seat> GetSeatOrThrow(Guid seatId) => await _repositorySeatQuery.GetById(seatId)
            ?? throw new KeyNotFoundException("Butaca no encontrada.");

        private static void ValidateSeatBelongsToSector(Seat seat, int sectorId)
        {
            if (seat.SectorId != sectorId)
            {
                throw new KeyNotFoundException("Butaca no encontrada para el sector indicado.");
            }
        }

        private async Task EnsureSeatHasNoReservations(Guid seatId)
        {
            var hasReservations = await _repositoryReservationQuery.AnyBySeatId(seatId);
            if (hasReservations)
            {
                throw new InvalidOperationException("No se puede eliminar la butaca porque tiene reservas asociadas.");
            }
        }

        private async Task CreateDeleteAuditLog(int? userId, Seat seat)
        {
            await _createAuditLogHandler.Handle(new CreateAuditLogCommand
            {
                UserId = userId,
                Action = AuditActions.DeleteSeat,
                EntityType = AuditEntityTypes.Seat,
                EntityId = seat.Id.ToString(),
                Details = $"Butaca eliminada. SectorId={seat.SectorId}, RowIdentifier={seat.RowIdentifier}, SeatNumber={seat.SeatNumber}, Status={seat.Status}"
            });
        }
    }
}
