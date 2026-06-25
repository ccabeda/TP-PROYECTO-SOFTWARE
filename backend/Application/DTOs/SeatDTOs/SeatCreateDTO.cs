namespace TP_PROYECTO_SOFTWARE.Application.DTOs.SeatDTOs
{
    public record SeatCreateDTO
    {
        public string RowIdentifier { get; set; } = string.Empty;
        public int SeatNumber { get; set; }
    }
}
