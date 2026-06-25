using TP_PROYECTO_SOFTWARE.Domain.Models;

namespace TP_PROYECTO_SOFTWARE.Application.IRepository.ICommand
{
    public interface IRepositorySectorCommand
    {
        Task Create(Sector sector);
        Task Delete(Sector sector);
        Task DeleteRange(IEnumerable<Sector> sectors);
        Task Save();
    }
}
