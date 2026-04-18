using FluentValidation;
using TP_PROYECTO_SOFTWARE.Aplication.DTOs.ReservationDTOs;

namespace TP_PROYECTO_SOFTWARE.Aplication.Validations.Reservation
{
    public class ReservationCreateValidator : AbstractValidator<ReservationCreateDTO>
    {
        public ReservationCreateValidator()
        {
            RuleFor(x => x.UserId)
                .GreaterThan(0)
                .WithMessage("El UserId debe ser mayor a 0.");

            RuleFor(x => x.SeatId)
                .NotEqual(Guid.Empty)
                .WithMessage("El SeatId no puede ser vacío.");
        }
    }
}
