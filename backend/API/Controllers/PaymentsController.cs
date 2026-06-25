using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TP_PROYECTO_SOFTWARE.API.Helpers;
using TP_PROYECTO_SOFTWARE.Application.DTOs.PaymentDTOs;
using TP_PROYECTO_SOFTWARE.Application.DTOs.ReservationDTOs;
using TP_PROYECTO_SOFTWARE.Application.IHandlers;
using TP_PROYECTO_SOFTWARE.Application.UseCases.Reservations.Commands;

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
        [SwaggerResponse(StatusCodes.Status201Created, "Created")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Not Found")]
        [SwaggerResponse(StatusCodes.Status409Conflict, "Conflict")]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "Forbidden")]
        [ProducesResponseType(typeof(ReservationGetDTO), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreatePayment([FromBody] PaymentCreateDTO paymentCreateDTO)
        {
            var command = _mapper.Map<ConfirmReservationPaymentCommand>(paymentCreateDTO);
            command.CurrentUserId = UserClaimsHelper.GetRequiredCurrentUserId(User);
            command.IsAdmin = UserClaimsHelper.IsAdmin(User);

            var result = await _confirmReservationPaymentHandler.Handle(command);

            return StatusCode(StatusCodes.Status201Created, result);
        }
    }
}
