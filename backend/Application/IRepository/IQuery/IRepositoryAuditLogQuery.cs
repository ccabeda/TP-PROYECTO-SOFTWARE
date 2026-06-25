using TP_PROYECTO_SOFTWARE.Domain.Models;
using TP_PROYECTO_SOFTWARE.Application.UseCases.AuditLogs.Queries;

namespace TP_PROYECTO_SOFTWARE.Application.IRepository.IQuery
{
    public interface IRepositoryAuditLogQuery
    {
        Task<(List<AuditLog> Items, int TotalCount)> GetAll(GetAuditLogsQuery query);
    }
}

