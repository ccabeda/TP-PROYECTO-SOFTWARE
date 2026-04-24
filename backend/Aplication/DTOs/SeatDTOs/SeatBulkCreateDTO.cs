namespace TP_PROYECTO_SOFTWARE.Aplication.DTOs.SeatDTOs
{
    public record SeatBulkCreateDTO
    {
        public int RowCount { get; set; }
        public int SeatsPerRow { get; set; }
    }
}
