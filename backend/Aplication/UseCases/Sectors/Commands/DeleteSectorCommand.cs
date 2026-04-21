namespace TP_PROYECTO_SOFTWARE.Aplication.UseCases.Sectors.Commands
{
    public record DeleteSectorCommand
    {
        public int? UserId { get; set; }
        public int EventId { get; set; }
        public int SectorId { get; set; }
    }
}
