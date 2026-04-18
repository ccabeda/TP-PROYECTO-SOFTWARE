using TP_PROYECTO_SOFTWARE.Domain.Models;

namespace TP_PROYECTO_SOFTWARE.Aplication.IRepository.IQuery
{
    public interface IRepositoryUserQuery
    {
        Task<USER?> GetById(int id);
    }
}
