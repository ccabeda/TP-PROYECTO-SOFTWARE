using AutoMapper;
using TP_PROYECTO_SOFTWARE.Aplication.DTOs.UserDTOs;
using TP_PROYECTO_SOFTWARE.Aplication.IHandlers;
using TP_PROYECTO_SOFTWARE.Aplication.IRepository.ICommand;
using TP_PROYECTO_SOFTWARE.Aplication.IRepository.IQuery;
using TP_PROYECTO_SOFTWARE.Aplication.UseCases.Users.Commands;
using TP_PROYECTO_SOFTWARE.Domain.Models;

namespace TP_PROYECTO_SOFTWARE.Aplication.UseCases.Users.Handlers
{
    public class CreateUserHandler : ICreateUserHandler
    {
        private readonly IRepositoryUserQuery _repositoryUserQuery;
        private readonly IRepositoryUserCommand _repositoryUserCommand;
        private readonly IMapper _mapper;

        public CreateUserHandler(IRepositoryUserQuery repositoryUserQuery, IRepositoryUserCommand repositoryUserCommand, IMapper mapper)
        {
            _repositoryUserQuery = repositoryUserQuery;
            _repositoryUserCommand = repositoryUserCommand;
            _mapper = mapper;
        }

        public async Task<UserGetDTO> Handle(CreateUserCommand command)
        {
            var existingUser = await _repositoryUserQuery.GetByEmail(command.Email);
            if (existingUser is not null)
            {
                throw new InvalidOperationException("Ya existe un usuario con ese email.");
            }

            var user = new USER
            {
                Name = command.Name,
                Email = command.Email,
                PasswordHash = command.Password
            };

            await _repositoryUserCommand.Create(user);
            await _repositoryUserCommand.Save();

            return _mapper.Map<UserGetDTO>(user);
        }
    }
}
