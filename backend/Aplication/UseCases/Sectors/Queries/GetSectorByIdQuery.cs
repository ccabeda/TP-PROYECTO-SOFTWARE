namespace TP_PROYECTO_SOFTWARE.Aplication.UseCases.Sectors.Queries
{
    public record GetSectorByIdQuery
    {
        public int EventId { get; set; }
        public int SectorId { get; set; }
    }
}
