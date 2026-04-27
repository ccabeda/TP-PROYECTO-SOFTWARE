import { useEffect, useState } from "react";

const THEME_STORAGE_KEY = "ticketing_theme";

function getStoredTheme() {
  if (typeof window === "undefined") {
    return true;
  }

  return window.localStorage.getItem(THEME_STORAGE_KEY) !== "light";
}

function persistTheme(darkMode) {
  if (typeof window === "undefined") {
    return;
  }

  window.localStorage.setItem(THEME_STORAGE_KEY, darkMode ? "dark" : "light");
}

function useTheme() {
  const [darkMode, setDarkMode] = useState(() => getStoredTheme());

  useEffect(() => {
    persistTheme(darkMode);
  }, [darkMode]);

  return { darkMode, setDarkMode };
}

export default useTheme;
