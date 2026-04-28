using TP_PROYECTO_SOFTWARE.Aplication.DTOs;
using TP_PROYECTO_SOFTWARE.Aplication.DTOs.AuditLogDTOs;
using TP_PROYECTO_SOFTWARE.Aplication.UseCases.AuditLogs.Queries;

namespace TP_PROYECTO_SOFTWARE.Aplication.IHandlers
{
    public interface IGetAuditLogsHandler
    {
        Task<PagedResultDTO<AuditLogGetDTO>> Handle(GetAuditLogsQuery query);
    }
}
