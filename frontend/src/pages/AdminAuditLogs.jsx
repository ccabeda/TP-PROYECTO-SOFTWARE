import { useEffect, useMemo, useState } from "react";
import DatePicker from "react-datepicker";
import { format } from "date-fns";
import { enUS, es } from "date-fns/locale";
import { useNavigate } from "react-router-dom";
import AppShell from "../components/layout/AppShell";
import useAuthContext from "../context/useAuthContext";
import useLanguageContext from "../context/useLanguageContext";
import useDocumentTitle from "../hooks/useDocumentTitle";
import getErrorMessage from "../lib/getErrorMessage";
import { t } from "../lib/i18n";
import { getAdminAuditLogs } from "../services/adminService";

const ADMIN_AUDIT_LOGS_PATH = "/admin/audit-logs";
const ADMIN_AUDIT_LOGS_PAGE_SIZE = 12;

function isAdminSession(session) {
  return session?.role?.trim().toLowerCase() === "admin";
}

function getAuditLocale(language) {
  return language === "es" ? "es-AR" : "en-US";
}

function formatAuditDate(dateValue, language) {
  return new Intl.DateTimeFormat(getAuditLocale(language), {
    dateStyle: "medium",
    timeStyle: "short",
  }).format(new Date(dateValue));
}

function getAuditFieldLabel(field, language) {
  const labels = {
    es: {
      Name: "Nombre",
      Venue: "Estadio",
      EventDate: "Fecha",
      Status: "Estado",
      ImageUrl: "Imagen",
      DescriptionLength: "Descripción",
      EventId: "Evento",
      SectorId: "Sector",
      RowCount: "Filas",
      GeneratedRows: "Filas generadas",
      SeatsPerRow: "Asientos por fila",
      TotalCreated: "Butacas creadas",
      Price: "Precio",
      Capacity: "Capacidad",
    },
    en: {
      Name: "Name",
      Venue: "Venue",
      EventDate: "Date",
      Status: "Status",
      ImageUrl: "Image",
      DescriptionLength: "Description",
      EventId: "Event",
      SectorId: "Sector",
      RowCount: "Rows",
      GeneratedRows: "Generated rows",
      SeatsPerRow: "Seats per row",
      TotalCreated: "Seats created",
      Price: "Price",
      Capacity: "Capacity",
    },
  };

  return labels[language]?.[field] ?? field;
}

function formatAuditFieldValue(field, value, language) {
  if (!value) {
    return language === "es" ? "Sin datos" : "No data";
  }

  if (field === "EventDate") {
    return formatAuditDate(value, language);
  }

  if (field === "DescriptionLength") {
    return language === "es" ? `${value} caracteres` : `${value} characters`;
  }

  return value;
}

function parseAuditDetails(details, language) {
  const separatorIndex = details.indexOf(". ");
  const summary =
    separatorIndex >= 0 ? details.slice(0, separatorIndex + 1) : details;
  const rawFields =
    separatorIndex >= 0 ? details.slice(separatorIndex + 2).trim() : "";

  if (!rawFields || !rawFields.includes("=")) {
    return { summary, fields: [] };
  }

  const fields = rawFields
    .split(", ")
    .map((part) => {
      const [rawKey, ...rawValueParts] = part.split("=");
      const key = rawKey?.trim();
      const value = rawValueParts.join("=").trim();

      if (!key) {
        return null;
      }

      return {
        key,
        label: getAuditFieldLabel(key, language),
        value: formatAuditFieldValue(key, value, language),
      };
    })
    .filter(Boolean);

  return { summary, fields };
}

