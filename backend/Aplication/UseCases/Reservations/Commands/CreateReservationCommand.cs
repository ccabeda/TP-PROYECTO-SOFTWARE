namespace TP_PROYECTO_SOFTWARE.Aplication.UseCases.Reservations.Commands
{
    public record CreateReservationCommand
    {
        public int UserId { get; set; }
        public Guid SeatId { get; set; }
    }
}
