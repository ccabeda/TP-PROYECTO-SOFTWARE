using TP_PROYECTO_SOFTWARE.Aplication.IRepository.ICommand;
using TP_PROYECTO_SOFTWARE.Aplication.IUnitOfWork;
using TP_PROYECTO_SOFTWARE.Infraestructure.Persistence;

namespace TP_PROYECTO_SOFTWARE.Infraestructure.UnitOfWork
{
    public class UnitOfWorkReservationCommand : IUnitOfWorkReservationCommand //junto los metodos para hacer reservacion, asi en los usecase no llamo a los 3 repositorys
    {
        private readonly AplicationDbContext _context;

        public UnitOfWorkReservationCommand(
            AplicationDbContext context,
            IRepositoryReservationCommand repositoryReservationCommand,
            IRepositorySeatCommand repositorySeatCommand,
            IRepositoryAuditLogCommand repositoryAuditLogCommand)
        {
            _context = context;
            RepositoryReservationCommand = repositoryReservationCommand;
            RepositorySeatCommand = repositorySeatCommand;
            RepositoryAuditLogCommand = repositoryAuditLogCommand;
        }

        public IRepositoryReservationCommand RepositoryReservationCommand { get; }
        public IRepositorySeatCommand RepositorySeatCommand { get; }
        public IRepositoryAuditLogCommand RepositoryAuditLogCommand { get; }

        public async Task Save() => await _context.SaveChangesAsync();
    }
}
