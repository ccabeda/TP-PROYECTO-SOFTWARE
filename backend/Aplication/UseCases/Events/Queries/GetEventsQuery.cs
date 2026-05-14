namespace TP_PROYECTO_SOFTWARE.Aplication.UseCases.Events.Queries
{
    public record GetEventsQuery
    {
        public string? Name { get; set; }
        public DateTime? EventDate { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 12;
    }
}
