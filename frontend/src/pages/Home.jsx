import { useNavigate } from "react-router-dom";
import Topbar from "../components/topbar";
import { t } from "../i18n";

function Home({
  darkMode,
  setDarkMode,
  session,
  onLogout,
  language,
}) {
  const navigate = useNavigate();

  const eventos = [
    {
      id: 2,
      titulo: "Q'Lokura",
      fecha: "25 abril 2026 y 1 fecha más",
      imagen:
        "https://images.unsplash.com/photo-1493225457124-a3eb161ffa5f?q=80&w=1200&auto=format&fit=crop",
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
        <section className="header">
          <div>
            <p className="header-badge">{t(language, "home.badge")}</p>
            <h1>{t(language, "home.title")}</h1>
            <p className="header-text">
              {t(language, "home.copy")}
            </p>
          </div>
        </section>

        <section className="events-section" id="events">
          <div className="section-header">
            <h2>{t(language, "home.upcoming")}</h2>
            <button
              className="btn btn-secondary"
              onClick={() => navigate("/events")}
            >
              {t(language, "home.viewAll")}
            </button>
          </div>

          <div className="events-grid">
            {eventos.map((evento) => (
              <article className="event-container" key={evento.id}>
                <img src={evento.imagen} alt={evento.titulo}
                  onClick={() => navigate(`/event/${evento.id}`)} />

                <div className="event-body">
                  <h3>{evento.titulo}</h3>
                  <p>{evento.fecha}</p>

                  <div className="event-actions">
                    <button className="btn link-button"
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
            {/* <button>
            
            </button> 
            Botón para volver al inicio*/}
          </div>
        </section>
      </main>
    </div>
  );
}

export default Home;
