using FluentValidation;
using TP_PROYECTO_SOFTWARE.Aplication.DTOs.SectorDTOs;

namespace TP_PROYECTO_SOFTWARE.Aplication.Validations.Sector
{
    public class SectorCreateValidator : AbstractValidator<SectorCreateDTO>
    {
        public SectorCreateValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Price).GreaterThan(0);
            RuleFor(x => x.Capacity).GreaterThan(0).LessThanOrEqualTo(100);
        }
    }
}
