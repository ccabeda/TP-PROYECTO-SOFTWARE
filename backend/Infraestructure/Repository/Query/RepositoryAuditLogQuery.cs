using Microsoft.EntityFrameworkCore;
using TP_PROYECTO_SOFTWARE.Aplication.IRepository.IQuery;
using TP_PROYECTO_SOFTWARE.Aplication.UseCases.AuditLogs.Queries;
using TP_PROYECTO_SOFTWARE.Domain.Models;
using TP_PROYECTO_SOFTWARE.Infraestructure.Persistence;

namespace TP_PROYECTO_SOFTWARE.Infraestructure.Repository.Query
{
    public class RepositoryAuditLogQuery : IRepositoryAuditLogQuery
    {
        private readonly AplicationDbContext _context;

        public RepositoryAuditLogQuery(AplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<AUDIT_LOG>> GetAll(GetAuditLogsQuery query)
        {
            var auditLogsQuery = _context.AUDIT_LOG
                .Include(a => a.User)
                .AsQueryable();

            if (query.UserId.HasValue)
            {
                auditLogsQuery = auditLogsQuery.Where(a => a.UserId == query.UserId.Value);
            }

            if (query.Date.HasValue)
            {
                var date = query.Date.Value.Date;
                var endOfDay = date.AddDays(1).AddTicks(-1);
                auditLogsQuery = auditLogsQuery.Where(a => a.CreatedAt >= date && a.CreatedAt <= endOfDay);
            }

            if (query.DateFrom.HasValue)
            {
                var dateFrom = query.DateFrom.Value.Date;
                auditLogsQuery = auditLogsQuery.Where(a => a.CreatedAt >= dateFrom);
            }

            if (query.DateTo.HasValue)
            {
                var dateTo = query.DateTo.Value.Date.AddDays(1).AddTicks(-1);
                auditLogsQuery = auditLogsQuery.Where(a => a.CreatedAt <= dateTo);
            }

            return await auditLogsQuery
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();
        }
    }
}
