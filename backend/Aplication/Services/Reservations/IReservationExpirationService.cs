namespace TP_PROYECTO_SOFTWARE.Aplication.Services.Reservations
{
    public interface IReservationExpirationService
    {
        Task ExpirePendingReservations();
    }
}
