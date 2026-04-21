namespace TP_PROYECTO_SOFTWARE.Aplication.UseCases.Events.Commands
{
    public record DeleteEventCommand
    {
        public int? UserId { get; set; }
        public int EventId { get; set; }
    }
}
