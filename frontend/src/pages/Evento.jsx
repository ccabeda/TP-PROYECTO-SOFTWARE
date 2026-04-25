import Topbar from "../components/topbar";
import { Link } from "react-router-dom";
import { useNavigate } from "react-router-dom";

function Evento({ darkMode, setDarkMode }) {
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
    return (
        <div className={darkMode ? "page dark" : "page"}>
            <Topbar darkMode={darkMode} setDarkMode={setDarkMode} />    

            <main className="content">
                <h1>Detalle del Evento</h1>
                <p>Info detallada del eventos</p>
            </main>
            
        </div>
    );
}

export default Evento;