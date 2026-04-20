using TP_PROYECTO_SOFTWARE.Aplication.DTOs.UserDTOs;
using TP_PROYECTO_SOFTWARE.Aplication.UseCases.Users.Queries;

namespace TP_PROYECTO_SOFTWARE.Aplication.IHandlers
{
    public interface IGetUsersHandler
    {
        Task<List<UserGetDTO>> Handle(GetUsersQuery query);
    }
}
