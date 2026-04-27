namespace TP_PROYECTO_SOFTWARE.Aplication.DTOs.ReservationDTOs
{
    public record ReservationTicketGetDTO
    {
        public Guid Id { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime ReservedAt { get; set; }
        public int EventId { get; set; }
        public string EventName { get; set; } = string.Empty;
        public DateTime EventDate { get; set; }
        public string Venue { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
        public string SectorName { get; set; } = string.Empty;
        public decimal SectorPrice { get; set; }
        public string RowIdentifier { get; set; } = string.Empty;
        public int SeatNumber { get; set; }
    }
}
