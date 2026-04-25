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
            var eventEntity = await GetEventOrThrow(command.EventId);

            await EnsureEventHasNoReservations(command.EventId);

            var sectors = await GetSectorsByEvent(command.EventId);
            var seats = await GetSeatsByEvent(command.EventId);

            await DeleteSeatsIfAny(seats);
            await DeleteSectorsIfAny(sectors);
            await CreateDeleteAuditLog(command.UserId, eventEntity, sectors.Count, seats.Count);

            await _repositoryEventCommand.Delete(eventEntity);
            await _repositoryEventCommand.Save();
        }

        private async Task<Domain.Models.Event> GetEventOrThrow(int eventId) => await _repositoryEventQuery.GetById(eventId)
            ?? throw new KeyNotFoundException("Evento no encontrado.");

        private async Task EnsureEventHasNoReservations(int eventId)
        {
            var hasReservations = await _repositoryReservationQuery.AnyByEventId(eventId);
            if (hasReservations)
            {
                throw new InvalidOperationException("No se puede eliminar el evento porque tiene reservas asociadas.");
            }
        }

        private async Task<List<Domain.Models.Sector>> GetSectorsByEvent(int eventId) => await _repositorySectorQuery.GetByEventId(eventId);

        private async Task<List<Domain.Models.Seat>> GetSeatsByEvent(int eventId) => await _repositorySeatQuery.GetByEventId(eventId);

        private async Task DeleteSeatsIfAny(List<Domain.Models.Seat> seats)
        {
            if (seats.Count > 0)
            {
                await _repositorySeatCommand.DeleteRange(seats);
            }
        }

        private async Task DeleteSectorsIfAny(List<Domain.Models.Sector> sectors)
        {
            if (sectors.Count > 0)
            {
                await _repositorySectorCommand.DeleteRange(sectors);
            }
        }

        private async Task CreateDeleteAuditLog(int? userId, Domain.Models.Event eventEntity, int deletedSectorsCount, int deletedSeatsCount)
        {
            await _createAuditLogHandler.Handle(new CreateAuditLogCommand
            {
                UserId = userId,
                Action = "DeleteEvent",
                EntityType = "Event",
                EntityId = eventEntity.Id.ToString(),
                Details = $"Evento eliminado. Name={eventEntity.Name}, Venue={eventEntity.Venue}, EventDate={eventEntity.EventDate:O}, DeletedSectors={deletedSectorsCount}, DeletedSeats={deletedSeatsCount}"
            });
        }
    }
}
