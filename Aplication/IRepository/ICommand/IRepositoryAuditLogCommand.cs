using TP_PROYECTO_SOFTWARE.Domain.Models;

namespace TP_PROYECTO_SOFTWARE.Aplication.IRepository.ICommand
{
    public interface IRepositoryAuditLogCommand
    {
        Task Create(AUDIT_LOG auditLog);
    }
}
