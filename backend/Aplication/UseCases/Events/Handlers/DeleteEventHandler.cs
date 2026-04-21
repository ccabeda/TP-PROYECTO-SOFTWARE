using TP_PROYECTO_SOFTWARE.Aplication.IHandlers;
using TP_PROYECTO_SOFTWARE.Aplication.IRepository.ICommand;
using TP_PROYECTO_SOFTWARE.Aplication.IRepository.IQuery;
using TP_PROYECTO_SOFTWARE.Aplication.UseCases.AuditLogs.Commands;
using TP_PROYECTO_SOFTWARE.Aplication.UseCases.Events.Commands;

namespace TP_PROYECTO_SOFTWARE.Aplication.UseCases.Events.Handlers
{
    public class DeleteEventHandler : IDeleteEventHandler
    {
        private readonly IRepositoryEventQuery _repositoryEventQuery;
        private readonly IRepositorySectorQuery _repositorySectorQuery;
        private readonly IRepositorySeatQuery _repositorySeatQuery;
        private readonly IRepositoryReservationQuery _repositoryReservationQuery;
        private readonly IRepositoryEventCommand _repositoryEventCommand;
        private readonly IRepositorySectorCommand _repositorySectorCommand;
        private readonly IRepositorySeatCommand _repositorySeatCommand;
        private readonly ICreateAuditLogHandler _createAuditLogHandler;

        public DeleteEventHandler(
            IRepositoryEventQuery repositoryEventQuery,
            IRepositorySectorQuery repositorySectorQuery,
            IRepositorySeatQuery repositorySeatQuery,
            IRepositoryReservationQuery repositoryReservationQuery,
            IRepositoryEventCommand repositoryEventCommand,
            IRepositorySectorCommand repositorySectorCommand,
            IRepositorySeatCommand repositorySeatCommand,
            ICreateAuditLogHandler createAuditLogHandler)
        {
            _repositoryEventQuery = repositoryEventQuery;
            _repositorySectorQuery = repositorySectorQuery;
            _repositorySeatQuery = repositorySeatQuery;
            _repositoryReservationQuery = repositoryReservationQuery;
            _repositoryEventCommand = repositoryEventCommand;
            _repositorySectorCommand = repositorySectorCommand;
            _repositorySeatCommand = repositorySeatCommand;
            _createAuditLogHandler = createAuditLogHandler;
        }

        public async Task Handle(DeleteEventCommand command)
        {
            var eventEntity = await _repositoryEventQuery.GetById(command.EventId)
                ?? throw new KeyNotFoundException("Evento no encontrado.");

            var sectors = await _repositorySectorQuery.GetByEventId(command.EventId);
            var seats = new List<Domain.Models.SEAT>();

            foreach (var sector in sectors)
            {
                var sectorSeats = await _repositorySeatQuery.GetBySectorId(sector.Id);
                seats.AddRange(sectorSeats);
            }

            var reservations = await _repositoryReservationQuery.GetBySeatIds(seats.Select(s => s.Id).ToList());
            if (reservations.Count > 0)
            {
                throw new InvalidOperationException("No se puede eliminar el evento porque tiene reservas asociadas.");
            }

            if (seats.Count > 0)
            {
                await _repositorySeatCommand.DeleteRange(seats);
            }

            if (sectors.Count > 0)
            {
                await _repositorySectorCommand.DeleteRange(sectors);
            }

            await _createAuditLogHandler.Handle(new CreateAuditLogCommand
            {
                UserId = command.UserId,
                Action = "DeleteEvent",
                EntityType = "EVENT",
                EntityId = eventEntity.Id.ToString(),
                Details = $"Evento eliminado. Name={eventEntity.Name}, Venue={eventEntity.Venue}, EventDate={eventEntity.EventDate:O}, DeletedSectors={sectors.Count}, DeletedSeats={seats.Count}"
            });
            await _repositoryEventCommand.Delete(eventEntity);
            await _repositoryEventCommand.Save();
        }
    }
}
