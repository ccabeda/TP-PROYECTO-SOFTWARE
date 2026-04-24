using FluentValidation;
using Microsoft.Extensions.Options;
using TP_PROYECTO_SOFTWARE.Aplication.Configuration;
using TP_PROYECTO_SOFTWARE.Aplication.DTOs.SeatDTOs;

namespace TP_PROYECTO_SOFTWARE.Aplication.Validations.Seat
{
    public class SeatCreateValidator : AbstractValidator<SeatCreateDTO>
    {
        public SeatCreateValidator(IOptions<TicketingRulesOptions> ticketingRules)
        {
            var rules = ticketingRules.Value;
            var validRows = TicketingRulesOptionsHelper
                .GetValidRowLabels(rules)
                .ToHashSet();

            RuleFor(x => x.RowIdentifier)
                .NotEmpty()
                .Must(rowIdentifier =>
                {
                    var normalizedRow = rowIdentifier.Trim().ToUpperInvariant();
                    return validRows.Contains(normalizedRow);
                })
                .WithMessage($"La fila debe ser una de las configuradas: {string.Join(", ", validRows)}.");

            RuleFor(x => x.SeatNumber)
                .GreaterThan(0)
                .LessThanOrEqualTo(rules.MaxSeatsPerRow)
                .WithMessage($"El número de asiento debe estar entre 1 y {rules.MaxSeatsPerRow}.");
        }
    }
}
