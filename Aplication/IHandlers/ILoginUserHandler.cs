using TP_PROYECTO_SOFTWARE.Aplication.DTOs.UserDTOs;
using TP_PROYECTO_SOFTWARE.Aplication.UseCases.Users.Commands;

namespace TP_PROYECTO_SOFTWARE.Aplication.IHandlers
{
    public interface ILoginUserHandler
    {
        Task<UserGetDTO> Handle(LoginUserCommand command);
    }
}
