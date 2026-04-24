using TP_PROYECTO_SOFTWARE.Aplication.IHandlers;
using TP_PROYECTO_SOFTWARE.Aplication.IRepository.ICommand;
using TP_PROYECTO_SOFTWARE.Aplication.IRepository.IQuery;
using TP_PROYECTO_SOFTWARE.Aplication.UseCases.AuditLogs.Commands;
using TP_PROYECTO_SOFTWARE.Aplication.UseCases.Sectors.Commands;

namespace TP_PROYECTO_SOFTWARE.Aplication.UseCases.Sectors.Handlers
{
    public class DeleteSectorHandler : IDeleteSectorHandler
    {
        private readonly IRepositorySectorQuery _repositorySectorQuery;
        private readonly IRepositorySeatQuery _repositorySeatQuery;
        private readonly IRepositoryReservationQuery _repositoryReservationQuery;
        private readonly IRepositorySectorCommand _repositorySectorCommand;
        private readonly IRepositorySeatCommand _repositorySeatCommand;
        private readonly ICreateAuditLogHandler _createAuditLogHandler;

        public DeleteSectorHandler(
            IRepositorySectorQuery repositorySectorQuery,
            IRepositorySeatQuery repositorySeatQuery,
            IRepositoryReservationQuery repositoryReservationQuery,
            IRepositorySectorCommand repositorySectorCommand,
            IRepositorySeatCommand repositorySeatCommand,
            ICreateAuditLogHandler createAuditLogHandler)
        {
            _repositorySectorQuery = repositorySectorQuery;
            _repositorySeatQuery = repositorySeatQuery;
            _repositoryReservationQuery = repositoryReservationQuery;
            _repositorySectorCommand = repositorySectorCommand;
            _repositorySeatCommand = repositorySeatCommand;
            _createAuditLogHandler = createAuditLogHandler;
        }

        public async Task Handle(DeleteSectorCommand command)
        {
            var sector = await GetSectorOrThrow(command.SectorId);

            ValidateSectorBelongsToEvent(sector, command.EventId);
            await EnsureSectorHasNoReservations(sector.Id);

            var seats = await GetSeatsBySector(sector.Id);
            await DeleteSeatsIfAny(seats);
            await CreateDeleteAuditLog(command.UserId, sector, seats.Count);

            await _repositorySectorCommand.Delete(sector);
            await _repositorySectorCommand.Save();
        }

        private async Task<Domain.Models.SECTOR> GetSectorOrThrow(int sectorId) => await _repositorySectorQuery.GetById(sectorId)
            ?? throw new KeyNotFoundException("Sector no encontrado.");

        private static void ValidateSectorBelongsToEvent(Domain.Models.SECTOR sector, int eventId)
        {
            if (sector.EventId != eventId)
            {
                throw new KeyNotFoundException("Sector no encontrado para el evento indicado.");
            }
        }

        private async Task EnsureSectorHasNoReservations(int sectorId)
        {
            var hasReservations = await _repositoryReservationQuery.AnyBySectorId(sectorId);
            if (hasReservations)
            {
                throw new InvalidOperationException("No se puede eliminar el sector porque tiene reservas asociadas.");
            }
        }

        private async Task<List<Domain.Models.SEAT>> GetSeatsBySector(int sectorId) => await _repositorySeatQuery.GetBySectorId(sectorId);

        private async Task DeleteSeatsIfAny(List<Domain.Models.SEAT> seats)
        {
            if (seats.Count > 0)
            {
                await _repositorySeatCommand.DeleteRange(seats);
            }
        }

        private async Task CreateDeleteAuditLog(int? userId, Domain.Models.SECTOR sector, int deletedSeatsCount)
        {
            await _createAuditLogHandler.Handle(new CreateAuditLogCommand
            {
                UserId = userId,
                Action = "DeleteSector",
                EntityType = "SECTOR",
                EntityId = sector.Id.ToString(),
                Details = $"Sector eliminado. EventId={sector.EventId}, Name={sector.Name}, Price={sector.Price}, Capacity={sector.Capacity}, DeletedSeats={deletedSeatsCount}"
            });
        }
    }
}
