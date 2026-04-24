using AutoMapper;
using TP_PROYECTO_SOFTWARE.Aplication.DTOs.EventDTOs;
using TP_PROYECTO_SOFTWARE.Aplication.IHandlers;
using TP_PROYECTO_SOFTWARE.Aplication.IRepository.ICommand;
using TP_PROYECTO_SOFTWARE.Aplication.UseCases.AuditLogs.Commands;
using TP_PROYECTO_SOFTWARE.Aplication.UseCases.Events.Commands;
using TP_PROYECTO_SOFTWARE.Domain.Models;

namespace TP_PROYECTO_SOFTWARE.Aplication.UseCases.Events.Handlers
{
    public class CreateEventHandler : ICreateEventHandler
    {
        private readonly IRepositoryEventCommand _repositoryEventCommand;
        private readonly ICreateAuditLogHandler _createAuditLogHandler;
        private readonly IMapper _mapper;

        public CreateEventHandler(IRepositoryEventCommand repositoryEventCommand, ICreateAuditLogHandler createAuditLogHandler, IMapper mapper)
        {
            _repositoryEventCommand = repositoryEventCommand;
            _createAuditLogHandler = createAuditLogHandler;
            _mapper = mapper;
        }

        public async Task<EventGetDTO> Handle(CreateEventCommand command)
        {
            var eventEntity = BuildEvent(command);

            await _repositoryEventCommand.Create(eventEntity);
            await CreateAuditLog(command.UserId, eventEntity);
            await _repositoryEventCommand.Save();

            return _mapper.Map<EventGetDTO>(eventEntity);
        }

        private static EVENT BuildEvent(CreateEventCommand command) => new()
        {
            Name = command.Name,
            EventDate = command.EventDate,
            Venue = command.Venue,
            Status = command.Status
        };

        private async Task CreateAuditLog(int? userId, EVENT eventEntity)
        {
            await _createAuditLogHandler.Handle(new CreateAuditLogCommand
            {
                UserId = userId,
                Action = "CreateEvent",
                EntityType = "EVENT",
                EntityId = eventEntity.Id.ToString(),
                Details = $"Evento creado. Name={eventEntity.Name}, Venue={eventEntity.Venue}, EventDate={eventEntity.EventDate:O}, Status={eventEntity.Status}"
            });
        }
    }
}
