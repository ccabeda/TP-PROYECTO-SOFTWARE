using AutoMapper;
using Microsoft.Extensions.Options;
using TP_PROYECTO_SOFTWARE.Application.Configuration;
using TP_PROYECTO_SOFTWARE.Application.DTOs.SeatDTOs;
using TP_PROYECTO_SOFTWARE.Application.IHandlers;
using TP_PROYECTO_SOFTWARE.Application.IRepository.ICommand;
using TP_PROYECTO_SOFTWARE.Application.IRepository.IQuery;
using TP_PROYECTO_SOFTWARE.Application.Services.Seats;
using TP_PROYECTO_SOFTWARE.Application.UseCases.AuditLogs.Commands;
using TP_PROYECTO_SOFTWARE.Application.UseCases.Seats.Commands;
using TP_PROYECTO_SOFTWARE.Domain.Constants;
using TP_PROYECTO_SOFTWARE.Domain.Models;

namespace TP_PROYECTO_SOFTWARE.Application.UseCases.Seats.Handlers
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
                Status = SeatStatuses.Available,
                Version = 1
            };

            await _repositorySeatCommand.Create(seat);
            await _repositorySeatCommand.Save();
            await _createAuditLogHandler.Handle(new CreateAuditLogCommand
            {
                UserId = command.UserId,
                Action = AuditActions.CreateSeat,
                EntityType = AuditEntityTypes.Seat,
                EntityId = seat.Id.ToString(),
                Details = $"Butaca creada. SectorId={seat.SectorId}, RowIdentifier={seat.RowIdentifier}, SeatNumber={seat.SeatNumber}, Status={seat.Status}"
            });

            return _mapper.Map<SeatGetDTO>(seat);
        }
    }
}
