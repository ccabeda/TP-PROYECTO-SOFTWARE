using TP_PROYECTO_SOFTWARE.Domain.Models;

namespace TP_PROYECTO_SOFTWARE.Aplication.IRepository.ICommand
{
    public interface IRepositoryUserCommand
    {
        Task Create(USER user);
        Task Save();
    }
}
