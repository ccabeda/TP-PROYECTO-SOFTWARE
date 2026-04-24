using TP_PROYECTO_SOFTWARE.Domain.Models;

namespace TP_PROYECTO_SOFTWARE.Aplication.Services.Seats
{
    public interface ISeatRulesService
    {
        Task<SECTOR> GetSectorOrThrow(int sectorId);
        List<string> GetGeneratedRows(int rowCount);
        string NormalizeAndValidateRow(string rowIdentifier);
        void ValidateSeatNumber(int seatNumber);
        Task ValidateSectorCapacity(int sectorId, int sectorCapacity, int seatsToCreate);
    }
}
