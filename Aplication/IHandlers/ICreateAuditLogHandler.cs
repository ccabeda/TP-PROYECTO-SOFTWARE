using TP_PROYECTO_SOFTWARE.Aplication.UseCases.AuditLogs.Commands;

namespace TP_PROYECTO_SOFTWARE.Aplication.IHandlers
{
    public interface ICreateAuditLogHandler
    {
        Task Handle(CreateAuditLogCommand command);
    }
}
