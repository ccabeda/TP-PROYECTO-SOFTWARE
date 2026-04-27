import { useEffect, useState } from "react";
import { getStoredLanguage, persistLanguage } from "../lib/i18n";

const SUPPORTED_LANGUAGES = new Set(["es", "en"]);

function useLanguage() {
  const [language, setLanguage] = useState(() => getStoredLanguage());

  useEffect(() => {
    document.documentElement.lang = language;
  }, [language]);

  function updateLanguage(nextLanguage) {
    if (!SUPPORTED_LANGUAGES.has(nextLanguage)) {
      return;
    }

    persistLanguage(nextLanguage);
    setLanguage(nextLanguage);
  }

  return { language, setLanguage: updateLanguage };
}

export default useLanguage;
