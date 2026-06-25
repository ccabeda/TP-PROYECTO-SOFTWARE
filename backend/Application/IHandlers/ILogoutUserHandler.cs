using TP_PROYECTO_SOFTWARE.Application.UseCases.Users.Commands;

namespace TP_PROYECTO_SOFTWARE.Application.IHandlers
{
    public interface ILogoutUserHandler
    {
        Task Handle(LogoutUserCommand command);
    }
}
