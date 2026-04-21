using FluentValidation;
using TP_PROYECTO_SOFTWARE.Aplication.DTOs.SeatDTOs;

namespace TP_PROYECTO_SOFTWARE.Aplication.Validations.Seat
{
    public class SeatBulkCreateValidator : AbstractValidator<SeatBulkCreateDTO>
    {
        public SeatBulkCreateValidator()
        {
            RuleFor(x => x.Rows).NotEmpty();
            RuleFor(x => x.Rows.Count).LessThanOrEqualTo(10);
            RuleForEach(x => x.Rows).NotEmpty().MaximumLength(10);
            RuleFor(x => x.SeatsPerRow).GreaterThan(0).LessThanOrEqualTo(10);
        }
    }
}
