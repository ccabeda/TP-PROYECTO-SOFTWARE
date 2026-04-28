import { createContext } from "react";
import useTheme from "../hooks/useTheme";

const ThemeContext = createContext(null);
ThemeContext.displayName = "ThemeContext";

export function ThemeProvider({ children }) {
  const value = useTheme();

  return <ThemeContext.Provider value={value}>{children}</ThemeContext.Provider>;
}

export { ThemeContext };
