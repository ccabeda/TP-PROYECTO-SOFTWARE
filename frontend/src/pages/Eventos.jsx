import Topbar from "../components/topbar";
import { useNavigate } from "react-router-dom";
import { t } from "../i18n";

function Eventos({ darkMode, setDarkMode, session, onLogout, language }) {
  const navigate = useNavigate();
  const eventos = [
    {
      id: 1,
      titulo: "Q'Lokura",
      fecha: "25 abril 2026",
      imagen:
        "https://images.unsplash.com/photo-1493225457124-a3eb161ffa5f?q=80&w=1200&auto=format&fit=crop",
    },
    {
      id: 2,
      titulo: "Festival Electrónico",
      fecha: "10 mayo 2026",
      imagen:
        "https://images.unsplash.com/photo-1507874457470-272b3c8d8ee2?q=80&w=1200&auto=format&fit=crop",
    },
    {
      id: 3,
      titulo: "La Delio Valdez",
      fecha: "18 mayo 2026",
      imagen:
        "https://images.unsplash.com/photo-1501386761578-eac5c94b800a?q=80&w=1200&auto=format&fit=crop",
    },
    {
      id: 4,
      titulo: "Noche Indie",
      fecha: "24 mayo 2026",
      imagen:
        "https://images.unsplash.com/photo-1516280440614-37939bbacd81?q=80&w=1200&auto=format&fit=crop",
    },
  ];

  return (
    <div className={darkMode ? "page dark" : "page"}>
      <Topbar
        darkMode={darkMode}
        setDarkMode={setDarkMode}
        session={session}
        onLogout={onLogout}
        language={language}
      />

      <main className="content">
        <h1 style={{ marginBottom: "20px" }}>{t(language, "home.allEvents")}</h1>

        <div className="events-grid">
          {eventos.map((evento) => (
            <article className="event-container" key={evento.id}>
              <img
                src={evento.imagen}
                alt={evento.titulo}
                onClick={() => navigate(`/event/${evento.id}`)}
              />

              <div className="event-body">
                <h3>{evento.titulo}</h3>
                <p>{evento.fecha}</p>

                <div className="event-actions">
                  <button
                    className="btn link-button"
                    onClick={() => navigate(`/event/${evento.id}`)}
                  >
                    {t(language, "home.moreInfo")}
                  </button>

                  <button
                    className="btn btn-event"
                    onClick={() => navigate(`/event/${evento.id}`)}
                  >
                    {t(language, "home.buy")}
                  </button>
                </div>
              </div>
            </article>
          ))}
        </div>
      </main>
    </div>
  );
}

export default Eventos;
