import { BrowserRouter, Routes, Route, Navigate } from "react-router-dom";
import { useState } from "react";
import Home from "./pages/Home";
import Eventos from "./pages/Eventos";
import Evento from "./pages/Evento";

function App() {
  const [darkMode, setDarkMode] = useState(true);

  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<Home darkMode={darkMode} setDarkMode={setDarkMode}/>} />
        <Route path="/events" element={<Eventos darkMode={darkMode} setDarkMode={setDarkMode}/>} />
        <Route path="/event/:id" element={<Evento darkMode={darkMode} setDarkMode={setDarkMode}/>} />
        <Route path="*" element={<Navigate to="/" />} />
      </Routes>
    </BrowserRouter>  
  );
}

export default App;