function AdminAuditLogs() {
  const navigate = useNavigate();
  const { session } = useAuthContext();
  const { language } = useLanguageContext();
  const locale = language === "es" ? es : enUS;
  const [searchTerm, setSearchTerm] = useState("");
  const [dateFrom, setDateFrom] = useState("");
  const [dateTo, setDateTo] = useState("");
  const [selectedDateFrom, setSelectedDateFrom] = useState(null);
  const [selectedDateTo, setSelectedDateTo] = useState(null);
  const [logs, setLogs] = useState([]);
  const [page, setPage] = useState(1);
  const [totalCount, setTotalCount] = useState(0);
  const [totalPages, setTotalPages] = useState(0);
  const [error, setError] = useState("");
  const [isLoading, setIsLoading] = useState(true);
  const isAdmin = isAdminSession(session);

  const adminLabel = t(language, "topbar.admin");
  const adminAuditLogsTitle = t(language, "home.adminAuditLogsTitle");
  const adminAuditLogsCopy = t(language, "home.adminAuditLogsCopy");
  const adminForbiddenTitle = t(language, "home.adminForbiddenTitle");
  const adminForbiddenCopy = t(language, "home.adminForbiddenCopy");
  const adminAuditLogsSearchPlaceholder = t(language, "home.adminAuditLogsSearchPlaceholder");
  const adminAuditLogsDateFromLabel = t(language, "home.adminAuditLogsDateFromLabel");
  const adminAuditLogsDateToLabel = t(language, "home.adminAuditLogsDateToLabel");
  const adminAuditLogsLoading = t(language, "home.adminAuditLogsLoading");
  const adminAuditLogsEmpty = t(language, "home.adminAuditLogsEmpty");
  const adminAuditLogsSummaryLabel = t(language, "home.adminAuditLogsSummaryLabel");
  const adminAuditLogsCountLabel = t(language, "home.adminAuditLogsCountLabel", {
    count: String(totalCount),
  });
  const adminAuditLogsActionLabel = t(language, "home.adminAuditLogsActionLabel");
  const adminAuditLogsUserLabel = t(language, "home.adminAuditLogsUserLabel");
  const adminAuditLogsEntityLabel = t(language, "home.adminAuditLogsEntityLabel");
  const adminAuditLogsDateLabel = t(language, "home.adminAuditLogsDateLabel");
  const adminPaginationPrevious = t(language, "home.adminPaginationPrevious");
  const adminPaginationNext = t(language, "home.adminPaginationNext");
  const adminPaginationPage = t(language, "home.adminPaginationPage", {
    page: String(page),
    totalPages: String(totalPages || 1),
  });

  useDocumentTitle(adminAuditLogsTitle);

  function handleDateFromChange(date) {
    setSelectedDateFrom(date);
    setDateFrom(date ? format(date, "yyyy-MM-dd") : "");
    setPage(1);
  }

  function handleDateToChange(date) {
    setSelectedDateTo(date);
    setDateTo(date ? format(date, "yyyy-MM-dd") : "");
    setPage(1);
  }

  useEffect(() => {
    if (!session?.token) {
      navigate("/login", {
        replace: true,
        state: { redirectTo: ADMIN_AUDIT_LOGS_PATH },
      });
    }
  }, [navigate, session]);

  useEffect(() => {
    let isMounted = true;

    async function loadAuditLogs() {
      if (!session?.token || !isAdmin) {
        if (isMounted) {
          setIsLoading(false);
        }
        return;
      }

      setIsLoading(true);
      setError("");

      try {
        const auditLogs = await getAdminAuditLogs(
          {
            search: searchTerm,
            dateFrom,
            dateTo,
            page,
            pageSize: ADMIN_AUDIT_LOGS_PAGE_SIZE,
          },
          session.token,
        );

        if (!isMounted) {
          return;
        }

        setLogs(auditLogs.items ?? []);
        setTotalCount(auditLogs.totalCount ?? 0);
        setTotalPages(auditLogs.totalPages ?? 0);
      } catch (loadError) {
        if (!isMounted) {
          return;
        }

        setError(
          getErrorMessage(loadError, "No se pudo cargar la auditoría."),
        );
      } finally {
        if (isMounted) {
          setIsLoading(false);
        }
      }
    }

    loadAuditLogs();

    return () => {
      isMounted = false;
    };
  }, [dateFrom, dateTo, isAdmin, page, searchTerm, session]);

  if (!session?.token) {
    return null;
  }

  if (!isAdmin) {
    return (
      <AppShell>
        <section className="admin-page">
          <article className="purchase-card admin-panel-card">
            <h1>{adminForbiddenTitle}</h1>
            <p className="event-detail-copy">{adminForbiddenCopy}</p>
          </article>
        </section>
      </AppShell>
    );
  }

  return (
    <AppShell>
      <section className="admin-page">
        <div className="purchase-header admin-header">
          <div className="purchase-header-badge-row">
            <p className="header-badge purchase-header-badge">{adminLabel}</p>
          </div>
          <h1>{adminAuditLogsTitle}</h1>
          <p className="header-text admin-header-copy">{adminAuditLogsCopy}</p>
        </div>

        <article className="purchase-card admin-panel-card admin-audit-card">
          <div className="admin-users-toolbar">
            <div className="admin-users-toolbar-copy">
              <strong>{adminAuditLogsSummaryLabel}</strong>
              <span>{adminAuditLogsCountLabel}</span>
            </div>
          </div>

          <div className="admin-audit-filters">
            <input
              className="auth-input admin-users-search"
              type="search"
              value={searchTerm}
              onChange={(event) => {
                setSearchTerm(event.target.value);
                setPage(1);
              }}
              placeholder={adminAuditLogsSearchPlaceholder}
            />

            <div className="admin-audit-date-field">
              <label className="auth-label" htmlFor="admin-audit-date-from">
                {adminAuditLogsDateFromLabel}
              </label>
              <DatePicker
                id="admin-audit-date-from"
                selected={selectedDateFrom}
                onChange={handleDateFromChange}
                locale={locale}
                dateFormat="dd/MM/yyyy"
                placeholderText={adminAuditLogsDateFromLabel}
                className="auth-input events-search events-date-filter admin-date-filter"
                showPopperArrow={false}
                isClearable
                popperPlacement="bottom-start"
              />
            </div>

            <div className="admin-audit-date-field">
              <label className="auth-label" htmlFor="admin-audit-date-to">
                {adminAuditLogsDateToLabel}
              </label>
              <DatePicker
                id="admin-audit-date-to"
                selected={selectedDateTo}
                onChange={handleDateToChange}
                locale={locale}
                dateFormat="dd/MM/yyyy"
                placeholderText={adminAuditLogsDateToLabel}
                className="auth-input events-search events-date-filter admin-date-filter"
                showPopperArrow={false}
                isClearable
                minDate={selectedDateFrom ?? undefined}
                popperPlacement="bottom-start"
              />
            </div>
          </div>

          {isLoading ? (
            <p className="events-feedback">{adminAuditLogsLoading}</p>
          ) : error ? (
            <p className="auth-message auth-message-error">{error}</p>
          ) : logs.length === 0 ? (
            <p className="events-feedback">{adminAuditLogsEmpty}</p>
          ) : (
            <div className="admin-audit-list">
              {logs.map((log) => (
                <AuditLogCard
                  key={log.id}
                  log={log}
                  language={language}
                  adminAuditLogsActionLabel={adminAuditLogsActionLabel}
                  adminAuditLogsUserLabel={adminAuditLogsUserLabel}
                  adminAuditLogsEntityLabel={adminAuditLogsEntityLabel}
                  adminAuditLogsDateLabel={adminAuditLogsDateLabel}
                />
              ))}
            </div>
          )}

          {totalPages > 1 ? (
            <div className="admin-pagination">
              <button
                type="button"
                className="button button-secondary admin-pagination-button"
                onClick={() => setPage((current) => Math.max(current - 1, 1))}
                disabled={page <= 1 || isLoading}
              >
                {adminPaginationPrevious}
              </button>
              <span className="admin-pagination-label">{adminPaginationPage}</span>
              <button
                type="button"
                className="button button-secondary admin-pagination-button"
                onClick={() =>
                  setPage((current) => Math.min(current + 1, totalPages))
                }
                disabled={page >= totalPages || isLoading}
              >
                {adminPaginationNext}
              </button>
            </div>
          ) : null}
        </article>
      </section>
    </AppShell>
  );
}

