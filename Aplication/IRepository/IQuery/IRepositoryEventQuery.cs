using TP_PROYECTO_SOFTWARE.Domain.Models;

namespace TP_PROYECTO_SOFTWARE.Aplication.IRepository.IQuery
{
    public interface IRepositoryEventQuery
    {
        Task<EVENT?> GetById(int id);
        Task<List<EVENT>> GetAll();
    }
}
