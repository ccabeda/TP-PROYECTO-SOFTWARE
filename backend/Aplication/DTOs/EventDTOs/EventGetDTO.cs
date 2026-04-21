namespace TP_PROYECTO_SOFTWARE.Aplication.DTOs.EventDTOs
{
    public record EventGetDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public DateTime EventDate { get; set; }
        public string? Venue { get; set; }
        public string? Status { get; set; }
    }
}
