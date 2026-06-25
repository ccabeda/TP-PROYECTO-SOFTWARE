using TP_PROYECTO_SOFTWARE.Domain.Models;

namespace TP_PROYECTO_SOFTWARE.Application.IRepository.ICommand
{
    public interface IRepositoryUserCommand
    {
        Task Create(User user);
        Task Save();
    }
}
