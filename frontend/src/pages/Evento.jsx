import Topbar from "../components/topbar";
import { useParams } from "react-router-dom";
import { t } from "../i18n";

function Evento({ darkMode, setDarkMode, session, onLogout, language }) {
    const { id } = useParams();
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
    }
    ];

    const evento = eventos.find((item) => item.id === Number(id)) ?? eventos[0];
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
                <section className="event-detail">
                  <img
                    className="event-detail-image"
                    src={evento.imagen}
                    alt={evento.titulo}
                  />
                  <div className="event-detail-body">
                    <p className="header-badge">{t(language, "home.detailBadge")}</p>
                    <h1>{evento.titulo}</h1>
                    <p className="header-text">{evento.fecha}</p>
                    <p className="event-detail-copy">
                      {t(language, "home.detailCopy")}
                    </p>
                  </div>
                </section>
            </main>
            
        </div>
    );
}

export default Evento;
