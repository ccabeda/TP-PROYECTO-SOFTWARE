namespace TP_PROYECTO_SOFTWARE.Aplication.UseCases.Seats.Queries
{
    public record GetSeatByIdQuery
    {
        public int SectorId { get; set; }
        public Guid SeatId { get; set; }
    }
}
