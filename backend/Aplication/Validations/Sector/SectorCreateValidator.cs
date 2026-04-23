using FluentValidation;
using Microsoft.Extensions.Options;
using TP_PROYECTO_SOFTWARE.Aplication.Configuration;
using TP_PROYECTO_SOFTWARE.Aplication.DTOs.SectorDTOs;

namespace TP_PROYECTO_SOFTWARE.Aplication.Validations.Sector
{
    public class SectorCreateValidator : AbstractValidator<SectorCreateDTO>
    {
        public SectorCreateValidator(IOptions<TicketingRulesOptions> ticketingRules)
        {
            var rules = ticketingRules.Value;

            RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Price).GreaterThan(0);
            RuleFor(x => x.Capacity)
                .GreaterThan(0)
                .LessThanOrEqualTo(rules.MaxSectorCapacity)
                .WithMessage($"La capacidad del sector debe ser mayor a 0 y menor o igual a {rules.MaxSectorCapacity}.");
        }
    }
}
