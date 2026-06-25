using TP_PROYECTO_SOFTWARE.Application.UseCases.AuditLogs.Commands;

namespace TP_PROYECTO_SOFTWARE.Application.IHandlers
{
    public interface ICreateAuditLogHandler
    {
        Task Handle(CreateAuditLogCommand command);
    }
}
