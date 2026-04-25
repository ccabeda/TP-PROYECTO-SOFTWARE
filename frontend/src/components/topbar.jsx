import { Link } from "react-router-dom";
import { useNavigate } from "react-router-dom";
import { t } from "../i18n";
import ThemeToggle from "./ThemeToggle";

function Topbar({ darkMode, setDarkMode, session, onLogout, language }) {
  const navigate = useNavigate();

  return (
    <header className="topbar">
      <div className="logo">
        <Link to="/">Nombre_Empresa</Link>
      </div>

      <nav className="nav">
        <Link to="/">{t(language, "topbar.home")}</Link>
        <Link to="/events">{t(language, "topbar.events")}</Link>
      </nav>

      <div className="topbar-actions">
        <ThemeToggle
          darkMode={darkMode}
          onToggle={() => setDarkMode(!darkMode)}
          ariaLabel={t(
            language,
            darkMode ? "topbar.themeToLight" : "topbar.themeToDark",
          )}
        />

        {session?.token ? (
          <>
            <span className="session-chip">
              {t(language, "topbar.greeting", {
                name: session.name ?? "usuario",
              })}
            </span>
            <button className="btn btn-login" onClick={onLogout}>
              {t(language, "topbar.logout")}
            </button>
          </>
        ) : (
          <>
            <button className="btn btn-login" onClick={() => navigate("/login")}>
              {t(language, "topbar.login")}
            </button>

            <button
              className="btn btn-primary"
              onClick={() => navigate("/register")}
            >
              {t(language, "topbar.register")}
            </button>
          </>
        )}
      </div>
    </header>
  );
}

export default Topbar;
