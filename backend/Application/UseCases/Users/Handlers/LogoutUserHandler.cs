using Microsoft.AspNetCore.Identity;
using TP_PROYECTO_SOFTWARE.Application.IHandlers;
using TP_PROYECTO_SOFTWARE.Application.UseCases.Users.Commands;
using TP_PROYECTO_SOFTWARE.Domain.Models;

namespace TP_PROYECTO_SOFTWARE.Application.UseCases.Users.Handlers
{
    public class LogoutUserHandler : ILogoutUserHandler
    {
        private readonly UserManager<User> _userManager;

        public LogoutUserHandler(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task Handle(LogoutUserCommand command)
        {
            var user = await _userManager.FindByIdAsync(command.UserId.ToString())
                ?? throw new UnauthorizedAccessException("Usuario no autenticado.");

            user.RefreshTokenHash = null;
            user.RefreshTokenExpiresAt = null;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                throw new InvalidOperationException("No se pudo cerrar la sesión del usuario.");
            }
        }
    }
}
