using Microsoft.Extensions.Options;
using TP_PROYECTO_SOFTWARE.Aplication.Configuration;
using TP_PROYECTO_SOFTWARE.Aplication.IRepository.IQuery;
using TP_PROYECTO_SOFTWARE.Domain.Models;

namespace TP_PROYECTO_SOFTWARE.Aplication.Services.Seats
{
    public class SeatRulesService : ISeatRulesService
    {
        private readonly TicketingRulesOptions _ticketingRules;
        private readonly IRepositorySectorQuery _repositorySectorQuery;
        private readonly IRepositorySeatQuery _repositorySeatQuery;

        public SeatRulesService(
            IOptions<TicketingRulesOptions> ticketingRules,
            IRepositorySectorQuery repositorySectorQuery,
            IRepositorySeatQuery repositorySeatQuery)
        {
            _ticketingRules = ticketingRules.Value;
            _repositorySectorQuery = repositorySectorQuery;
            _repositorySeatQuery = repositorySeatQuery;
        }

        public async Task<SECTOR> GetSectorOrThrow(int sectorId) => await _repositorySectorQuery.GetById(sectorId)
            ?? throw new KeyNotFoundException("Sector no encontrado.");

        public List<string> GetGeneratedRows(int rowCount)
        {
            if (rowCount <= 0 || rowCount > _ticketingRules.MaxRowsPerBulkCreate)
            {
                throw new InvalidOperationException($"La cantidad de filas debe estar entre 1 y {_ticketingRules.MaxRowsPerBulkCreate}.");
            }

            return GetValidRowLabels()
                .Take(rowCount)
                .ToList();
        }

        public string NormalizeAndValidateRow(string rowIdentifier)
        {
            var normalizedRowIdentifier = rowIdentifier.Trim().ToUpperInvariant();
            var validRows = GetValidRowLabels().ToHashSet();

            if (!validRows.Contains(normalizedRowIdentifier))
            {
                throw new InvalidOperationException($"La fila debe ser una de las configuradas: {string.Join(", ", validRows)}.");
            }

            return normalizedRowIdentifier;
        }

        public void ValidateSeatNumber(int seatNumber)
        {
            if (seatNumber <= 0 || seatNumber > _ticketingRules.MaxSeatsPerRow)
            {
                throw new InvalidOperationException($"El número de asiento debe estar entre 1 y {_ticketingRules.MaxSeatsPerRow}.");
            }
        }

        public async Task ValidateSectorCapacity(int sectorId, int sectorCapacity, int seatsToCreate)
        {
            var existingSeatsCount = await _repositorySeatQuery.CountBySectorId(sectorId);
            if (existingSeatsCount + seatsToCreate > sectorCapacity)
            {
                throw new InvalidOperationException("La operación supera la capacidad máxima del sector.");
            }
        }

        private List<string> GetValidRowLabels() => TicketingRulesOptionsHelper.GetValidRowLabels(_ticketingRules);
    }
}
