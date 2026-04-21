namespace TP_PROYECTO_SOFTWARE.Aplication.UseCases.Sectors.Commands
{
    public record CreateSectorCommand
    {
        public int? UserId { get; set; }
        public int EventId { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Capacity { get; set; }
    }
}
