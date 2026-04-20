using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TP_PROYECTO_SOFTWARE.Aplication.DTOs.EventDTOs;
using TP_PROYECTO_SOFTWARE.Aplication.IHandlers;
using TP_PROYECTO_SOFTWARE.Aplication.UseCases.Events.Queries;

namespace TP_PROYECTO_SOFTWARE.API.Controllers
{
    [Route("api/v1/events")]
    [ApiController]
    [Tags("Events")]
    public class EventsController : ControllerBase
    {
        private readonly IGetEventsHandler _handler;

        public EventsController(IGetEventsHandler handler)
        {
            _handler = handler;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Listado de eventos")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success")]
        [ProducesResponseType(typeof(List<EventGetDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetEvents([FromQuery] string? name, [FromQuery] DateTime? eventDate)
        {
            var result = await _handler.Handle(new GetEventsQuery
            {
                Name = name,
                EventDate = eventDate
            });
            return Ok(result);
        }
    }
}
