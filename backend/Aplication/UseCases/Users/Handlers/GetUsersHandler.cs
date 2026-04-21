using AutoMapper;
using TP_PROYECTO_SOFTWARE.Aplication.DTOs.UserDTOs;
using TP_PROYECTO_SOFTWARE.Aplication.IHandlers;
using TP_PROYECTO_SOFTWARE.Aplication.IRepository.IQuery;
using TP_PROYECTO_SOFTWARE.Aplication.UseCases.Users.Queries;

namespace TP_PROYECTO_SOFTWARE.Aplication.UseCases.Users.Handlers
{
    public class GetUsersHandler : IGetUsersHandler
    {
        private readonly IRepositoryUserQuery _repositoryUserQuery;
        private readonly IMapper _mapper;

        public GetUsersHandler(IRepositoryUserQuery repositoryUserQuery, IMapper mapper)
        {
            _repositoryUserQuery = repositoryUserQuery;
            _mapper = mapper;
        }

        public async Task<List<UserGetDTO>> Handle(GetUsersQuery query)
        {
            var users = await _repositoryUserQuery.GetAll();
            return _mapper.Map<List<UserGetDTO>>(users);
        }
    }
}
