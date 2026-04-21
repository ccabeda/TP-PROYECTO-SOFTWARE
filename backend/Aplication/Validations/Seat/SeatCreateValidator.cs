using FluentValidation;
using TP_PROYECTO_SOFTWARE.Aplication.DTOs.SeatDTOs;

namespace TP_PROYECTO_SOFTWARE.Aplication.Validations.Seat
{
    public class SeatCreateValidator : AbstractValidator<SeatCreateDTO>
    {
        public SeatCreateValidator()
        {
            RuleFor(x => x.RowIdentifier).NotEmpty().MaximumLength(10);
            RuleFor(x => x.SeatNumber).GreaterThan(0);
        }
    }
}
