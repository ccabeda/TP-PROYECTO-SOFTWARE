using TP_PROYECTO_SOFTWARE.Aplication.IRepository.ICommand;

namespace TP_PROYECTO_SOFTWARE.Aplication.IUnitOfWork
{
    public interface IUnitOfWorkReservationCommand
    {
        IRepositoryReservationCommand RepositoryReservationCommand { get; }
        IRepositorySeatCommand RepositorySeatCommand { get; }
        Task Save();
    }
}
