using TP_PROYECTO_SOFTWARE.Aplication.IHandlers;
using TP_PROYECTO_SOFTWARE.Aplication.IRepository.ICommand;
using TP_PROYECTO_SOFTWARE.Aplication.UseCases.AuditLogs.Commands;
using TP_PROYECTO_SOFTWARE.Domain.Models;

namespace TP_PROYECTO_SOFTWARE.Aplication.UseCases.AuditLogs.Handlers
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
            var auditLog = new AuditLog
            {
                UserId = command.UserId,
                Action = command.Action,
                EntityType = command.EntityType,
                EntityId = command.EntityId,
                Details = command.Details,
                CreatedAt = DateTime.UtcNow
            };

            await _repositoryAuditLogCommand.Create(auditLog);
        }
    }
}

