using AutoMapper;
using Microsoft.AspNetCore.Identity;
using TP_PROYECTO_SOFTWARE.Aplication.DTOs.UserDTOs;
using TP_PROYECTO_SOFTWARE.Aplication.IHandlers;
using TP_PROYECTO_SOFTWARE.Aplication.ISecurity;
using TP_PROYECTO_SOFTWARE.Aplication.UseCases.AuditLogs.Commands;
using TP_PROYECTO_SOFTWARE.Aplication.UseCases.Users.Commands;
using TP_PROYECTO_SOFTWARE.Domain.Constants;
using TP_PROYECTO_SOFTWARE.Domain.Models;

namespace TP_PROYECTO_SOFTWARE.Aplication.UseCases.Users.Handlers
{
    public class LoginUserHandler : ILoginUserHandler
    {
        private readonly UserManager<User> _userManager;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly ICreateAuditLogHandler _createAuditLogHandler;
        private readonly IMapper _mapper;

        public LoginUserHandler(
            UserManager<User> userManager,
            IJwtTokenGenerator jwtTokenGenerator,
            ICreateAuditLogHandler createAuditLogHandler,
            IMapper mapper)
        {
            _userManager = userManager;
            _jwtTokenGenerator = jwtTokenGenerator;
            _createAuditLogHandler = createAuditLogHandler;
            _mapper = mapper;
        }

        public async Task<UserLoginResponseDTO> Handle(LoginUserCommand command)
        {
            var user = await GetUserOrThrow(command.Email);
            await ValidatePasswordOrThrow(user, command.Password);
            var roles = await _userManager.GetRolesAsync(user);
            var response = BuildResponse(user, roles);

            await CreateAuditLog(user, response.Role);

            return response;
        }

        private async Task<User> GetUserOrThrow(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null)
            {
                await CreateRejectedAuditLog(null, email, "Usuario inexistente.");
                throw new UnauthorizedAccessException("Email o contraseña incorrectos. Verifica los datos e intenta de nuevo.");
            }

            return user;
        }

        private async Task ValidatePasswordOrThrow(User user, string password)
        {
            var isPasswordValid = await _userManager.CheckPasswordAsync(user, password);
            if (!isPasswordValid)
            {
                await CreateRejectedAuditLog(user.Id, user.Email, "Contraseña inválida.");
                throw new UnauthorizedAccessException("Email o contraseña incorrectos. Verifica los datos e intenta de nuevo.");
            }
        }

        private UserLoginResponseDTO BuildResponse(User user, IList<string> roles)
        {
            var response = _mapper.Map<UserLoginResponseDTO>(user);
            response.Role = _jwtTokenGenerator.ResolvePrimaryRole(roles);
            response.Token = _jwtTokenGenerator.GenerateToken(user, roles);
            return response;
        }

        private async Task CreateAuditLog(User user, string role)
        {
            await _createAuditLogHandler.Handle(new CreateAuditLogCommand
            {
                UserId = user.Id,
                Action = AuditActions.LoginUser,
                EntityType = AuditEntityTypes.User,
                EntityId = user.Id.ToString(),
                Details = $"Login exitoso. UserId={user.Id}, Email={user.Email}, Role={role}"
            });
        }

        private async Task CreateRejectedAuditLog(int? userId, string? email, string reason)
        {
            await _createAuditLogHandler.Handle(new CreateAuditLogCommand
            {
                UserId = userId,
                Action = AuditActions.LoginUserRejected,
                EntityType = AuditEntityTypes.User,
                EntityId = userId?.ToString() ?? (email ?? string.Empty),
                Details = $"Login rechazado. UserId={userId?.ToString() ?? "null"}, Email={email ?? string.Empty}, Reason={reason}"
            });
        }
    }
}
