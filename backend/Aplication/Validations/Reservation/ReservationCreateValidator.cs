using FluentValidation;
using TP_PROYECTO_SOFTWARE.Aplication.DTOs.PaymentDTOs;
using TP_PROYECTO_SOFTWARE.Aplication.DTOs.ReservationDTOs;

namespace TP_PROYECTO_SOFTWARE.Aplication.Validations.Reservation
{
    public class ReservationCreateValidator : AbstractValidator<ReservationCreateDTO>
    {
        public ReservationCreateValidator()
        {
            RuleFor(x => x.SeatId)
                .NotEqual(Guid.Empty)
                .WithMessage("El SeatId no puede ser vacío.");
        }
    }

    public class PaymentCreateValidator : AbstractValidator<PaymentCreateDTO>
    {
        public PaymentCreateValidator()
        {
            RuleFor(x => x.ReservationId)
                .NotEqual(Guid.Empty)
                .WithMessage("El ReservationId no puede ser vacío.");
        }
    }
}
