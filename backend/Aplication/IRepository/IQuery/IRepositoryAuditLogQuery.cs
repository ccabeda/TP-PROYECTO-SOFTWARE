using TP_PROYECTO_SOFTWARE.Domain.Models;
using TP_PROYECTO_SOFTWARE.Aplication.UseCases.AuditLogs.Queries;

namespace TP_PROYECTO_SOFTWARE.Aplication.IRepository.IQuery
{
    public interface IRepositoryAuditLogQuery
    {
        Task<List<AuditLog>> GetAll(GetAuditLogsQuery query);
    }
}

