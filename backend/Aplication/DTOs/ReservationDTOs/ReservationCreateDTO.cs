namespace TP_PROYECTO_SOFTWARE.Aplication.DTOs.ReservationDTOs
{
    public record ReservationCreateDTO
    {
        public int UserId { get; set; }
        public Guid SeatId { get; set; }
    }
}
