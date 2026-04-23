using AutoMapper;
using TP_PROYECTO_SOFTWARE.Aplication.DTOs.SeatDTOs;
using TP_PROYECTO_SOFTWARE.Aplication.IHandlers;
using TP_PROYECTO_SOFTWARE.Aplication.IRepository.ICommand;
using TP_PROYECTO_SOFTWARE.Aplication.IRepository.IQuery;
using TP_PROYECTO_SOFTWARE.Aplication.UseCases.AuditLogs.Commands;
using TP_PROYECTO_SOFTWARE.Aplication.UseCases.Seats.Commands;
using TP_PROYECTO_SOFTWARE.Domain.Models;

namespace TP_PROYECTO_SOFTWARE.Aplication.UseCases.Seats.Handlers
{
    public class CreateSeatHandler : ICreateSeatHandler
    {
        private readonly IRepositorySectorQuery _repositorySectorQuery;
        private readonly IRepositorySeatQuery _repositorySeatQuery;
        private readonly IRepositorySeatCommand _repositorySeatCommand;
        private readonly ICreateAuditLogHandler _createAuditLogHandler;
        private readonly IMapper _mapper;

        public CreateSeatHandler(
            IRepositorySectorQuery repositorySectorQuery,
            IRepositorySeatQuery repositorySeatQuery,
            IRepositorySeatCommand repositorySeatCommand,
            ICreateAuditLogHandler createAuditLogHandler,
            IMapper mapper)
        {
            _repositorySectorQuery = repositorySectorQuery;
            _repositorySeatQuery = repositorySeatQuery;
            _repositorySeatCommand = repositorySeatCommand;
            _createAuditLogHandler = createAuditLogHandler;
            _mapper = mapper;
        }

        public async Task<SeatGetDTO> Handle(CreateSeatCommand command)
        {
            var sector = await _repositorySectorQuery.GetById(command.SectorId)
                ?? throw new KeyNotFoundException("Sector no encontrado.");

            var existingSeats = await _repositorySeatQuery.GetBySectorId(command.SectorId);
            if (existingSeats.Count >= sector.Capacity)
            {
                throw new InvalidOperationException("El sector ya alcanzó su capacidad máxima de asientos.");
            }

            var normalizedRowIdentifier = command.RowIdentifier.Trim().ToUpperInvariant();
            var duplicatedSeat = existingSeats.Any(s =>
                s.RowIdentifier.Equals(normalizedRowIdentifier, StringComparison.OrdinalIgnoreCase) &&
                s.SeatNumber == command.SeatNumber);

            if (duplicatedSeat)
            {
                throw new InvalidOperationException($"Ya existe la butaca {normalizedRowIdentifier}{command.SeatNumber} en el sector.");
            }

            var seat = new SEAT
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
                EntityType = "SEAT",
                EntityId = seat.Id.ToString(),
                Details = $"Butaca creada. SectorId={seat.SectorId}, RowIdentifier={seat.RowIdentifier}, SeatNumber={seat.SeatNumber}, Status={seat.Status}"
            });
            await _repositorySeatCommand.Save();

            return _mapper.Map<SeatGetDTO>(seat);
        }
    }
}
