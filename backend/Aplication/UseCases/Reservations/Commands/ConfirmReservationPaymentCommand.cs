namespace TP_PROYECTO_SOFTWARE.Aplication.UseCases.Reservations.Commands
{
    public record ConfirmReservationPaymentCommand
    {
        public Guid ReservationId { get; set; }
    }
}
