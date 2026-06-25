using TP_PROYECTO_SOFTWARE.Application.DTOs.UserDTOs;
using TP_PROYECTO_SOFTWARE.Application.UseCases.Users.Commands;

namespace TP_PROYECTO_SOFTWARE.Application.IHandlers
{
    public interface ILoginUserHandler
    {
        Task<UserLoginResponseDTO> Handle(LoginUserCommand command);
    }
}
