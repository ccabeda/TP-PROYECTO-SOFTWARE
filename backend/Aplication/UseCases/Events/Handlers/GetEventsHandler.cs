using AutoMapper;
using TP_PROYECTO_SOFTWARE.Aplication.IHandlers;
using TP_PROYECTO_SOFTWARE.Aplication.DTOs;
using TP_PROYECTO_SOFTWARE.Aplication.DTOs.EventDTOs;
using TP_PROYECTO_SOFTWARE.Aplication.IRepository.IQuery;
using TP_PROYECTO_SOFTWARE.Aplication.UseCases.Events.Queries;
using System;
using System.Linq;

namespace TP_PROYECTO_SOFTWARE.Aplication.UseCases.Events.Handlers
{
    public class GetEventsHandler : IGetEventsHandler
    {
        private readonly IRepositoryEventQuery _repositoryEventQuery;
        private readonly IMapper _mapper;

        public GetEventsHandler(IRepositoryEventQuery repositoryEventQuery, IMapper mapper)
        {
            _repositoryEventQuery = repositoryEventQuery;
            _mapper = mapper;
        }

        public async Task<PagedResultDTO<EventGetDTO>> Handle(GetEventsQuery query)
        {
            var normalizedQuery = new GetEventsQuery
            {
                Name = query.Name,
                EventDate = query.EventDate,
                Page = query.Page < 1 ? 1 : query.Page,
                PageSize = Math.Clamp(query.PageSize, 1, 50)
            };

            var (events, totalCount) = await _repositoryEventQuery.GetAll(normalizedQuery);
            var eventDtos = _mapper.Map<List<EventGetDTO>>(events);

            for (var index = 0; index < events.Count; index++)
            {
                eventDtos[index].Status = ResolveDisplayStatus(events[index]);
            }

            return new PagedResultDTO<EventGetDTO>
            {
                Items = eventDtos,
                TotalCount = totalCount,
                Page = normalizedQuery.Page,
                PageSize = normalizedQuery.PageSize,
                TotalPages = (int)Math.Ceiling((double)totalCount / normalizedQuery.PageSize)
            };
        }

        private static string ResolveDisplayStatus(Domain.Models.Event eventEntity)
        {
            var allSeats = eventEntity.Sectors.SelectMany(sector => sector.Seats).ToList();

            if (allSeats.Count == 0)
            {
                return eventEntity.Status;
            }

            var hasAvailableSeats = allSeats.Any(seat =>
                string.Equals(seat.Status, "Available", StringComparison.OrdinalIgnoreCase));

            return hasAvailableSeats ? eventEntity.Status : "SoldOut";
        }
    }
}
