using TP_PROYECTO_SOFTWARE.Domain.Models;

namespace TP_PROYECTO_SOFTWARE.Aplication.IRepository.ICommand
{
    public interface IRepositorySectorCommand
    {
        Task Create(SECTOR sector);
        Task Delete(SECTOR sector);
        Task DeleteRange(IEnumerable<SECTOR> sectors);
        Task Save();
    }
}