function AuditLogCard({
  log,
  language,
  adminAuditLogsActionLabel,
  adminAuditLogsUserLabel,
  adminAuditLogsEntityLabel,
  adminAuditLogsDateLabel,
}) {
  const parsedDetails = useMemo(
    () => parseAuditDetails(log.details, language),
    [language, log.details],
  );

  return (
    <article className="admin-audit-item">
      <div className="admin-audit-main">
        <div className="admin-audit-badge">{log.action}</div>
        <div className="admin-audit-copy">
          <strong>{log.entityType}</strong>
          <p>{parsedDetails.summary}</p>
        </div>

        {parsedDetails.fields.length > 0 ? (
          <div className="admin-audit-fields">
            {parsedDetails.fields.map((field) => (
              <div key={`${log.id}-${field.key}`} className="admin-audit-field">
                <span>{field.label}</span>
                <strong>{field.value}</strong>
              </div>
            ))}
          </div>
        ) : null}
      </div>

      <dl className="admin-audit-meta">
        <div>
          <dt>{adminAuditLogsUserLabel}</dt>
          <dd>{log.userEmail || "-"}</dd>
        </div>
        <div>
          <dt>{adminAuditLogsActionLabel}</dt>
          <dd>{log.action}</dd>
        </div>
        <div>
          <dt>{adminAuditLogsEntityLabel}</dt>
          <dd>
            {log.entityType} #{log.entityId}
          </dd>
        </div>
        <div>
          <dt>{adminAuditLogsDateLabel}</dt>
          <dd>{formatAuditDate(log.createdAt, language)}</dd>
        </div>
      </dl>
    </article>
  );
}

export default AdminAuditLogs;
