using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TP_PROYECTO_SOFTWARE.API.Helpers;
using TP_PROYECTO_SOFTWARE.Aplication.DTOs.SeatDTOs;
using TP_PROYECTO_SOFTWARE.Aplication.IHandlers;
using TP_PROYECTO_SOFTWARE.Aplication.UseCases.Seats.Commands;
using TP_PROYECTO_SOFTWARE.Aplication.UseCases.Seats.Queries;

namespace TP_PROYECTO_SOFTWARE.API.Controllers
{
    [Route("api/v1")]
    [ApiController]
    [Tags("Seats")]
    public class SeatsController : ControllerBase
    {
        private readonly IGetSeatsByEventHandler _getSeatsByEventHandler;
        private readonly IGetSeatsBySectorHandler _getSeatsBySectorHandler;
        private readonly ICreateSeatHandler _createSeatHandler;
        private readonly ICreateSeatsBulkHandler _createSeatsBulkHandler;
        private readonly IDeleteSeatHandler _deleteSeatHandler;
        private readonly IMapper _mapper;

        public SeatsController(
            IGetSeatsByEventHandler getSeatsByEventHandler,
            IGetSeatsBySectorHandler getSeatsBySectorHandler,
            ICreateSeatHandler createSeatHandler,
            ICreateSeatsBulkHandler createSeatsBulkHandler,
            IDeleteSeatHandler deleteSeatHandler,
            IMapper mapper)
        {
            _getSeatsByEventHandler = getSeatsByEventHandler;
            _getSeatsBySectorHandler = getSeatsBySectorHandler;
            _createSeatHandler = createSeatHandler;
            _createSeatsBulkHandler = createSeatsBulkHandler;
            _deleteSeatHandler = deleteSeatHandler;
            _mapper = mapper;
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
            var result = await _getSeatsBySectorHandler.Handle(new GetSeatsBySectorQuery { SectorId = sectorId });
            return Ok(result);
        }

        [HttpPost("sectors/{sectorId}/seats")]
        [Authorize(Roles = "Admin")]
        [SwaggerOperation(Summary = "Crea un asiento para un sector")]
        [SwaggerResponse(StatusCodes.Status201Created, "Created")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Not Found")]
        [ProducesResponseType(typeof(SeatGetDTO), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateSeat([FromRoute] int sectorId, [FromBody] SeatCreateDTO seatCreateDTO)
        {
            var command = _mapper.Map<CreateSeatCommand>(seatCreateDTO);
            command.SectorId = sectorId;
            command.UserId = UserClaimsHelper.GetCurrentUserId(User);

            var result = await _createSeatHandler.Handle(command);

            return CreatedAtAction(nameof(GetSeatsBySector), new { sectorId }, result);
        }

        [HttpPost("sectors/{sectorId}/seats/bulk")]
        [Authorize(Roles = "Admin")]
        [SwaggerOperation(Summary = "Crea butacas masivamente para un sector")]
        [SwaggerResponse(StatusCodes.Status201Created, "Created")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Not Found")]
        [SwaggerResponse(StatusCodes.Status409Conflict, "Conflict")]
        [ProducesResponseType(typeof(List<SeatGetDTO>), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateSeatsBulk([FromRoute] int sectorId, [FromBody] SeatBulkCreateDTO seatBulkCreateDTO)
        {
            var command = _mapper.Map<CreateSeatsBulkCommand>(seatBulkCreateDTO);
            command.SectorId = sectorId;
            command.UserId = UserClaimsHelper.GetCurrentUserId(User);

            var result = await _createSeatsBulkHandler.Handle(command);

            return CreatedAtAction(nameof(GetSeatsBySector), new { sectorId }, result);
        }

        [HttpDelete("sectors/{sectorId}/seats/{seatId}")]
        [Authorize(Roles = "Admin")]
        [SwaggerOperation(Summary = "Elimina una butaca si no tiene reservas asociadas")]
        [SwaggerResponse(StatusCodes.Status204NoContent, "No Content")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Not Found")]
        [SwaggerResponse(StatusCodes.Status409Conflict, "Conflict")]
        public async Task<IActionResult> DeleteSeat([FromRoute] int sectorId, [FromRoute] Guid seatId)
        {
            await _deleteSeatHandler.Handle(new DeleteSeatCommand
            {
                SectorId = sectorId,
                SeatId = seatId,
                UserId = UserClaimsHelper.GetCurrentUserId(User)
            });

            return NoContent();
        }
    }
}
