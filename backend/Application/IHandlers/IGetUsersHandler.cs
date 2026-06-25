using TP_PROYECTO_SOFTWARE.Application.DTOs;
using TP_PROYECTO_SOFTWARE.Application.DTOs.UserDTOs;
using TP_PROYECTO_SOFTWARE.Application.UseCases.Users.Queries;

namespace TP_PROYECTO_SOFTWARE.Application.IHandlers
{
    public interface IGetUsersHandler
    {
        Task<PagedResultDTO<UserGetDTO>> Handle(GetUsersQuery query);
    }
}
