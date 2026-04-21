using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TP_PROYECTO_SOFTWARE.API.Helpers;
using TP_PROYECTO_SOFTWARE.Aplication.DTOs.EventDTOs;
using TP_PROYECTO_SOFTWARE.Aplication.IHandlers;
using TP_PROYECTO_SOFTWARE.Aplication.UseCases.Events.Commands;
using TP_PROYECTO_SOFTWARE.Aplication.UseCases.Events.Queries;

namespace TP_PROYECTO_SOFTWARE.API.Controllers
{
    [Route("api/v1/events")]
    [ApiController]
    [Tags("Events")]
    public class EventsController : ControllerBase
    {
        private readonly IGetEventsHandler _getEventsHandler;
        private readonly ICreateEventHandler _createEventHandler;
        private readonly IDeleteEventHandler _deleteEventHandler;
        private readonly IMapper _mapper;

        public EventsController(
            IGetEventsHandler getEventsHandler,
            ICreateEventHandler createEventHandler,
            IDeleteEventHandler deleteEventHandler,
            IMapper mapper)
        {
            _getEventsHandler = getEventsHandler;
            _createEventHandler = createEventHandler;
            _deleteEventHandler = deleteEventHandler;
            _mapper = mapper;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Listado de eventos")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success")]
        [ProducesResponseType(typeof(List<EventGetDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetEvents([FromQuery] string? name, [FromQuery] DateTime? eventDate)
        {
            var result = await _getEventsHandler.Handle(new GetEventsQuery
            {
                Name = name,
                EventDate = eventDate
            });
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [SwaggerOperation(Summary = "Crea un evento")]
        [SwaggerResponse(StatusCodes.Status201Created, "Created")]
        [ProducesResponseType(typeof(EventGetDTO), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateEvent([FromBody] EventCreateDTO eventCreateDTO)
        {
            var command = _mapper.Map<CreateEventCommand>(eventCreateDTO);
            command.UserId = UserClaimsHelper.GetCurrentUserId(User);
            var result = await _createEventHandler.Handle(command);

            return CreatedAtAction(nameof(GetEvents), new { id = result.Id }, result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        [SwaggerOperation(Summary = "Elimina un evento y sus sectores/asientos si no tiene reservas asociadas")]
        [SwaggerResponse(StatusCodes.Status204NoContent, "No Content")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Not Found")]
        [SwaggerResponse(StatusCodes.Status409Conflict, "Conflict")]
        public async Task<IActionResult> DeleteEvent([FromRoute] int id)
        {
            await _deleteEventHandler.Handle(new DeleteEventCommand
            {
                EventId = id,
                UserId = UserClaimsHelper.GetCurrentUserId(User)
            });
            return NoContent();
        }
    }
}
