using FluentValidation;
using TP_PROYECTO_SOFTWARE.Application.DTOs.EventDTOs;

namespace TP_PROYECTO_SOFTWARE.Application.Validations.Event
{
    public class EventCreateValidator : AbstractValidator<EventCreateDTO>
    {
        public EventCreateValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(150);
            RuleFor(x => x.Venue).NotEmpty().MaximumLength(150);
            RuleFor(x => x.Status).NotEmpty().MaximumLength(50);
            RuleFor(x => x.EventDate).GreaterThan(DateTime.MinValue);
            RuleFor(x => x.ImageUrl)
                .MaximumLength(500)
                .Must(url => string.IsNullOrWhiteSpace(url) || Uri.TryCreate(url, UriKind.Absolute, out _))
                .WithMessage("ImageUrl debe ser una URL valida.");
            RuleFor(x => x.Description).MaximumLength(2000);
        }
    }
}
