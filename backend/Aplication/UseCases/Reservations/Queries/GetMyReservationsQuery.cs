namespace TP_PROYECTO_SOFTWARE.Aplication.UseCases.Reservations.Queries
{
    public record GetMyReservationsQuery
    {
        public int CurrentUserId { get; set; }
    }
}
