using FluentValidation;
using Microsoft.Extensions.Options;
using TP_PROYECTO_SOFTWARE.Aplication.Configuration;
using TP_PROYECTO_SOFTWARE.Aplication.DTOs.SeatDTOs;

namespace TP_PROYECTO_SOFTWARE.Aplication.Validations.Seat
{
    public class SeatBulkCreateValidator : AbstractValidator<SeatBulkCreateDTO>
    {
        public SeatBulkCreateValidator(IOptions<TicketingRulesOptions> ticketingRules)
        {
            var rules = ticketingRules.Value;

            RuleFor(x => x.RowCount)
                .GreaterThan(0)
                .LessThanOrEqualTo(rules.MaxRowsPerBulkCreate)
                .WithMessage($"La cantidad de filas no puede superar {rules.MaxRowsPerBulkCreate}.");
            RuleFor(x => x.SeatsPerRow)
                .GreaterThan(0)
                .LessThanOrEqualTo(rules.MaxSeatsPerRow)
                .WithMessage($"La cantidad de asientos por fila debe estar entre 1 y {rules.MaxSeatsPerRow}.");
        }
    }
}
