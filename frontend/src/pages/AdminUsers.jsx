import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import AppShell from "../components/layout/AppShell";
import useAuthContext from "../context/useAuthContext";
import useLanguageContext from "../context/useLanguageContext";
import useDocumentTitle from "../hooks/useDocumentTitle";
import getErrorMessage from "../lib/getErrorMessage";
import { t } from "../lib/i18n";
import { getAdminUsersPage } from "../services/adminService";

const ADMIN_USERS_PATH = "/admin/users";
const ADMIN_USERS_PAGE_SIZE = 12;

function isAdminSession(session) {
  return session?.role?.trim().toLowerCase() === "admin";
}

function AdminUsers() {
  const navigate = useNavigate();
  const { session } = useAuthContext();
  const { language } = useLanguageContext();
  const [nameSearchTerm, setNameSearchTerm] = useState("");
  const [emailSearchTerm, setEmailSearchTerm] = useState("");
  const [users, setUsers] = useState([]);
  const [page, setPage] = useState(1);
  const [totalCount, setTotalCount] = useState(0);
  const [totalPages, setTotalPages] = useState(0);
  const [error, setError] = useState("");
  const [isLoading, setIsLoading] = useState(true);
  const isAdmin = isAdminSession(session);

  const adminLabel = t(language, "topbar.admin");
  const adminUsersTitle = t(language, "home.adminUsersTitle");
  const adminUsersCopy = t(language, "home.adminUsersCopy");
  const adminForbiddenTitle = t(language, "home.adminForbiddenTitle");
  const adminForbiddenCopy = t(language, "home.adminForbiddenCopy");
  const adminUsersNameSearchPlaceholder = t(language, "home.adminUsersNameSearchPlaceholder");
  const adminUsersEmailSearchPlaceholder = t(language, "home.adminUsersEmailSearchPlaceholder");
  const adminUsersLoading = t(language, "home.adminUsersLoading");
  const adminUsersEmpty = t(language, "home.adminUsersEmpty");
  const adminUsersNameLabel = t(language, "home.adminUsersNameLabel");
  const adminUsersEmailLabel = t(language, "home.adminUsersEmailLabel");
  const adminUsersSummaryLabel = t(language, "home.adminUsersSummaryLabel");
  const adminUsersCountLabel = t(language, "home.adminUsersCountLabel", {
    count: String(totalCount),
  });
  const adminPaginationPrevious = t(language, "home.adminPaginationPrevious");
  const adminPaginationNext = t(language, "home.adminPaginationNext");
  const adminPaginationPage = t(language, "home.adminPaginationPage", {
    page: String(page),
    totalPages: String(totalPages || 1),
  });

  useDocumentTitle(adminUsersTitle);

  useEffect(() => {
    if (!session?.token) {
      navigate("/login", { replace: true, state: { redirectTo: ADMIN_USERS_PATH } });
    }
  }, [navigate, session]);

  useEffect(() => {
    let isMounted = true;

    async function loadUsers() {
      if (!session?.token || !isAdmin) {
        if (isMounted) {
          setIsLoading(false);
        }
        return;
      }

      setIsLoading(true);
      setError("");

      try {
        const usersResult = await getAdminUsersPage(
          {
            name: nameSearchTerm,
            email: emailSearchTerm,
            page,
            pageSize: ADMIN_USERS_PAGE_SIZE,
          },
          session.token,
        );

        if (!isMounted) {
          return;
        }

        setUsers(usersResult.items ?? []);
        setTotalCount(usersResult.totalCount ?? 0);
        setTotalPages(usersResult.totalPages ?? 0);
      } catch (loadError) {
        if (!isMounted) {
          return;
        }

        setError(
          getErrorMessage(loadError, "No se pudo cargar la lista de usuarios."),
        );
      } finally {
        if (isMounted) {
          setIsLoading(false);
        }
      }
    }

    loadUsers();

    return () => {
      isMounted = false;
    };
  }, [emailSearchTerm, isAdmin, nameSearchTerm, page, session]);

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
          <h1>{adminUsersTitle}</h1>
          <p className="header-text admin-header-copy">{adminUsersCopy}</p>
        </div>

        <article className="purchase-card admin-panel-card admin-users-card">
          <div className="admin-users-toolbar">
            <div className="admin-users-toolbar-copy">
              <strong>{adminUsersSummaryLabel}</strong>
              <span>{adminUsersCountLabel}</span>
            </div>
          </div>

          <div className="admin-users-search-grid">
            <input
              className="auth-input admin-users-search"
              type="search"
              value={nameSearchTerm}
              onChange={(event) => {
                setNameSearchTerm(event.target.value);
                setPage(1);
              }}
              placeholder={adminUsersNameSearchPlaceholder}
            />
            <input
              className="auth-input admin-users-search"
              type="search"
              value={emailSearchTerm}
              onChange={(event) => {
                setEmailSearchTerm(event.target.value);
                setPage(1);
              }}
              placeholder={adminUsersEmailSearchPlaceholder}
            />
          </div>

          {isLoading ? (
            <p className="events-feedback">{adminUsersLoading}</p>
          ) : error ? (
            <p className="auth-message auth-message-error">{error}</p>
          ) : users.length === 0 ? (
            <p className="events-feedback">{adminUsersEmpty}</p>
          ) : (
            <div className="admin-users-list">
              {users.map((user) => (
                <article key={user.id} className="admin-user-item">
                  <div className="admin-user-main">
                    <div className="admin-user-avatar" aria-hidden="true">
                      {user.name?.trim()?.charAt(0)?.toUpperCase() ?? "U"}
                    </div>
                    <div className="admin-user-copy">
                      <strong>{user.name}</strong>
                      <p>{user.email}</p>
                    </div>
                  </div>
                  <dl className="admin-user-meta">
                    <div>
                      <dt>{adminUsersNameLabel}</dt>
                      <dd>#{user.id}</dd>
                    </div>
                    <div>
                      <dt>{adminUsersEmailLabel}</dt>
                      <dd>{user.email}</dd>
                    </div>
                  </dl>
                </article>
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

export default AdminUsers;
