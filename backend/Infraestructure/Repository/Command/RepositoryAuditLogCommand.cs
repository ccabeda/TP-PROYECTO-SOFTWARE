using TP_PROYECTO_SOFTWARE.Aplication.IRepository.ICommand;
using TP_PROYECTO_SOFTWARE.Domain.Models;
using TP_PROYECTO_SOFTWARE.Infraestructure.Persistence;

namespace TP_PROYECTO_SOFTWARE.Infraestructure.Repository.Command
{
    public class RepositoryAuditLogCommand : IRepositoryAuditLogCommand
    {
        private readonly AplicationDbContext _context;

        public RepositoryAuditLogCommand(AplicationDbContext context)
        {
            _context = context;
        }

        public async Task Create(AUDIT_LOG auditLog) => await _context.AUDIT_LOG.AddAsync(auditLog);
    }
}
