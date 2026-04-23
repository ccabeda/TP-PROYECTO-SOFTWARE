using AutoMapper;
using TP_PROYECTO_SOFTWARE.Aplication.DTOs.EventDTOs;
using TP_PROYECTO_SOFTWARE.Aplication.IHandlers;
using TP_PROYECTO_SOFTWARE.Aplication.IRepository.IQuery;
using TP_PROYECTO_SOFTWARE.Aplication.UseCases.Events.Queries;

namespace TP_PROYECTO_SOFTWARE.Aplication.UseCases.Events.Handlers
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

            return _mapper.Map<EventGetDTO>(eventEntity);
        }
    }
}
