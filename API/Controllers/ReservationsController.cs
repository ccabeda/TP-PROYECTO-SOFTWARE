using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TP_PROYECTO_SOFTWARE.Aplication.DTOs.ReservationDTOs;
using TP_PROYECTO_SOFTWARE.Aplication.UseCases.Reservations.Commands;
using TP_PROYECTO_SOFTWARE.Aplication.UseCases.Reservations.Handlers;

namespace TP_PROYECTO_SOFTWARE.API.Controllers
{
    [Route("api/v1/reservations")]
    [ApiController]
    [Tags("Reservations")]
    public class ReservationsController : ControllerBase
    {
        private readonly CreateReservationHandler _handler;
        private readonly IMapper _mapper;

        public ReservationsController(CreateReservationHandler handler, IMapper mapper)
        {
            _handler = handler;
            _mapper = mapper;
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Intento básico de reserva")]
        [SwaggerResponse(StatusCodes.Status201Created, "Created")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Not Found")]
        [SwaggerResponse(StatusCodes.Status409Conflict, "Conflict")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Bad Request")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal Server Error")]
        [ProducesResponseType(typeof(ReservationGetDTO), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateReservation([FromBody] ReservationCreateDTO reservationCreateDTO)
        {
            var command = _mapper.Map<CreateReservationCommand>(reservationCreateDTO);
            var result = await _handler.Handle(command);

            return CreatedAtAction(nameof(CreateReservation), new { id = result.Id }, result);
        }
    }
}
