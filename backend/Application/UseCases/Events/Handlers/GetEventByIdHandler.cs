using AutoMapper;
using TP_PROYECTO_SOFTWARE.Application.DTOs.EventDTOs;
using TP_PROYECTO_SOFTWARE.Application.IHandlers;
using TP_PROYECTO_SOFTWARE.Application.IRepository.IQuery;
using TP_PROYECTO_SOFTWARE.Application.UseCases.Events.Queries;
using System;
using System.Linq;
using TP_PROYECTO_SOFTWARE.Domain.Constants;

namespace TP_PROYECTO_SOFTWARE.Application.UseCases.Events.Handlers
{
    public class GetEventByIdHandler : IGetEventByIdHandler
    {
        private readonly IRepositoryEventQuery _repositoryEventQuery;
        private readonly IMapper _mapper;

        public GetEventByIdHandler(IRepositoryEventQuery repositoryEventQuery, IMapper mapper)
        {
            _repositoryEventQuery = repositoryEventQuery;
            _mapper = mapper;
        }

        public async Task<EventGetDTO> Handle(GetEventByIdQuery query)
        {
            var eventEntity = await _repositoryEventQuery.GetById(query.Id)
                ?? throw new KeyNotFoundException("Evento no encontrado.");

            var eventDto = _mapper.Map<EventGetDTO>(eventEntity);
            eventDto.Status = ResolveDisplayStatus(eventEntity);

            return eventDto;
        }

        private static string ResolveDisplayStatus(Domain.Models.Event eventEntity)
        {
            var allSeats = eventEntity.Sectors.SelectMany(sector => sector.Seats).ToList();

            if (allSeats.Count == 0)
            {
                return eventEntity.Status;
            }

            var hasAvailableSeats = allSeats.Any(seat =>
                string.Equals(seat.Status, SeatStatuses.Available, StringComparison.OrdinalIgnoreCase));

            return hasAvailableSeats ? eventEntity.Status : EventDisplayStatuses.SoldOut;
        }
    }
}
