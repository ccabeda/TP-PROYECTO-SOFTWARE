using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using TP_PROYECTO_SOFTWARE.Aplication.Configuration;
using TP_PROYECTO_SOFTWARE.Aplication.DTOs.UserDTOs;
using TP_PROYECTO_SOFTWARE.Aplication.IHandlers;
using TP_PROYECTO_SOFTWARE.Aplication.UseCases.AuditLogs.Commands;
using TP_PROYECTO_SOFTWARE.Aplication.UseCases.Users.Commands;
using TP_PROYECTO_SOFTWARE.Domain.Models;

namespace TP_PROYECTO_SOFTWARE.Aplication.UseCases.Users.Handlers
{
    public class CreateUserHandler : ICreateUserHandler
    {
        private readonly UserManager<User> _userManager;
        private readonly ICreateAuditLogHandler _createAuditLogHandler;
        private readonly IMapper _mapper;
        private readonly AuthorizationSettingsOptions _authorizationSettings;

        public CreateUserHandler(
            UserManager<User> userManager,
            ICreateAuditLogHandler createAuditLogHandler,
            IMapper mapper,
            IOptions<AuthorizationSettingsOptions> authorizationSettings)
        {
            _userManager = userManager;
            _createAuditLogHandler = createAuditLogHandler;
            _mapper = mapper;
            _authorizationSettings = authorizationSettings.Value;
        }

        public async Task<UserGetDTO> Handle(CreateUserCommand command)
        {
            var existingUser = await _userManager.FindByEmailAsync(command.Email);
            if (existingUser is not null)
            {
                throw new InvalidOperationException("Ya existe un usuario con ese email.");
            }

            var user = BuildUser(command);

            await CreateUserOrThrow(user, command.Password);

            var role = await AssignInitialRoleOrThrow(user);

            await CreateAuditLog(user, role);

            return _mapper.Map<UserGetDTO>(user);
        }

        private static User BuildUser(CreateUserCommand command)
        {
            return new User
            {
                Name = command.Name,
                Email = command.Email,
                UserName = command.Email
            };
        }

        private async Task CreateUserOrThrow(User user, string password)
        {
            var createResult = await _userManager.CreateAsync(user, password);
            EnsureSucceeded(createResult);
        }

        private async Task<string> AssignInitialRoleOrThrow(User user)
        {
            var role = ResolveInitialRole(user.Email!);
            var roleResult = await _userManager.AddToRoleAsync(user, role);
            EnsureSucceeded(roleResult);
            return role;
        }

        private async Task CreateAuditLog(User user, string role)
        {
            await _createAuditLogHandler.Handle(new CreateAuditLogCommand
            {
                UserId = null,
                Action = "CreateUser",
                EntityType = "USER",
                EntityId = user.Id.ToString(),
                Details = $"Usuario creado. UserId={user.Id}, Email={user.Email}, Name={user.Name}, Role={role}"
            });
        }

        private string ResolveInitialRole(string email)
        {
            return _authorizationSettings.AdminEmails.Contains(email, StringComparer.OrdinalIgnoreCase) ? "Admin" : "User";
        }

        private static void EnsureSucceeded(IdentityResult result)
        {
            if (!result.Succeeded)
            {
                throw new InvalidOperationException(string.Join(" ", result.Errors.Select(error => error.Description)));
            }
        }
    }
}
