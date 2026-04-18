using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TP_PROYECTO_SOFTWARE.Aplication.DTOs.SectorDTOs;
using TP_PROYECTO_SOFTWARE.Aplication.UseCases.Sectors.Handlers;
using TP_PROYECTO_SOFTWARE.Aplication.UseCases.Sectors.Queries;

namespace TP_PROYECTO_SOFTWARE.API.Controllers
{
    [Route("api/v1/events/{eventId}/sectors")]
    [ApiController]
    [Tags("Sectors")]
    public class SectorsController : ControllerBase
    {
        private readonly GetSectorsByEventHandler _handler;

        public SectorsController(GetSectorsByEventHandler handler)
        {
            _handler = handler;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Listado de sectores por evento")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success")]
        [ProducesResponseType(typeof(List<SectorGetDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSectorsByEvent([FromRoute] int eventId)
        {
            var result = await _handler.Handle(new GetSectorsByEventQuery { EventId = eventId });
            return Ok(result);
        }
    }
}
