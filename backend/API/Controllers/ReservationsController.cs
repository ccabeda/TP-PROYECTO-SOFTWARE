using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TP_PROYECTO_SOFTWARE.Aplication.DTOs.ReservationDTOs;
using TP_PROYECTO_SOFTWARE.Aplication.IHandlers;
using TP_PROYECTO_SOFTWARE.Aplication.UseCases.Reservations.Commands;
using TP_PROYECTO_SOFTWARE.Aplication.UseCases.Reservations.Queries;

namespace TP_PROYECTO_SOFTWARE.API.Controllers
{
    [Route("api/v1/reservations")]
    [ApiController]
    [Tags("Reservations")]
    public class ReservationsController : ControllerBase
    {
        private readonly ICreateReservationHandler _handler;
        private readonly IConfirmReservationPaymentHandler _confirmReservationPaymentHandler;
        private readonly IGetReservationByIdHandler _getReservationByIdHandler;
        private readonly IMapper _mapper;

        public ReservationsController(
            ICreateReservationHandler handler,
            IConfirmReservationPaymentHandler confirmReservationPaymentHandler,
            IGetReservationByIdHandler getReservationByIdHandler,
            IMapper mapper)
        {
            _handler = handler;
            _confirmReservationPaymentHandler = confirmReservationPaymentHandler;
            _getReservationByIdHandler = getReservationByIdHandler;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Obtiene una reserva por id")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Not Found")]
        [ProducesResponseType(typeof(ReservationGetDTO), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetReservationById([FromRoute] Guid id)
        {
            var result = await _getReservationByIdHandler.Handle(new GetReservationByIdQuery { Id = id });
            return Ok(result);
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

            return CreatedAtAction(nameof(GetReservationById), new { id = result.Id }, result);
        }

        [HttpPost("{id}/payment")]
        [SwaggerOperation(Summary = "Confirma el pago simulado de una reserva")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Not Found")]
        [SwaggerResponse(StatusCodes.Status409Conflict, "Conflict")]
        [ProducesResponseType(typeof(ReservationGetDTO), StatusCodes.Status200OK)]
        public async Task<IActionResult> ConfirmReservationPayment([FromRoute] Guid id)
        {
            var result = await _confirmReservationPaymentHandler.Handle(new ConfirmReservationPaymentCommand
            {
                ReservationId = id
            });

            return Ok(result);
        }
    }
}
