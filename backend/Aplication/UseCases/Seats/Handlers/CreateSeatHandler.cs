using AutoMapper;
using Microsoft.Extensions.Options;
using TP_PROYECTO_SOFTWARE.Aplication.Configuration;
using TP_PROYECTO_SOFTWARE.Aplication.DTOs.SeatDTOs;
using TP_PROYECTO_SOFTWARE.Aplication.IHandlers;
using TP_PROYECTO_SOFTWARE.Aplication.IRepository.ICommand;
using TP_PROYECTO_SOFTWARE.Aplication.IRepository.IQuery;
using TP_PROYECTO_SOFTWARE.Aplication.Services.Seats;
using TP_PROYECTO_SOFTWARE.Aplication.UseCases.AuditLogs.Commands;
using TP_PROYECTO_SOFTWARE.Aplication.UseCases.Seats.Commands;
using TP_PROYECTO_SOFTWARE.Domain.Models;

namespace TP_PROYECTO_SOFTWARE.Aplication.UseCases.Seats.Handlers
{
    public class CreateSeatHandler : ICreateSeatHandler
    {
        private readonly ISeatRulesService _seatRulesService;
        private readonly IRepositorySeatQuery _repositorySeatQuery;
        private readonly IRepositorySeatCommand _repositorySeatCommand;
        private readonly ICreateAuditLogHandler _createAuditLogHandler;
        private readonly IMapper _mapper;

        public CreateSeatHandler(
            ISeatRulesService seatRulesService,
            IRepositorySeatQuery repositorySeatQuery,
            IRepositorySeatCommand repositorySeatCommand,
            ICreateAuditLogHandler createAuditLogHandler,
            IMapper mapper)
        {
            _seatRulesService = seatRulesService;
            _repositorySeatQuery = repositorySeatQuery;
            _repositorySeatCommand = repositorySeatCommand;
            _createAuditLogHandler = createAuditLogHandler;
            _mapper = mapper;
        }

        public async Task<SeatGetDTO> Handle(CreateSeatCommand command)
        {
            var sector = await _seatRulesService.GetSectorOrThrow(command.SectorId);

            await _seatRulesService.ValidateSectorCapacity(command.SectorId, sector.Capacity, 1);

            var normalizedRowIdentifier = _seatRulesService.NormalizeAndValidateRow(command.RowIdentifier);

            _seatRulesService.ValidateSeatNumber(command.SeatNumber);

            var duplicatedSeat = await _repositorySeatQuery.ExistsInSector(
                command.SectorId,
                normalizedRowIdentifier,
                command.SeatNumber);

            if (duplicatedSeat)
            {
                throw new InvalidOperationException($"Ya existe la butaca {normalizedRowIdentifier}{command.SeatNumber} en el sector.");
            }

            var seat = new Seat
            {
                SectorId = sector.Id,
                RowIdentifier = normalizedRowIdentifier,
                SeatNumber = command.SeatNumber,
                Status = "Available",
                Version = 1
            };

            await _repositorySeatCommand.Create(seat);
            await _createAuditLogHandler.Handle(new CreateAuditLogCommand
            {
                UserId = command.UserId,
                Action = "CreateSeat",
                EntityType = "Seat",
                EntityId = seat.Id.ToString(),
                Details = $"Butaca creada. SectorId={seat.SectorId}, RowIdentifier={seat.RowIdentifier}, SeatNumber={seat.SeatNumber}, Status={seat.Status}"
            });
            await _repositorySeatCommand.Save();

            return _mapper.Map<SeatGetDTO>(seat);
        }
    }
}
