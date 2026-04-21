using AutoMapper;
using Microsoft.AspNetCore.Identity;
using TP_PROYECTO_SOFTWARE.Aplication.DTOs.UserDTOs;
using TP_PROYECTO_SOFTWARE.Aplication.IHandlers;
using TP_PROYECTO_SOFTWARE.Aplication.IRepository.IQuery;
using TP_PROYECTO_SOFTWARE.Aplication.ISecurity;
using TP_PROYECTO_SOFTWARE.Aplication.UseCases.Users.Commands;
using TP_PROYECTO_SOFTWARE.Domain.Models;

namespace TP_PROYECTO_SOFTWARE.Aplication.UseCases.Users.Handlers
{
    public class LoginUserHandler : ILoginUserHandler
    {
        private static readonly PasswordHasher<USER> PasswordHasher = new();
        private readonly IRepositoryUserQuery _repositoryUserQuery;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IMapper _mapper;

        public LoginUserHandler(IRepositoryUserQuery repositoryUserQuery, IJwtTokenGenerator jwtTokenGenerator, IMapper mapper)
        {
            _repositoryUserQuery = repositoryUserQuery;
            _jwtTokenGenerator = jwtTokenGenerator;
            _mapper = mapper;
        }

        public async Task<UserLoginResponseDTO> Handle(LoginUserCommand command)
        {
            var user = await _repositoryUserQuery.GetByEmail(command.Email)
                ?? throw new KeyNotFoundException("Usuario no encontrado.");

            var verificationResult = PasswordHasher.VerifyHashedPassword(user, user.PasswordHash, command.Password);

            if (verificationResult == PasswordVerificationResult.Failed)
            {
                throw new UnauthorizedAccessException("Credenciales invalidas.");
            }

            var response = _mapper.Map<UserLoginResponseDTO>(user);
            response.Role = user.Email.Equals("agustin@test.com", StringComparison.OrdinalIgnoreCase) ? "Admin" : "User";
            response.Token = _jwtTokenGenerator.GenerateToken(user);

            return response;
        }
    }
}
