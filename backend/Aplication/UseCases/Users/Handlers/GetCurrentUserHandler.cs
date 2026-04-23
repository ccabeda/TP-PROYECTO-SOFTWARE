using AutoMapper;
using TP_PROYECTO_SOFTWARE.Aplication.DTOs.UserDTOs;
using TP_PROYECTO_SOFTWARE.Aplication.IHandlers;
using TP_PROYECTO_SOFTWARE.Aplication.IRepository.IQuery;
using TP_PROYECTO_SOFTWARE.Aplication.UseCases.Users.Queries;

namespace TP_PROYECTO_SOFTWARE.Aplication.UseCases.Users.Handlers
{
    public class GetCurrentUserHandler : IGetCurrentUserHandler
    {
        private readonly IRepositoryUserQuery _repositoryUserQuery;
        private readonly IMapper _mapper;

        public GetCurrentUserHandler(IRepositoryUserQuery repositoryUserQuery, IMapper mapper)
        {
            _repositoryUserQuery = repositoryUserQuery;
            _mapper = mapper;
        }

        public async Task<UserGetDTO> Handle(GetCurrentUserQuery query)
        {
            var user = await _repositoryUserQuery.GetById(query.UserId)
                ?? throw new KeyNotFoundException("Usuario no encontrado.");

            return _mapper.Map<UserGetDTO>(user);
        }
    }
}
