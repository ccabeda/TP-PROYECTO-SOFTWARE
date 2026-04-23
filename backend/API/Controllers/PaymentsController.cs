using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TP_PROYECTO_SOFTWARE.API.Helpers;
using TP_PROYECTO_SOFTWARE.Aplication.DTOs.PaymentDTOs;
using TP_PROYECTO_SOFTWARE.Aplication.DTOs.ReservationDTOs;
using TP_PROYECTO_SOFTWARE.Aplication.IHandlers;
using TP_PROYECTO_SOFTWARE.Aplication.UseCases.Reservations.Commands;

namespace TP_PROYECTO_SOFTWARE.API.Controllers
{
    [Route("api/v1/payments")]
    [ApiController]
    [Tags("Payments")]
    public class PaymentsController : ControllerBase
    {
        private readonly IConfirmReservationPaymentHandler _confirmReservationPaymentHandler;
        private readonly IMapper _mapper;

        public PaymentsController(
            IConfirmReservationPaymentHandler confirmReservationPaymentHandler,
            IMapper mapper)
        {
            _confirmReservationPaymentHandler = confirmReservationPaymentHandler;
            _mapper = mapper;
        }

        [HttpPost]
        [Authorize]
        [SwaggerOperation(Summary = "Crea un pago simulado para una reserva")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Not Found")]
        [SwaggerResponse(StatusCodes.Status409Conflict, "Conflict")]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "Forbidden")]
        [ProducesResponseType(typeof(ReservationGetDTO), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreatePayment([FromBody] PaymentCreateDTO paymentCreateDTO)
        {
            var currentUserId = UserClaimsHelper.GetCurrentUserId(User)
                ?? throw new UnauthorizedAccessException("Usuario no autenticado.");

            var command = _mapper.Map<ConfirmReservationPaymentCommand>(paymentCreateDTO);
            command.CurrentUserId = currentUserId;
            command.IsAdmin = UserClaimsHelper.IsAdmin(User);

            var result = await _confirmReservationPaymentHandler.Handle(command);

            return Ok(result);
        }
    }
}
