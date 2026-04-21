namespace TP_PROYECTO_SOFTWARE.Aplication.UseCases.Seats.Commands
{
    public record DeleteSeatCommand
    {
        public int? UserId { get; set; }
        public int SectorId { get; set; }
        public Guid SeatId { get; set; }
    }
}
