using TP_PROYECTO_SOFTWARE.Aplication.IHandlers;
using TP_PROYECTO_SOFTWARE.Aplication.IRepository.ICommand;
using TP_PROYECTO_SOFTWARE.Aplication.IRepository.IQuery;
using TP_PROYECTO_SOFTWARE.Aplication.UseCases.AuditLogs.Commands;
using TP_PROYECTO_SOFTWARE.Aplication.UseCases.Seats.Commands;

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
            var seat = await _repositorySeatQuery.GetById(command.SeatId)
                ?? throw new KeyNotFoundException("Butaca no encontrada.");

            if (seat.SectorId != command.SectorId)
            {
                throw new KeyNotFoundException("Butaca no encontrada para el sector indicado.");
            }

            var reservations = await _repositoryReservationQuery.GetBySeatIds(new List<Guid> { seat.Id });
            if (reservations.Count > 0)
            {
                throw new InvalidOperationException("No se puede eliminar la butaca porque tiene reservas asociadas.");
            }

            await _createAuditLogHandler.Handle(new CreateAuditLogCommand
            {
                UserId = command.UserId,
                Action = "DeleteSeat",
                EntityType = "SEAT",
                EntityId = seat.Id.ToString(),
                Details = $"Butaca eliminada. SectorId={seat.SectorId}, RowIdentifier={seat.RowIdentifier}, SeatNumber={seat.SeatNumber}, Status={seat.Status}"
            });
            await _repositorySeatCommand.Delete(seat);
            await _repositorySeatCommand.Save();
        }
    }
}
