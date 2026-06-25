using FluentValidation;
using TP_PROYECTO_SOFTWARE.Application.DTOs.UserDTOs;

namespace TP_PROYECTO_SOFTWARE.Application.Validations.User
{
    public class UserLoginValidator : AbstractValidator<UserLoginDTO>
    {
        public UserLoginValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                .MaximumLength(150);

            RuleFor(x => x.Password)
                .NotEmpty()
                .MaximumLength(255);
        }
    }
}
