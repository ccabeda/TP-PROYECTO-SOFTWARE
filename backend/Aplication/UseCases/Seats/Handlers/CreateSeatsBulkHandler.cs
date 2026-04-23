using AutoMapper;
using Microsoft.Extensions.Options;
using TP_PROYECTO_SOFTWARE.Aplication.Configuration;
using TP_PROYECTO_SOFTWARE.Aplication.DTOs.SeatDTOs;
using TP_PROYECTO_SOFTWARE.Aplication.IHandlers;
using TP_PROYECTO_SOFTWARE.Aplication.IRepository.ICommand;
using TP_PROYECTO_SOFTWARE.Aplication.IRepository.IQuery;
using TP_PROYECTO_SOFTWARE.Aplication.UseCases.AuditLogs.Commands;
using TP_PROYECTO_SOFTWARE.Aplication.UseCases.Seats.Commands;
using TP_PROYECTO_SOFTWARE.Domain.Models;

namespace TP_PROYECTO_SOFTWARE.Aplication.UseCases.Seats.Handlers
{
    public class CreateSeatsBulkHandler : ICreateSeatsBulkHandler
    {
        private readonly TicketingRulesOptions _ticketingRules;
        private readonly IRepositorySectorQuery _repositorySectorQuery;
        private readonly IRepositorySeatQuery _repositorySeatQuery;
        private readonly IRepositorySeatCommand _repositorySeatCommand;
        private readonly ICreateAuditLogHandler _createAuditLogHandler;
        private readonly IMapper _mapper;

        public CreateSeatsBulkHandler(
            IOptions<TicketingRulesOptions> ticketingRules,
            IRepositorySectorQuery repositorySectorQuery,
            IRepositorySeatQuery repositorySeatQuery,
            IRepositorySeatCommand repositorySeatCommand,
            ICreateAuditLogHandler createAuditLogHandler,
            IMapper mapper)
        {
            _ticketingRules = ticketingRules.Value;
            _repositorySectorQuery = repositorySectorQuery;
            _repositorySeatQuery = repositorySeatQuery;
            _repositorySeatCommand = repositorySeatCommand;
            _createAuditLogHandler = createAuditLogHandler;
            _mapper = mapper;
        }

        public async Task<List<SeatGetDTO>> Handle(CreateSeatsBulkCommand command)
        {
            var sector = await _repositorySectorQuery.GetById(command.SectorId)
                ?? throw new KeyNotFoundException("Sector no encontrado.");

            var normalizedRows = command.Rows
                .Select(row => row.Trim().ToUpperInvariant())
                .ToList();

            if (normalizedRows.Count == 0)
            {
                throw new InvalidOperationException("Debe indicar al menos una fila.");
            }

            if (normalizedRows.Distinct().Count() != normalizedRows.Count)
            {
                throw new InvalidOperationException("No se permiten filas repetidas.");
            }

            if (command.SeatsPerRow <= 0 || command.SeatsPerRow > _ticketingRules.MaxSeatsPerRow)
            {
                throw new InvalidOperationException($"La cantidad de asientos por fila debe estar entre 1 y {_ticketingRules.MaxSeatsPerRow}.");
            }

            var existingSeats = await _repositorySeatQuery.GetBySectorId(command.SectorId);
            var totalSeatsToCreate = normalizedRows.Count * command.SeatsPerRow;
            if (existingSeats.Count + totalSeatsToCreate > sector.Capacity)
            {
                throw new InvalidOperationException("La operación supera la capacidad máxima del sector.");
            }

            var existingSeatKeys = existingSeats
                .Select(s => $"{s.RowIdentifier.ToUpperInvariant()}-{s.SeatNumber}")
                .ToHashSet();

            var seatsToCreate = new List<SEAT>();

            foreach (var row in normalizedRows)
            {
                for (var seatNumber = 1; seatNumber <= command.SeatsPerRow; seatNumber++)
                {
                    var seatKey = $"{row}-{seatNumber}";
                    if (existingSeatKeys.Contains(seatKey))
                    {
                        throw new InvalidOperationException($"Ya existe la butaca {row}{seatNumber} en el sector.");
                    }

                    seatsToCreate.Add(new SEAT
                    {
                        SectorId = sector.Id,
                        RowIdentifier = row,
                        SeatNumber = seatNumber,
                        Status = "Available",
                        Version = 1
                    });
                }
            }

            foreach (var seat in seatsToCreate)
            {
                await _repositorySeatCommand.Create(seat);
            }

            await _createAuditLogHandler.Handle(new CreateAuditLogCommand
            {
                UserId = command.UserId,
                Action = "CreateSeatsBulk",
                EntityType = "SEAT",
                EntityId = command.SectorId.ToString(),
                Details = $"Creación masiva de butacas. SectorId={sector.Id}, Rows={string.Join(",", normalizedRows)}, SeatsPerRow={command.SeatsPerRow}, TotalCreated={seatsToCreate.Count}"
            });

            await _repositorySeatCommand.Save();

            return _mapper.Map<List<SeatGetDTO>>(seatsToCreate);
        }
    }
}
