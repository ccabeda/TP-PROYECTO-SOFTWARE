import { createContext, useContext } from "react";
import useLanguage from "../hooks/useLanguage";

const LanguageContext = createContext(null);
LanguageContext.displayName = "LanguageContext";

export function LanguageProvider({ children }) {
  const value = useLanguage();

  return <LanguageContext.Provider value={value}>{children}</LanguageContext.Provider>;
}

export function useLanguageContext() {
  const context = useContext(LanguageContext);

  if (!context) {
    throw new Error("useLanguageContext must be used within a LanguageProvider");
  }

  return context;
}
