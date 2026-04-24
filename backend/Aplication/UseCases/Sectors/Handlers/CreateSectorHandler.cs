using AutoMapper;
using Microsoft.Extensions.Options;
using TP_PROYECTO_SOFTWARE.Aplication.Configuration;
using TP_PROYECTO_SOFTWARE.Aplication.DTOs.SectorDTOs;
using TP_PROYECTO_SOFTWARE.Aplication.IHandlers;
using TP_PROYECTO_SOFTWARE.Aplication.IRepository.ICommand;
using TP_PROYECTO_SOFTWARE.Aplication.IRepository.IQuery;
using TP_PROYECTO_SOFTWARE.Aplication.UseCases.AuditLogs.Commands;
using TP_PROYECTO_SOFTWARE.Aplication.UseCases.Sectors.Commands;
using TP_PROYECTO_SOFTWARE.Domain.Models;

namespace TP_PROYECTO_SOFTWARE.Aplication.UseCases.Sectors.Handlers
{
    public class CreateSectorHandler : ICreateSectorHandler
    {
        private readonly TicketingRulesOptions _ticketingRules;
        private readonly IRepositoryEventQuery _repositoryEventQuery;
        private readonly IRepositorySectorQuery _repositorySectorQuery;
        private readonly IRepositorySectorCommand _repositorySectorCommand;
        private readonly ICreateAuditLogHandler _createAuditLogHandler;
        private readonly IMapper _mapper;

        public CreateSectorHandler(
            IOptions<TicketingRulesOptions> ticketingRules,
            IRepositoryEventQuery repositoryEventQuery,
            IRepositorySectorQuery repositorySectorQuery,
            IRepositorySectorCommand repositorySectorCommand,
            ICreateAuditLogHandler createAuditLogHandler,
            IMapper mapper)
        {
            _ticketingRules = ticketingRules.Value;
            _repositoryEventQuery = repositoryEventQuery;
            _repositorySectorQuery = repositorySectorQuery;
            _repositorySectorCommand = repositorySectorCommand;
            _createAuditLogHandler = createAuditLogHandler;
            _mapper = mapper;
        }

        public async Task<SectorGetDTO> Handle(CreateSectorCommand command)
        {
            var eventEntity = await GetEventOrThrow(command.EventId);

            await ValidateMaxSectorsPerEvent(command.EventId);

            var sector = BuildSector(command, eventEntity.Id);

            await _repositorySectorCommand.Create(sector);
            await CreateAuditLog(command.UserId, sector);
            await _repositorySectorCommand.Save();

            return _mapper.Map<SectorGetDTO>(sector);
        }

        private async Task<Domain.Models.EVENT> GetEventOrThrow(int eventId) => await _repositoryEventQuery.GetById(eventId)
            ?? throw new KeyNotFoundException("Evento no encontrado.");

        private async Task ValidateMaxSectorsPerEvent(int eventId)
        {
            var sectorsCount = await _repositorySectorQuery.CountByEventId(eventId);
            if (sectorsCount >= _ticketingRules.MaxSectorsPerEvent)
            {
                throw new InvalidOperationException($"Un evento no puede tener más de {_ticketingRules.MaxSectorsPerEvent} sectores.");
            }
        }

        private static SECTOR BuildSector(CreateSectorCommand command, int eventId) => new()
        {
            EventId = eventId,
            Name = command.Name,
            Price = command.Price,
            Capacity = command.Capacity
        };

        private async Task CreateAuditLog(int? userId, SECTOR sector)
        {
            await _createAuditLogHandler.Handle(new CreateAuditLogCommand
            {
                UserId = userId,
                Action = "CreateSector",
                EntityType = "SECTOR",
                EntityId = sector.Id.ToString(),
                Details = $"Sector creado. EventId={sector.EventId}, Name={sector.Name}, Price={sector.Price}, Capacity={sector.Capacity}"
            });
        }
    }
}
