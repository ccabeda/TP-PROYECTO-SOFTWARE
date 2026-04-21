using TP_PROYECTO_SOFTWARE.Domain.Models;

namespace TP_PROYECTO_SOFTWARE.Aplication.IRepository.IQuery
{
    public interface IRepositoryUserQuery
    {
        Task<List<USER>> GetAll();
        Task<USER?> GetById(int id);
        Task<USER?> GetByEmail(string email);
    }
}
