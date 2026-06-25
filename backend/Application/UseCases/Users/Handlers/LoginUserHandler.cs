using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using TP_PROYECTO_SOFTWARE.Application.Configuration;
using TP_PROYECTO_SOFTWARE.Application.DTOs.UserDTOs;
using TP_PROYECTO_SOFTWARE.Application.IHandlers;
using TP_PROYECTO_SOFTWARE.Application.ISecurity;
using TP_PROYECTO_SOFTWARE.Application.UseCases.AuditLogs.Commands;
using TP_PROYECTO_SOFTWARE.Application.UseCases.Users.Commands;
using TP_PROYECTO_SOFTWARE.Domain.Constants;
using TP_PROYECTO_SOFTWARE.Domain.Models;

namespace TP_PROYECTO_SOFTWARE.Application.UseCases.Users.Handlers
{
    public class LoginUserHandler : ILoginUserHandler
    {
        private readonly UserManager<User> _userManager;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly ICreateAuditLogHandler _createAuditLogHandler;
        private readonly JwtSettingsOptions _jwtSettings;
        private readonly IMapper _mapper;

        public LoginUserHandler(
            UserManager<User> userManager,
            IJwtTokenGenerator jwtTokenGenerator,
            ICreateAuditLogHandler createAuditLogHandler,
            IOptions<JwtSettingsOptions> jwtSettings,
            IMapper mapper)
        {
            _userManager = userManager;
            _jwtTokenGenerator = jwtTokenGenerator;
            _createAuditLogHandler = createAuditLogHandler;
            _jwtSettings = jwtSettings.Value;
            _mapper = mapper;
        }

        public async Task<UserLoginResponseDTO> Handle(LoginUserCommand command)
        {
            var user = await GetUserOrThrow(command.Email);
            await ValidatePasswordOrThrow(user, command.Password);
            var roles = await _userManager.GetRolesAsync(user);
            var refreshToken = await SetRefreshToken(user);
            var response = BuildResponse(user, roles, refreshToken);

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

        private async Task<string> SetRefreshToken(User user)
        {
            var refreshToken = _jwtTokenGenerator.GenerateRefreshToken();
            user.RefreshTokenHash = _jwtTokenGenerator.HashRefreshToken(refreshToken);
            user.RefreshTokenExpiresAt = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenDays);

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                throw new InvalidOperationException("No se pudo actualizar la sesión del usuario.");
            }

            return refreshToken;
        }

        private UserLoginResponseDTO BuildResponse(User user, IList<string> roles, string refreshToken)
        {
            var response = _mapper.Map<UserLoginResponseDTO>(user);
            response.Role = _jwtTokenGenerator.ResolvePrimaryRole(roles);
            response.Token = _jwtTokenGenerator.GenerateToken(user, roles);
            response.RefreshToken = refreshToken;
            response.RefreshTokenExpiresAt = user.RefreshTokenExpiresAt ?? DateTime.UtcNow;
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
