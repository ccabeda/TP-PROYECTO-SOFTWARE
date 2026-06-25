namespace TP_PROYECTO_SOFTWARE.Application.DTOs.PaymentDTOs
{
    public record PaymentCreateDTO
    {
        public Guid ReservationId { get; set; }
    }
}
