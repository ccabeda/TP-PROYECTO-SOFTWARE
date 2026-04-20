using AutoMapper;
using TP_PROYECTO_SOFTWARE.Aplication.IHandlers;
using TP_PROYECTO_SOFTWARE.Aplication.DTOs.EventDTOs;
using TP_PROYECTO_SOFTWARE.Aplication.IRepository.IQuery;
using TP_PROYECTO_SOFTWARE.Aplication.UseCases.Events.Queries;

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

        public async Task<List<EventGetDTO>> Handle(GetEventsQuery query)
        {
            var events = await _repositoryEventQuery.GetAll(query);
            return _mapper.Map<List<EventGetDTO>>(events);
        }
    }
}
