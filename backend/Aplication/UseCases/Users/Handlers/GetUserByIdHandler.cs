using AutoMapper;
using TP_PROYECTO_SOFTWARE.Aplication.DTOs.UserDTOs;
using TP_PROYECTO_SOFTWARE.Aplication.IHandlers;
using TP_PROYECTO_SOFTWARE.Aplication.IRepository.IQuery;
using TP_PROYECTO_SOFTWARE.Aplication.UseCases.Users.Queries;

namespace TP_PROYECTO_SOFTWARE.Aplication.UseCases.Users.Handlers
{
    public class GetUserByIdHandler : IGetUserByIdHandler
    {
        private readonly IRepositoryUserQuery _repositoryUserQuery;
        private readonly IMapper _mapper;

        public GetUserByIdHandler(IRepositoryUserQuery repositoryUserQuery, IMapper mapper)
        {
            _repositoryUserQuery = repositoryUserQuery;
            _mapper = mapper;
        }

        public async Task<UserGetDTO> Handle(GetUserByIdQuery query)
        {
            var user = await _repositoryUserQuery.GetById(query.Id)
                ?? throw new KeyNotFoundException("Usuario no encontrado.");

            return _mapper.Map<UserGetDTO>(user);
        }
    }
}
