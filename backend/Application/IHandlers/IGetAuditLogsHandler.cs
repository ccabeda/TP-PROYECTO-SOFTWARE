using TP_PROYECTO_SOFTWARE.Application.DTOs;
using TP_PROYECTO_SOFTWARE.Application.DTOs.AuditLogDTOs;
using TP_PROYECTO_SOFTWARE.Application.UseCases.AuditLogs.Queries;

namespace TP_PROYECTO_SOFTWARE.Application.IHandlers
{
    public interface IGetAuditLogsHandler
    {
        Task<PagedResultDTO<AuditLogGetDTO>> Handle(GetAuditLogsQuery query);
    }
}
