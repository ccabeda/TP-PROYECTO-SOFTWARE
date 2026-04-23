using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TP_PROYECTO_SOFTWARE.Aplication.DTOs.AuditLogDTOs;
using TP_PROYECTO_SOFTWARE.Aplication.IHandlers;
using TP_PROYECTO_SOFTWARE.Aplication.UseCases.AuditLogs.Queries;

namespace TP_PROYECTO_SOFTWARE.API.Controllers
{
    [Route("api/v1/audit-logs")]
    [ApiController]
    [Tags("AuditLogs")]
    public class AuditLogsController : ControllerBase
    {
        private readonly IGetAuditLogsHandler _getAuditLogsHandler;

        public AuditLogsController(IGetAuditLogsHandler getAuditLogsHandler)
        {
            _getAuditLogsHandler = getAuditLogsHandler;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        [SwaggerOperation(Summary = "Listado de auditoría")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success")]
        [ProducesResponseType(typeof(List<AuditLogGetDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAuditLogs([FromQuery] int? userId, [FromQuery] DateTime? date, [FromQuery] DateTime? dateFrom, [FromQuery] DateTime? dateTo)
        {
            var result = await _getAuditLogsHandler.Handle(new GetAuditLogsQuery
            {
                UserId = userId,
                Date = date,
                DateFrom = dateFrom,
                DateTo = dateTo
            });
            return Ok(result);
        }
    }
}
