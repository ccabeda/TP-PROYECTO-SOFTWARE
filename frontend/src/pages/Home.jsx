import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { Link } from "react-router-dom";
import Topbar from "../components/topbar";

function Home({ darkMode, setDarkMode }) {
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
      <Topbar darkMode={darkMode} setDarkMode={setDarkMode} />

      <main className="content">
        <section className="header">
          <div>
            <p className="header-badge">Eventos en vivo</p>
            <h1>Viví tus eventos favoritos</h1>
            <p className="header-text">
              Descubrí conciertos, festivales y experiencias únicas. Explorá la
              cartelera y conseguí tus entradas.
            </p>
          </div>
        </section>

        <section className="events-section" id="events">
          <div className="section-header">
            <h2>Próximos Eventos</h2>
            <button className="btn btn-secondary"
              onClick={() => navigate("/eventos")}
            >
              Ver todos</button>
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
                      Más info
                    </button>

                    <button
                      className="btn btn-event"
                      onClick={() => navigate(`/event/${evento.id}`)}
                    >
                      Comprar
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