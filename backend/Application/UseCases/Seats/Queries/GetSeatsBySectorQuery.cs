namespace TP_PROYECTO_SOFTWARE.Application.UseCases.Seats.Queries
{
    public record GetSeatsBySectorQuery
    {
        public int SectorId { get; set; }
        public int? CurrentUserId { get; set; }
    }
}
