namespace TP_PROYECTO_SOFTWARE.Application.DTOs.SeatDTOs
{
    public record SeatGetDTO
    {
        public Guid Id { get; set; }
        public int SectorId { get; set; }
        public string? RowIdentifier { get; set; }
        public int SeatNumber { get; set; }
        public string? Status { get; set; }
        public bool ReservedByCurrentUser { get; set; }
        public Guid? ActiveReservationId { get; set; }
        public int Version { get; set; }
    }
}
