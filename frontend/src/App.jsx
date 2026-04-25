import { BrowserRouter, Routes, Route, Navigate } from "react-router-dom";
import { useEffect, useState } from "react";
import Home from "./pages/Home";
import Eventos from "./pages/Eventos";
import Evento from "./pages/Evento";
import Login from "./pages/Login";
import Register from "./pages/Register";
import { getStoredLanguage, persistLanguage } from "./i18n";
import {
  getCurrentUser,
  getSession,
  logoutUser,
} from "./services/authService";

const THEME_STORAGE_KEY = "ticketing_theme";

function App() {
  const [darkMode, setDarkMode] = useState(() => {
    return localStorage.getItem(THEME_STORAGE_KEY) === "light" ? false : true;
  });
  const [session, setSession] = useState(() => getSession());
  const [language, setLanguage] = useState(() => getStoredLanguage());

  useEffect(() => {
    async function syncCurrentUser() {
      const currentSession = getSession();
      if (!currentSession?.token) {
        setSession(null);
        return;
      }

      try {
        const currentUser = await getCurrentUser();
        if (!currentUser) {
          setSession(null);
          return;
        }

        setSession({
          ...currentSession,
          ...currentUser,
          token: currentSession.token,
        });
      } catch {
        logoutUser();
        setSession(null);
      }
    }

    void syncCurrentUser();
  }, []);

  useEffect(() => {
    document.documentElement.lang = language;
  }, [language]);

  useEffect(() => {
    localStorage.setItem(THEME_STORAGE_KEY, darkMode ? "dark" : "light");
  }, [darkMode]);

  function handleLogout() {
    logoutUser();
    setSession(null);
  }

  function handleLanguageChange(nextLanguage) {
    persistLanguage(nextLanguage);
    setLanguage(nextLanguage);
  }

  return (
    <BrowserRouter>
      <Routes>
        <Route
          path="/"
          element={
            <Home
              darkMode={darkMode}
              setDarkMode={setDarkMode}
              session={session}
              onLogout={handleLogout}
              language={language}
              onLanguageChange={handleLanguageChange}
            />
          }
        />
        <Route
          path="/events"
          element={
            <Eventos
              darkMode={darkMode}
              setDarkMode={setDarkMode}
              session={session}
              onLogout={handleLogout}
              language={language}
              onLanguageChange={handleLanguageChange}
            />
          }
        />
        <Route
          path="/event/:id"
          element={
            <Evento
              darkMode={darkMode}
              setDarkMode={setDarkMode}
              session={session}
              onLogout={handleLogout}
              language={language}
              onLanguageChange={handleLanguageChange}
            />
          }
        />
        <Route
          path="/login"
          element={
            <Login
              darkMode={darkMode}
              setDarkMode={setDarkMode}
              session={session}
              setSession={setSession}
              language={language}
              onLanguageChange={handleLanguageChange}
            />
          }
        />
        <Route
          path="/register"
          element={
            <Register
              darkMode={darkMode}
              setDarkMode={setDarkMode}
              session={session}
              language={language}
              onLanguageChange={handleLanguageChange}
            />
          }
        />
        <Route path="*" element={<Navigate to="/" />} />
      </Routes>
    </BrowserRouter>
  );
}

export default App;
