using TP_PROYECTO_SOFTWARE.Application.IHandlers;
using TP_PROYECTO_SOFTWARE.Application.IRepository.ICommand;
using TP_PROYECTO_SOFTWARE.Application.UseCases.AuditLogs.Commands;
using TP_PROYECTO_SOFTWARE.Domain.Factories;

namespace TP_PROYECTO_SOFTWARE.Application.UseCases.AuditLogs.Handlers
{
    public class CreateAuditLogHandler : ICreateAuditLogHandler
    {
        private readonly IRepositoryAuditLogCommand _repositoryAuditLogCommand;

        public CreateAuditLogHandler(IRepositoryAuditLogCommand repositoryAuditLogCommand)
        {
            _repositoryAuditLogCommand = repositoryAuditLogCommand;
        }

        public async Task Handle(CreateAuditLogCommand command)
        {
            var auditLog = AuditLogFactory.Create(
                command.UserId,
                command.Action,
                command.EntityType,
                command.EntityId,
                command.Details);

            await _repositoryAuditLogCommand.Create(auditLog);
            await _repositoryAuditLogCommand.Save();
        }
    }
}

