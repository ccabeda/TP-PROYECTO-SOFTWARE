namespace TP_PROYECTO_SOFTWARE.Aplication.UseCases.Events.Queries
{
    public record GetEventsQuery
    {
        public string? Name { get; set; }
        public DateTime? EventDate { get; set; }
    }
}
