using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using TP_PROYECTO_SOFTWARE.Application.Configuration;
using TP_PROYECTO_SOFTWARE.Application.DTOs.UserDTOs;
using TP_PROYECTO_SOFTWARE.Application.IHandlers;
using TP_PROYECTO_SOFTWARE.Application.IRepository.IQuery;
using TP_PROYECTO_SOFTWARE.Application.ISecurity;
using TP_PROYECTO_SOFTWARE.Application.UseCases.Users.Commands;
using TP_PROYECTO_SOFTWARE.Domain.Models;

namespace TP_PROYECTO_SOFTWARE.Application.UseCases.Users.Handlers
{
    public class RefreshUserTokenHandler : IRefreshUserTokenHandler
    {
        private readonly UserManager<User> _userManager;
        private readonly IRepositoryUserQuery _repositoryUserQuery;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly JwtSettingsOptions _jwtSettings;
        private readonly IMapper _mapper;

        public RefreshUserTokenHandler(
            UserManager<User> userManager,
            IRepositoryUserQuery repositoryUserQuery,
            IJwtTokenGenerator jwtTokenGenerator,
            IOptions<JwtSettingsOptions> jwtSettings,
            IMapper mapper)
        {
            _userManager = userManager;
            _repositoryUserQuery = repositoryUserQuery;
            _jwtTokenGenerator = jwtTokenGenerator;
            _jwtSettings = jwtSettings.Value;
            _mapper = mapper;
        }

        public async Task<UserLoginResponseDTO> Handle(RefreshUserTokenCommand command)
        {
            if (string.IsNullOrWhiteSpace(command.RefreshToken))
            {
                throw new UnauthorizedAccessException("Refresh token inválido.");
            }

            var refreshTokenHash = _jwtTokenGenerator.HashRefreshToken(command.RefreshToken);
            var user = await _repositoryUserQuery.GetByValidRefreshTokenHash(refreshTokenHash, DateTime.UtcNow)
                ?? throw new UnauthorizedAccessException("Refresh token inválido o expirado.");

            var roles = await _userManager.GetRolesAsync(user);
            var nextRefreshToken = _jwtTokenGenerator.GenerateRefreshToken();
            user.RefreshTokenHash = _jwtTokenGenerator.HashRefreshToken(nextRefreshToken);
            user.RefreshTokenExpiresAt = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenDays);

            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                throw new InvalidOperationException("No se pudo renovar la sesión del usuario.");
            }

            var response = _mapper.Map<UserLoginResponseDTO>(user);
            response.Role = _jwtTokenGenerator.ResolvePrimaryRole(roles);
            response.Token = _jwtTokenGenerator.GenerateToken(user, roles);
            response.RefreshToken = nextRefreshToken;
            response.RefreshTokenExpiresAt = user.RefreshTokenExpiresAt.Value;

            return response;
        }
    }
}
