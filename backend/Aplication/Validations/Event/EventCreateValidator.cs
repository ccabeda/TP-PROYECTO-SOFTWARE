using FluentValidation;
using TP_PROYECTO_SOFTWARE.Aplication.DTOs.EventDTOs;

namespace TP_PROYECTO_SOFTWARE.Aplication.Validations.Event
{
    public class EventCreateValidator : AbstractValidator<EventCreateDTO>
    {
        public EventCreateValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(150);
            RuleFor(x => x.Venue).NotEmpty().MaximumLength(150);
            RuleFor(x => x.Status).NotEmpty().MaximumLength(50);
            RuleFor(x => x.EventDate).GreaterThan(DateTime.MinValue);
        }
    }
}
