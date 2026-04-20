using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TP_PROYECTO_SOFTWARE.Aplication.DTOs.SeatDTOs;
using TP_PROYECTO_SOFTWARE.Aplication.IHandlers;
using TP_PROYECTO_SOFTWARE.Aplication.UseCases.Seats.Queries;

namespace TP_PROYECTO_SOFTWARE.API.Controllers
{
    [Route("api/v1")]
    [ApiController]
    [Tags("Seats")]
    public class SeatsController : ControllerBase
    {
        private readonly IGetSeatsByEventHandler _getSeatsByEventHandler;
        private readonly IGetSeatsBySectorHandler _handler;

        public SeatsController(IGetSeatsByEventHandler getSeatsByEventHandler, IGetSeatsBySectorHandler handler)
        {
            _getSeatsByEventHandler = getSeatsByEventHandler;
            _handler = handler;
        }

        [HttpGet("events/{eventId}/seats")]
        [SwaggerOperation(Summary = "Listado de asientos por evento")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Not Found")]
        [ProducesResponseType(typeof(List<SeatGetDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSeatsByEvent([FromRoute] int eventId)
        {
            var result = await _getSeatsByEventHandler.Handle(new GetSeatsByEventQuery { EventId = eventId });
            return Ok(result);
        }

        [HttpGet("sectors/{sectorId}/seats")]
        [SwaggerOperation(Summary = "Listado de asientos por sector")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Not Found")]
        [ProducesResponseType(typeof(List<SeatGetDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSeatsBySector([FromRoute] int sectorId)
        {
            var result = await _handler.Handle(new GetSeatsBySectorQuery { SectorId = sectorId });
            return Ok(result);
        }
    }
}
