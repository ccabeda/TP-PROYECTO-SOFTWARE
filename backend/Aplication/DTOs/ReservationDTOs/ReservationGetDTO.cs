namespace TP_PROYECTO_SOFTWARE.Aplication.DTOs.ReservationDTOs
{
    public record ReservationGetDTO
    {
        public Guid Id { get; set; }
        public int UserId { get; set; }
        public Guid SeatId { get; set; }
        public string? Status { get; set; }
        public DateTime ReservedAt { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
