import { createContext } from "react";
import useLanguage from "../hooks/useLanguage";

const LanguageContext = createContext(null);
LanguageContext.displayName = "LanguageContext";

export function LanguageProvider({ children }) {
  const value = useLanguage();

  return <LanguageContext.Provider value={value}>{children}</LanguageContext.Provider>;
}

export { LanguageContext };
