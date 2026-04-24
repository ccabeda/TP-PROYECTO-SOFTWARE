using AutoMapper;
using Microsoft.AspNetCore.Identity;
using TP_PROYECTO_SOFTWARE.Aplication.DTOs.UserDTOs;
using TP_PROYECTO_SOFTWARE.Aplication.IHandlers;
using TP_PROYECTO_SOFTWARE.Aplication.IRepository.ICommand;
using TP_PROYECTO_SOFTWARE.Aplication.IRepository.IQuery;
using TP_PROYECTO_SOFTWARE.Aplication.UseCases.AuditLogs.Commands;
using TP_PROYECTO_SOFTWARE.Aplication.UseCases.Users.Commands;
using TP_PROYECTO_SOFTWARE.Domain.Models;

namespace TP_PROYECTO_SOFTWARE.Aplication.UseCases.Users.Handlers
{
    public class CreateUserHandler : ICreateUserHandler
    {
        private static readonly PasswordHasher<USER> PasswordHasher = new();
        private readonly IRepositoryUserQuery _repositoryUserQuery;
        private readonly IRepositoryUserCommand _repositoryUserCommand;
        private readonly ICreateAuditLogHandler _createAuditLogHandler;
        private readonly IMapper _mapper;

        public CreateUserHandler(
            IRepositoryUserQuery repositoryUserQuery,
            IRepositoryUserCommand repositoryUserCommand,
            ICreateAuditLogHandler createAuditLogHandler,
            IMapper mapper)
        {
            _repositoryUserQuery = repositoryUserQuery;
            _repositoryUserCommand = repositoryUserCommand;
            _createAuditLogHandler = createAuditLogHandler;
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
                PasswordHash = string.Empty
            };

            user.PasswordHash = PasswordHasher.HashPassword(user, command.Password);

            await _repositoryUserCommand.Create(user);
            await _createAuditLogHandler.Handle(new CreateAuditLogCommand
            {
                UserId = null,
                Action = "CreateUser",
                EntityType = "USER",
                EntityId = user.Id.ToString(),
                Details = $"Usuario creado. UserId={user.Id}, Email={user.Email}, Name={user.Name}"
            });
            await _repositoryUserCommand.Save();

            return _mapper.Map<UserGetDTO>(user);
        }
    }
}
