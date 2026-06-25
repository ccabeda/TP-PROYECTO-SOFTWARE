namespace TP_PROYECTO_SOFTWARE.Application.UseCases.Reservations.Queries
{
    public record GetReservationByIdQuery
    {
        public Guid Id { get; set; }
        public int CurrentUserId { get; set; }
        public bool IsAdmin { get; set; }
    }
}
