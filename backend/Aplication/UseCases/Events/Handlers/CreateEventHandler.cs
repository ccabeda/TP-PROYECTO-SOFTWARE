using AutoMapper;
using TP_PROYECTO_SOFTWARE.Aplication.DTOs.EventDTOs;
using TP_PROYECTO_SOFTWARE.Aplication.IHandlers;
using TP_PROYECTO_SOFTWARE.Aplication.IRepository.ICommand;
using TP_PROYECTO_SOFTWARE.Aplication.IRepository.IQuery;
using TP_PROYECTO_SOFTWARE.Aplication.UseCases.AuditLogs.Commands;
using TP_PROYECTO_SOFTWARE.Aplication.UseCases.Events.Commands;
using TP_PROYECTO_SOFTWARE.Domain.Models;

namespace TP_PROYECTO_SOFTWARE.Aplication.UseCases.Events.Handlers
{
    public class CreateEventHandler : ICreateEventHandler
    {
        private readonly IRepositoryEventCommand _repositoryEventCommand;
        private readonly IRepositoryEventQuery _repositoryEventQuery;
        private readonly ICreateAuditLogHandler _createAuditLogHandler;
        private readonly IMapper _mapper;

        public CreateEventHandler(
            IRepositoryEventCommand repositoryEventCommand,
            IRepositoryEventQuery repositoryEventQuery,
            ICreateAuditLogHandler createAuditLogHandler,
            IMapper mapper)
        {
            _repositoryEventCommand = repositoryEventCommand;
            _repositoryEventQuery = repositoryEventQuery;
            _createAuditLogHandler = createAuditLogHandler;
            _mapper = mapper;
        }

        public async Task<EventGetDTO> Handle(CreateEventCommand command)
        {
            await EnsureEventIsUnique(command);
            var eventEntity = BuildEvent(command);

            await _repositoryEventCommand.Create(eventEntity);
            await CreateAuditLog(command.UserId, eventEntity);
            await _repositoryEventCommand.Save();

            return _mapper.Map<EventGetDTO>(eventEntity);
        }

        private async Task EnsureEventIsUnique(CreateEventCommand command)
        {
            var alreadyExists = await _repositoryEventQuery.ExistsDuplicate(
                command.Name,
                command.Venue,
                command.EventDate);

            if (alreadyExists)
            {
                throw new InvalidOperationException("Ya existe un evento con el mismo nombre, venue y fecha.");
            }
        }

        private static Event BuildEvent(CreateEventCommand command) => new()
        {
            Name = command.Name.Trim(),
            EventDate = command.EventDate,
            Venue = command.Venue.Trim(),
            Status = command.Status.Trim(),
            ImageUrl = string.IsNullOrWhiteSpace(command.ImageUrl) ? null : command.ImageUrl.Trim(),
            Description = string.IsNullOrWhiteSpace(command.Description) ? null : command.Description.Trim()
        };

        private async Task CreateAuditLog(int? userId, Event eventEntity)
        {
            await _createAuditLogHandler.Handle(new CreateAuditLogCommand
            {
                UserId = userId,
                Action = "CreateEvent",
                EntityType = "Event",
                EntityId = eventEntity.Id.ToString(),
                Details = $"Evento creado. Name={eventEntity.Name}, Venue={eventEntity.Venue}, EventDate={eventEntity.EventDate:O}, Status={eventEntity.Status}, ImageUrl={eventEntity.ImageUrl}, DescriptionLength={eventEntity.Description?.Length ?? 0}"
            });
        }
    }
}
