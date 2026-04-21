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
            var sector = await _repositorySectorQuery.GetById(command.SectorId)
                ?? throw new KeyNotFoundException("Sector no encontrado.");

            if (sector.EventId != command.EventId)
            {
                throw new KeyNotFoundException("Sector no encontrado para el evento indicado.");
            }

            var seats = await _repositorySeatQuery.GetBySectorId(sector.Id);
            var reservations = await _repositoryReservationQuery.GetBySeatIds(seats.Select(s => s.Id).ToList());
            if (reservations.Count > 0)
            {
                throw new InvalidOperationException("No se puede eliminar el sector porque tiene reservas asociadas.");
            }

            if (seats.Count > 0)
            {
                await _repositorySeatCommand.DeleteRange(seats);
            }

            await _createAuditLogHandler.Handle(new CreateAuditLogCommand
            {
                UserId = command.UserId,
                Action = "DeleteSector",
                EntityType = "SECTOR",
                EntityId = sector.Id.ToString(),
                Details = $"Sector eliminado. EventId={sector.EventId}, Name={sector.Name}, Price={sector.Price}, Capacity={sector.Capacity}, DeletedSeats={seats.Count}"
            });
            await _repositorySectorCommand.Delete(sector);
            await _repositorySectorCommand.Save();
        }
    }
}
