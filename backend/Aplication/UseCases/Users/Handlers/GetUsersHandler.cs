using AutoMapper;
using TP_PROYECTO_SOFTWARE.Aplication.DTOs;
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

        public async Task<PagedResultDTO<UserGetDTO>> Handle(GetUsersQuery query)
        {
            var normalizedPage = Math.Max(query.Page, 1);
            var normalizedPageSize = Math.Clamp(query.PageSize, 1, 50);
            var normalizedQuery = query with
            {
                Page = normalizedPage,
                PageSize = normalizedPageSize
            };

            var (users, totalCount) = await _repositoryUserQuery.GetAll(normalizedQuery);
            var items = _mapper.Map<List<UserGetDTO>>(users);

            return new PagedResultDTO<UserGetDTO>
            {
                Items = items,
                TotalCount = totalCount,
                Page = normalizedPage,
                PageSize = normalizedPageSize,
                TotalPages = (int)Math.Ceiling(totalCount / (double)normalizedPageSize)
            };
        }
    }
}
