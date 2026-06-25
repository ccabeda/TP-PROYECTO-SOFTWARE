using TP_PROYECTO_SOFTWARE.Application.UseCases.Users.Queries;
using TP_PROYECTO_SOFTWARE.Domain.Models;

namespace TP_PROYECTO_SOFTWARE.Application.IRepository.IQuery
{
    public interface IRepositoryUserQuery
    {
        Task<(List<User> Items, int TotalCount)> GetAll(GetUsersQuery query);
        Task<User?> GetById(int id);
        Task<User?> GetByEmail(string email);
        Task<User?> GetByValidRefreshTokenHash(string refreshTokenHash, DateTime utcNow);
    }
}
