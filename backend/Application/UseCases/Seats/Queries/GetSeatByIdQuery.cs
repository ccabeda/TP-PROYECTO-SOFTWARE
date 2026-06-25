namespace TP_PROYECTO_SOFTWARE.Application.UseCases.Seats.Queries
{
    public record GetSeatByIdQuery
    {
        public int SectorId { get; set; }
        public Guid SeatId { get; set; }
    }
}
