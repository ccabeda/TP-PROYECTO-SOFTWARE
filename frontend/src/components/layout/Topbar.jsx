import { Link, useLocation, useNavigate } from "react-router-dom";
import useAuthContext from "../../context/useAuthContext";
import useLanguageContext from "../../context/useLanguageContext";
import useThemeContext from "../../context/useThemeContext";
import { t } from "../../lib/i18n";
import TopbarActions from "./TopbarActions";

function Topbar() {
  const navigate = useNavigate();
  const location = useLocation();
  const { darkMode, setDarkMode } = useThemeContext();
  const { language } = useLanguageContext();
  const { session, clearSession } = useAuthContext();

  function handleBrandClick(event) {
    if (location.pathname === "/") {
      event.preventDefault();
    }

    window.scrollTo({ top: 0, behavior: "smooth" });

    if (location.pathname !== "/") {
      navigate("/");
    }
  }

  return (
    <header className="topbar">
      <div className="logo">
        <Link to="/" onClick={handleBrandClick}>
          TicketUnaj
        </Link>
      </div>

      <nav className="nav">
        <Link to="/">{t(language, "topbar.home")}</Link>
        <Link to="/events">{t(language, "topbar.events")}</Link>
      </nav>

      <TopbarActions
        darkMode={darkMode}
        setDarkMode={setDarkMode}
        session={session}
        onLogout={clearSession}
        language={language}
        onAdmin={() => navigate("/admin")}
        onAdminUsers={() => navigate("/admin/users")}
        onAdminAuditLogs={() => navigate("/admin/audit-logs")}
        onLogin={() => navigate("/login")}
        onMyTickets={() => navigate("/my-tickets")}
        onRegister={() => navigate("/register")}
      />
    </header>
  );
}

export default Topbar;
