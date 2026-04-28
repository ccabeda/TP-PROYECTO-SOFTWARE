using TP_PROYECTO_SOFTWARE.Aplication.UseCases.Users.Queries;
using TP_PROYECTO_SOFTWARE.Domain.Models;

namespace TP_PROYECTO_SOFTWARE.Aplication.IRepository.IQuery
{
    public interface IRepositoryUserQuery
    {
        Task<(List<User> Items, int TotalCount)> GetAll(GetUsersQuery query);
        Task<User?> GetById(int id);
        Task<User?> GetByEmail(string email);
    }
}
