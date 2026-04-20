using AutoMapper;
using TP_PROYECTO_SOFTWARE.Aplication.DTOs.UserDTOs;
using TP_PROYECTO_SOFTWARE.Aplication.IHandlers;
using TP_PROYECTO_SOFTWARE.Aplication.IRepository.IQuery;
using TP_PROYECTO_SOFTWARE.Aplication.UseCases.Users.Commands;

namespace TP_PROYECTO_SOFTWARE.Aplication.UseCases.Users.Handlers
{
    public class LoginUserHandler : ILoginUserHandler
    {
        private readonly IRepositoryUserQuery _repositoryUserQuery;
        private readonly IMapper _mapper;

        public LoginUserHandler(IRepositoryUserQuery repositoryUserQuery, IMapper mapper)
        {
            _repositoryUserQuery = repositoryUserQuery;
            _mapper = mapper;
        }

        public async Task<UserGetDTO> Handle(LoginUserCommand command)
        {
            var user = await _repositoryUserQuery.GetByEmail(command.Email)
                ?? throw new KeyNotFoundException("Usuario no encontrado.");

            if (user.PasswordHash != command.Password)
            {
                throw new UnauthorizedAccessException("Credenciales invalidas.");
            }

            return _mapper.Map<UserGetDTO>(user);
        }
    }
}
