namespace TP_PROYECTO_SOFTWARE.Application.UseCases.Events.Commands
{
    public record DeleteEventCommand
    {
        public int? UserId { get; set; }
        public int EventId { get; set; }
    }
}
