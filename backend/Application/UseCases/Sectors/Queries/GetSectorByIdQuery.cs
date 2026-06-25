namespace TP_PROYECTO_SOFTWARE.Application.UseCases.Sectors.Queries
{
    public record GetSectorByIdQuery
    {
        public int EventId { get; set; }
        public int SectorId { get; set; }
    }
}
