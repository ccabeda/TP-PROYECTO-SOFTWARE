namespace TP_PROYECTO_SOFTWARE.Aplication.UseCases.Seats.Commands
{
    public record CreateSeatsBulkCommand
    {
        public int? UserId { get; set; }
        public int SectorId { get; set; }
        public List<string> Rows { get; set; } = new();
        public int SeatsPerRow { get; set; }
    }
}
