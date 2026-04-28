using AutoMapper;
using TP_PROYECTO_SOFTWARE.Aplication.DTOs;
using TP_PROYECTO_SOFTWARE.Aplication.DTOs.AuditLogDTOs;
using TP_PROYECTO_SOFTWARE.Aplication.IHandlers;
using TP_PROYECTO_SOFTWARE.Aplication.IRepository.IQuery;
using TP_PROYECTO_SOFTWARE.Aplication.UseCases.AuditLogs.Queries;

namespace TP_PROYECTO_SOFTWARE.Aplication.UseCases.AuditLogs.Handlers
{
    public class GetAuditLogsHandler : IGetAuditLogsHandler
    {
        private readonly IRepositoryAuditLogQuery _repositoryAuditLogQuery;
        private readonly IMapper _mapper;

        public GetAuditLogsHandler(IRepositoryAuditLogQuery repositoryAuditLogQuery, IMapper mapper)
        {
            _repositoryAuditLogQuery = repositoryAuditLogQuery;
            _mapper = mapper;
        }

        public async Task<PagedResultDTO<AuditLogGetDTO>> Handle(GetAuditLogsQuery query)
        {
            if (query.Date.HasValue && (query.DateFrom.HasValue || query.DateTo.HasValue))
            {
                throw new InvalidOperationException("No se puede usar 'date' junto con 'dateFrom' o 'dateTo'.");
            }

            if (query.DateFrom.HasValue && query.DateTo.HasValue && query.DateFrom.Value.Date > query.DateTo.Value.Date)
            {
                throw new InvalidOperationException("dateFrom no puede ser mayor a dateTo.");
            }

            var normalizedPage = Math.Max(query.Page, 1);
            var normalizedPageSize = Math.Clamp(query.PageSize, 1, 50);
            var normalizedQuery = query with
            {
                Page = normalizedPage,
                PageSize = normalizedPageSize
            };

            var (auditLogs, totalCount) = await _repositoryAuditLogQuery.GetAll(normalizedQuery);
            var items = _mapper.Map<List<AuditLogGetDTO>>(auditLogs);

            return new PagedResultDTO<AuditLogGetDTO>
            {
                Items = items,
                TotalCount = totalCount,
                Page = normalizedPage,
                PageSize = normalizedPageSize,
                TotalPages = (int)Math.Ceiling(totalCount / (double)normalizedPageSize)
            };
        }
    }
}
