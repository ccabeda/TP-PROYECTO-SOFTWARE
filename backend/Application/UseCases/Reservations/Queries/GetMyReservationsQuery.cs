namespace TP_PROYECTO_SOFTWARE.Application.UseCases.Reservations.Queries
{
    public record GetMyReservationsQuery
    {
        public int CurrentUserId { get; set; }
    }
}
