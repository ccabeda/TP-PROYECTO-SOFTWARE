using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TP_PROYECTO_SOFTWARE.API.Helpers;
using TP_PROYECTO_SOFTWARE.Aplication.DTOs.SectorDTOs;
using TP_PROYECTO_SOFTWARE.Aplication.IHandlers;
using TP_PROYECTO_SOFTWARE.Aplication.UseCases.Sectors.Commands;
using TP_PROYECTO_SOFTWARE.Aplication.UseCases.Sectors.Queries;

namespace TP_PROYECTO_SOFTWARE.API.Controllers
{
    [Route("api/v1/events/{eventId}/sectors")]
    [ApiController]
    [Tags("Sectors")]
    public class SectorsController : ControllerBase
    {
        private readonly IGetSectorsByEventHandler _getSectorsByEventHandler;
        private readonly IGetSectorByIdHandler _getSectorByIdHandler;
        private readonly ICreateSectorHandler _createSectorHandler;
        private readonly IDeleteSectorHandler _deleteSectorHandler;
        private readonly IMapper _mapper;

        public SectorsController(
            IGetSectorsByEventHandler getSectorsByEventHandler,
            IGetSectorByIdHandler getSectorByIdHandler,
            ICreateSectorHandler createSectorHandler,
            IDeleteSectorHandler deleteSectorHandler,
            IMapper mapper)
        {
            _getSectorsByEventHandler = getSectorsByEventHandler;
            _getSectorByIdHandler = getSectorByIdHandler;
            _createSectorHandler = createSectorHandler;
            _deleteSectorHandler = deleteSectorHandler;
            _mapper = mapper;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Listado de sectores por evento")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success")]
        [ProducesResponseType(typeof(List<SectorGetDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSectorsByEvent([FromRoute] int eventId)
        {
            var result = await _getSectorsByEventHandler.Handle(new GetSectorsByEventQuery { EventId = eventId });
            return Ok(result);
        }

        [HttpGet("{sectorId}")]
        [SwaggerOperation(Summary = "Obtiene un sector por id dentro de un evento")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Not Found")]
        [ProducesResponseType(typeof(SectorGetDTO), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSectorById([FromRoute] int eventId, [FromRoute] int sectorId)
        {
            var result = await _getSectorByIdHandler.Handle(new GetSectorByIdQuery
            {
                EventId = eventId,
                SectorId = sectorId
            });
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [SwaggerOperation(Summary = "Crea un sector para un evento")]
        [SwaggerResponse(StatusCodes.Status201Created, "Created")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Not Found")]
        [ProducesResponseType(typeof(SectorGetDTO), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateSector([FromRoute] int eventId, [FromBody] SectorCreateDTO sectorCreateDTO)
        {
            var command = _mapper.Map<CreateSectorCommand>(sectorCreateDTO);
            command.EventId = eventId;
            command.UserId = UserClaimsHelper.GetCurrentUserId(User);

            var result = await _createSectorHandler.Handle(command);

            return CreatedAtAction(nameof(GetSectorById), new { eventId, sectorId = result.Id }, result);
        }

        [HttpDelete("{sectorId}")]
        [Authorize(Roles = "Admin")]
        [SwaggerOperation(Summary = "Elimina un sector y sus asientos si no tiene reservas asociadas")]
        [SwaggerResponse(StatusCodes.Status204NoContent, "No Content")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Not Found")]
        [SwaggerResponse(StatusCodes.Status409Conflict, "Conflict")]
        public async Task<IActionResult> DeleteSector([FromRoute] int eventId, [FromRoute] int sectorId)
        {
            await _deleteSectorHandler.Handle(new DeleteSectorCommand
            {
                EventId = eventId,
                SectorId = sectorId,
                UserId = UserClaimsHelper.GetCurrentUserId(User)
            });

            return NoContent();
        }
    }
}
