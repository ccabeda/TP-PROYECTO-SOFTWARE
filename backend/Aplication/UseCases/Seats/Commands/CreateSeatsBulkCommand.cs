namespace TP_PROYECTO_SOFTWARE.Aplication.UseCases.Seats.Commands
{
    public record CreateSeatsBulkCommand
    {
        public int? UserId { get; set; }
        public int SectorId { get; set; }
        public int RowCount { get; set; }
        public int SeatsPerRow { get; set; }
    }
}
