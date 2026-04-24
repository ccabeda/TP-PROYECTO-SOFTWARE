using TP_PROYECTO_SOFTWARE.Aplication.IHandlers;
using TP_PROYECTO_SOFTWARE.Aplication.IRepository.ICommand;
using TP_PROYECTO_SOFTWARE.Aplication.IRepository.IQuery;
using TP_PROYECTO_SOFTWARE.Aplication.UseCases.AuditLogs.Commands;
using TP_PROYECTO_SOFTWARE.Aplication.UseCases.Seats.Commands;
using TP_PROYECTO_SOFTWARE.Domain.Models;

namespace TP_PROYECTO_SOFTWARE.Aplication.UseCases.Seats.Handlers
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
            await CreateDeleteAuditLog(command.UserId, seat);

            await _repositorySeatCommand.Delete(seat);
            await _repositorySeatCommand.Save();
        }

        private async Task<SEAT> GetSeatOrThrow(Guid seatId) => await _repositorySeatQuery.GetById(seatId)
            ?? throw new KeyNotFoundException("Butaca no encontrada.");

        private static void ValidateSeatBelongsToSector(SEAT seat, int sectorId)
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

        private async Task CreateDeleteAuditLog(int? userId, SEAT seat)
        {
            await _createAuditLogHandler.Handle(new CreateAuditLogCommand
            {
                UserId = userId,
                Action = "DeleteSeat",
                EntityType = "SEAT",
                EntityId = seat.Id.ToString(),
                Details = $"Butaca eliminada. SectorId={seat.SectorId}, RowIdentifier={seat.RowIdentifier}, SeatNumber={seat.SeatNumber}, Status={seat.Status}"
            });
        }
    }
}
