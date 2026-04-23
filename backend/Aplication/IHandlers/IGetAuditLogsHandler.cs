using TP_PROYECTO_SOFTWARE.Aplication.DTOs.AuditLogDTOs;
using TP_PROYECTO_SOFTWARE.Aplication.UseCases.AuditLogs.Queries;

namespace TP_PROYECTO_SOFTWARE.Aplication.IHandlers
{
    public interface IGetAuditLogsHandler
    {
        Task<List<AuditLogGetDTO>> Handle(GetAuditLogsQuery query);
    }
}
