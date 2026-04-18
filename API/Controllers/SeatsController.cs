using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TP_PROYECTO_SOFTWARE.Aplication.DTOs.SeatDTOs;
using TP_PROYECTO_SOFTWARE.Aplication.UseCases.Seats.Handlers;
using TP_PROYECTO_SOFTWARE.Aplication.UseCases.Seats.Queries;

namespace TP_PROYECTO_SOFTWARE.API.Controllers
{
    [Route("api/v1/sectors/{sectorId}/seats")]
    [ApiController]
    [Tags("Seats")]
    public class SeatsController : ControllerBase
    {
        private readonly GetSeatsBySectorHandler _handler;

        public SeatsController(GetSeatsBySectorHandler handler)
        {
            _handler = handler;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Listado de asientos por sector")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success")]
        [ProducesResponseType(typeof(List<SeatGetDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSeatsBySector([FromRoute] int sectorId)
        {
            var result = await _handler.Handle(new GetSeatsBySectorQuery { SectorId = sectorId });
            return Ok(result);
        }
    }
}
