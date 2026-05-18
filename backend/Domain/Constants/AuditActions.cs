namespace TP_PROYECTO_SOFTWARE.Domain.Constants;

public static class AuditActions
{
    public const string CreateUser = "CreateUser";
    public const string LoginUser = "LoginUser";
    public const string LoginUserRejected = "LoginUserRejected";
    public const string CreateEvent = "CreateEvent";
    public const string DeleteEvent = "DeleteEvent";
    public const string CreateSector = "CreateSector";
    public const string DeleteSector = "DeleteSector";
    public const string CreateSeat = "CreateSeat";
    public const string CreateSeatsBulk = "CreateSeatsBulk";
    public const string DeleteSeat = "DeleteSeat";
    public const string CreateReservation = "CreateReservation";
    public const string CreateReservationRejected = "CreateReservationRejected";
    public const string CreateReservationRejectedConcurrency = "CreateReservationRejectedConcurrency";
    public const string ConfirmReservationPayment = "ConfirmReservationPayment";
    public const string ConfirmReservationPaymentRejected = "ConfirmReservationPaymentRejected";
    public const string ExpireReservation = "ExpireReservation";
}
