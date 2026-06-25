namespace TP_PROYECTO_SOFTWARE.Application.DTOs.SeatDTOs
{
    public record SeatBulkCreateDTO
    {
        public int RowCount { get; set; }
        public int SeatsPerRow { get; set; }
    }
}
