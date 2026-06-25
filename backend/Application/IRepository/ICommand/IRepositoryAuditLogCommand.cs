using TP_PROYECTO_SOFTWARE.Domain.Models;

namespace TP_PROYECTO_SOFTWARE.Application.IRepository.ICommand
{
    public interface IRepositoryAuditLogCommand
    {
        Task Create(AuditLog auditLog);
        Task Save();
    }
}

