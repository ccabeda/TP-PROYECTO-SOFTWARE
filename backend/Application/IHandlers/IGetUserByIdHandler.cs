using TP_PROYECTO_SOFTWARE.Application.DTOs.UserDTOs;
using TP_PROYECTO_SOFTWARE.Application.UseCases.Users.Queries;

namespace TP_PROYECTO_SOFTWARE.Application.IHandlers
{
    public interface IGetUserByIdHandler
    {
        Task<UserGetDTO> Handle(GetUserByIdQuery query);
    }
}
