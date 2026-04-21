namespace TP_PROYECTO_SOFTWARE.Aplication.UseCases.Seats.Commands
{
    public record CreateSeatCommand
    {
        public int? UserId { get; set; }
        public int SectorId { get; set; }
        public string RowIdentifier { get; set; } = string.Empty;
        public int SeatNumber { get; set; }
    }
}
