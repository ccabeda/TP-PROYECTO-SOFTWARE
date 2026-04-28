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

        public async Task<(List<AuditLog> Items, int TotalCount)> GetAll(GetAuditLogsQuery query)
        {
            var auditLogsQuery = _context.AuditLogs
                .AsNoTracking()
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

            if (!string.IsNullOrWhiteSpace(query.Search))
            {
                var normalizedSearch = query.Search.Trim();
                auditLogsQuery = auditLogsQuery.Where(a =>
                    (a.User != null && a.User.Email != null && a.User.Email.Contains(normalizedSearch)) ||
                    (a.Action != null && a.Action.Contains(normalizedSearch)) ||
                    (a.EntityType != null && a.EntityType.Contains(normalizedSearch)) ||
                    (a.Details != null && a.Details.Contains(normalizedSearch)));
            }

            var totalCount = await auditLogsQuery.CountAsync();
            var items = await auditLogsQuery
                .OrderByDescending(a => a.CreatedAt)
                .Skip((query.Page - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync();

            return (items, totalCount);
        }
    }
}
