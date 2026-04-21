namespace TP_PROYECTO_SOFTWARE.Aplication.DTOs.SeatDTOs
{
    public record SeatBulkCreateDTO
    {
        public List<string> Rows { get; set; } = new();
        public int SeatsPerRow { get; set; }
    }
}
