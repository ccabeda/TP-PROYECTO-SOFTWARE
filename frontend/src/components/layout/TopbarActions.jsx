import { useState } from "react";
import useLanguageContext from "../../context/useLanguageContext";
import { t } from "../../lib/i18n";
import ThemeToggle from "../ui/ThemeToggle";

function TopbarActions({
  darkMode,
  setDarkMode,
  session,
  onAdmin,
  onAdminUsers,
  onAdminAuditLogs,
  onLogout,
  onLogin,
  onMyTickets,
  onRegister,
}) {
  const [isLoggingOut, setIsLoggingOut] = useState(false);
  const [isLanguageMenuOpen, setIsLanguageMenuOpen] = useState(false);
  const [isAdminMenuOpen, setIsAdminMenuOpen] = useState(false);
  const { language, setLanguage } = useLanguageContext();
  const themeAriaLabel = t(
    language,
    darkMode ? "topbar.themeToLight" : "topbar.themeToDark",
  );
  const toggleLanguageLabel = t(language, "topbar.toggleLanguage");
  const currentLanguageLabel = language === "es" ? "ES" : "EN";
  const greetingLabel = t(language, "topbar.greeting", {
    name: session?.name ?? "usuario",
  });
  const loginLabel = t(language, "topbar.login");
  const adminLabel = t(language, "topbar.admin");
  const adminCreateEventLabel = t(language, "topbar.adminCreateEvent");
  const adminViewUsersLabel = t(language, "topbar.adminViewUsers");
  const adminViewAuditLogsLabel = t(language, "topbar.adminViewAuditLogs");
  const registerLabel = t(language, "topbar.register");
  const myTicketsLabel = t(language, "topbar.myTickets");
  const logoutLabel = t(language, "topbar.logout");
  const isAdmin = session?.role?.trim().toLowerCase() === "admin";

  function handleLogoutClick() {
    setIsLoggingOut(true);
    onLogout();
    window.setTimeout(() => {
      window.location.assign("/");
    }, 50);
  }

  function handleLanguageSelect(nextLanguage) {
    setIsLanguageMenuOpen(false);
    setLanguage(nextLanguage);
  }

  function handleAdminSelect(action) {
    setIsAdminMenuOpen(false);
    action();
  }

  return (
    <div className="topbar-actions">
      <ThemeToggle
        darkMode={darkMode}
        onToggle={() => setDarkMode((current) => !current)}
        ariaLabel={themeAriaLabel}
      />

      <div className={`language-menu ${isLanguageMenuOpen ? "is-open" : ""}`}>
        <button
          type="button"
          className="language-toggle"
          onClick={() => setIsLanguageMenuOpen((current) => !current)}
          aria-label={toggleLanguageLabel}
          aria-expanded={isLanguageMenuOpen}
        >
          <span aria-hidden="true">🌐</span>
          <span className="language-toggle-text">{currentLanguageLabel}</span>
        </button>

        {isLanguageMenuOpen ? (
          <div className="language-menu-dropdown">
            <button
              type="button"
              className={`language-menu-option ${language === "es" ? "active" : ""}`}
              onClick={() => handleLanguageSelect("es")}
            >
              Español
            </button>
            <button
              type="button"
              className={`language-menu-option ${language === "en" ? "active" : ""}`}
              onClick={() => handleLanguageSelect("en")}
            >
              English
            </button>
          </div>
        ) : null}
      </div>

      {session?.token ? (
        <>
          {isAdmin ? (
            <div className={`admin-menu ${isAdminMenuOpen ? "is-open" : ""}`}>
              <button
                type="button"
                className="admin-toggle"
                onClick={() => setIsAdminMenuOpen((current) => !current)}
                aria-label={adminLabel}
                aria-expanded={isAdminMenuOpen}
              >
                <span className="admin-toggle-dot" aria-hidden="true" />
                <span className="admin-toggle-text">{adminLabel}</span>
                <span className="admin-toggle-caret" aria-hidden="true">▾</span>
              </button>

              {isAdminMenuOpen ? (
                <div className="admin-menu-dropdown">
                  <button
                    type="button"
                    className="admin-menu-option"
                    onClick={() => handleAdminSelect(onAdmin)}
                  >
                    {adminCreateEventLabel}
                  </button>
                  <button
                    type="button"
                    className="admin-menu-option"
                    onClick={() => handleAdminSelect(onAdminUsers)}
                  >
                    {adminViewUsersLabel}
                  </button>
                  <button
                    type="button"
                    className="admin-menu-option"
                    onClick={() => handleAdminSelect(onAdminAuditLogs)}
                  >
                    {adminViewAuditLogsLabel}
                  </button>
                </div>
              ) : null}
            </div>
          ) : null}
          <span className="session-chip">{greetingLabel}</span>
          <button className="btn btn-login" onClick={onMyTickets}>
            {myTicketsLabel}
          </button>
          <button
            className="btn btn-login topbar-logout-button"
            onClick={handleLogoutClick}
            disabled={isLoggingOut}
          >
            <span
              className={`topbar-button-spinner ${isLoggingOut ? "is-visible" : ""}`}
              aria-hidden="true"
            />
            <span>{logoutLabel}</span>
          </button>
        </>
      ) : (
        <>
          <button className="btn btn-login" onClick={onLogin}>
            {loginLabel}
          </button>

          <button className="btn btn-primary" onClick={onRegister}>
            {registerLabel}
          </button>
        </>
      )}
    </div>
  );
}

export default TopbarActions;
