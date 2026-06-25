namespace TP_PROYECTO_SOFTWARE.Application.Services.Reservations
{
    public interface IReservationExpirationService
    {
        Task ExpirePendingReservations();
    }
}
