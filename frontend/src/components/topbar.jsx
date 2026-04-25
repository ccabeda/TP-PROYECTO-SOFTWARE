import { Link } from "react-router-dom";
import { useNavigate } from "react-router-dom";

function Topbar({ darkMode, setDarkMode }) {
  const navigate = useNavigate();
  return (
    <header className="topbar">
      <div className="logo">
        <Link to="/">Nombre_Empresa</Link>
      </div>

      <nav className="nav">
        <Link to="/">Inicio</Link>
        <Link to="/events">Eventos</Link>
      </nav>

      <div className="topbar-actions">
        <button
          className="theme-button"
          onClick={() => setDarkMode(!darkMode)}
        >
          {darkMode ? "☀" : "🌙"}
        </button>

        <button
          className="btn btn-login"
          // onClick={() => navigate("/login")}
        >
          Iniciar sesión
        </button>

        <button className="btn btn-primary"
        // onClick={() => navigate("/register")}
        >
          Crear cuenta
        </button>
      </div>
    </header>
  );
}

export default Topbar;