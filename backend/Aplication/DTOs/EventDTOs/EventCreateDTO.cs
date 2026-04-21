namespace TP_PROYECTO_SOFTWARE.Aplication.DTOs.EventDTOs
{
    public record EventCreateDTO
    {
        public string Name { get; set; } = string.Empty;
        public DateTime EventDate { get; set; }
        public string Venue { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }
}
