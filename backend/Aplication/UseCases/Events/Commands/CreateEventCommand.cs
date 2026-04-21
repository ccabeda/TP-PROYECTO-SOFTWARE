namespace TP_PROYECTO_SOFTWARE.Aplication.UseCases.Events.Commands
{
    public record CreateEventCommand
    {
        public int? UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime EventDate { get; set; }
        public string Venue { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }
}
