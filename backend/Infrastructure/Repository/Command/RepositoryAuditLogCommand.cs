using TP_PROYECTO_SOFTWARE.Application.IRepository.ICommand;
using TP_PROYECTO_SOFTWARE.Domain.Models;
using TP_PROYECTO_SOFTWARE.Infrastructure.Persistence;

namespace TP_PROYECTO_SOFTWARE.Infrastructure.Repository.Command
{
    public class RepositoryAuditLogCommand : IRepositoryAuditLogCommand
    {
        private readonly ApplicationDbContext _context;

        public RepositoryAuditLogCommand(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Create(AuditLog auditLog) => await _context.AuditLogs.AddAsync(auditLog);
        public async Task Save() => await _context.SaveChangesAsync();
    }
}


