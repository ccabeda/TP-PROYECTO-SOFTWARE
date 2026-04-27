import { useState } from "react";
import { useLanguageContext } from "../../context/LanguageContext";
import { t } from "../../lib/i18n";
import ThemeToggle from "../ui/ThemeToggle";

function TopbarActions({
  darkMode,
  setDarkMode,
  session,
  onLogout,
  onLogin,
  onMyTickets,
  onRegister,
}) {
  const [isLoggingOut, setIsLoggingOut] = useState(false);
  const [isLanguageMenuOpen, setIsLanguageMenuOpen] = useState(false);
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
  const registerLabel = t(language, "topbar.register");
  const myTicketsLabel = t(language, "topbar.myTickets");
  const logoutLabel = t(language, "topbar.logout");

  function handleLogoutClick() {
    setIsLoggingOut(true);
    onLogout();
    window.setTimeout(() => {
      window.location.assign("/");
    }, 250);
  }

  function handleLanguageSelect(nextLanguage) {
    setIsLanguageMenuOpen(false);
    setLanguage(nextLanguage);
    window.location.reload();
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
