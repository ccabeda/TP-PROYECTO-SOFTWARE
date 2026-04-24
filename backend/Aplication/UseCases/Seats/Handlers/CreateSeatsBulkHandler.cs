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
    public class CreateSeatsBulkHandler : ICreateSeatsBulkHandler
    {
        private readonly ISeatRulesService _seatRulesService;
        private readonly IRepositorySeatQuery _repositorySeatQuery;
        private readonly IRepositorySeatCommand _repositorySeatCommand;
        private readonly ICreateAuditLogHandler _createAuditLogHandler;
        private readonly IMapper _mapper;

        public CreateSeatsBulkHandler(
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

        public async Task<List<SeatGetDTO>> Handle(CreateSeatsBulkCommand command)
        {
            var sector = await _seatRulesService.GetSectorOrThrow(command.SectorId);
            var generatedRows = _seatRulesService.GetGeneratedRows(command.RowCount);

            _seatRulesService.ValidateSeatNumber(command.SeatsPerRow);
            await _seatRulesService.ValidateSectorCapacity(command.SectorId, sector.Capacity, generatedRows.Count * command.SeatsPerRow);

            var existingSeatKeys = await GetExistingSeatKeys(command.SectorId, generatedRows, command.SeatsPerRow);
            var seatsToCreate = BuildSeatsToCreate(sector.Id, generatedRows, command.SeatsPerRow, existingSeatKeys);

            await PersistSeats(seatsToCreate, command, sector.Id, generatedRows);

            return _mapper.Map<List<SeatGetDTO>>(seatsToCreate);
        }

        private async Task<HashSet<string>> GetExistingSeatKeys(int sectorId, List<string> generatedRows, int seatsPerRow)
        {
            var existingSeats = await _repositorySeatQuery.GetExistingInSector(
                sectorId,
                generatedRows,
                seatsPerRow);

            return existingSeats
                .Select(s => $"{s.RowIdentifier.ToUpperInvariant()}-{s.SeatNumber}")
                .ToHashSet();
        }

        private static List<SEAT> BuildSeatsToCreate(int sectorId, List<string> generatedRows, int seatsPerRow, HashSet<string> existingSeatKeys)
        {
            var seatsToCreate = new List<SEAT>();

            foreach (var row in generatedRows)
            {
                for (var seatNumber = 1; seatNumber <= seatsPerRow; seatNumber++)
                {
                    var seatKey = $"{row}-{seatNumber}";
                    if (existingSeatKeys.Contains(seatKey))
                    {
                        throw new InvalidOperationException($"Ya existe la butaca {row}{seatNumber} en el sector.");
                    }

                    seatsToCreate.Add(new SEAT
                    {
                        SectorId = sectorId,
                        RowIdentifier = row,
                        SeatNumber = seatNumber,
                        Status = "Available",
                        Version = 1
                    });
                }
            }

            return seatsToCreate;
        }

        private async Task PersistSeats(List<SEAT> seatsToCreate, CreateSeatsBulkCommand command, int sectorId, List<string> generatedRows)
        {
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
                Details = $"Creación masiva de butacas. SectorId={sectorId}, RowCount={command.RowCount}, GeneratedRows={string.Join(",", generatedRows)}, SeatsPerRow={command.SeatsPerRow}, TotalCreated={seatsToCreate.Count}"
            });

            await _repositorySeatCommand.Save();
        }
    }
}
