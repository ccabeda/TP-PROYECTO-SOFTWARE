using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TP_PROYECTO_SOFTWARE.Aplication.DTOs.EventDTOs;
using TP_PROYECTO_SOFTWARE.Aplication.DTOs.SeatDTOs;
using TP_PROYECTO_SOFTWARE.Aplication.UseCases.Events.Handlers;
using TP_PROYECTO_SOFTWARE.Aplication.UseCases.Events.Queries;
using TP_PROYECTO_SOFTWARE.Aplication.UseCases.Seats.Handlers;
using TP_PROYECTO_SOFTWARE.Aplication.UseCases.Seats.Queries;

namespace TP_PROYECTO_SOFTWARE.API.Controllers
{
    [Route("api/v1/events")]
    [ApiController]
    [Tags("Events")]
    public class EventsController : ControllerBase
    {
        private readonly GetEventsHandler _handler;
        private readonly GetSeatsByEventHandler _getSeatsByEventHandler;

        public EventsController(GetEventsHandler handler, GetSeatsByEventHandler getSeatsByEventHandler)
        {
            _handler = handler;
            _getSeatsByEventHandler = getSeatsByEventHandler;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Listado de eventos")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success")]
        [ProducesResponseType(typeof(List<EventGetDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetEvents()
        {
            var result = await _handler.Handle(new GetEventsQuery());
            return Ok(result);
        }

        [HttpGet("{eventId}/seats")]
        [SwaggerOperation(Summary = "Listado de asientos por evento")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success")]
        [ProducesResponseType(typeof(List<SeatGetDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSeatsByEvent([FromRoute] int eventId)
        {
            var result = await _getSeatsByEventHandler.Handle(new GetSeatsByEventQuery { EventId = eventId });
            return Ok(result);
        }
    }
}
