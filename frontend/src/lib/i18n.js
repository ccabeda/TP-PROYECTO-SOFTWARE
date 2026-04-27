import { authTranslations } from "./translations/auth";
import { homeTranslations } from "./translations/home";
import { topbarTranslations } from "./translations/topbar";

export const LANGUAGE_STORAGE_KEY = "ticketing_language";
export const SUPPORTED_LANGUAGES = ["es", "en"];

function isSupportedLanguage(language) {
  return SUPPORTED_LANGUAGES.includes(language);
}

export const translations = {
  es: {
    topbar: topbarTranslations.es,
    auth: authTranslations.es,
    home: homeTranslations.es,
  },
  en: {
    topbar: topbarTranslations.en,
    auth: authTranslations.en,
    home: homeTranslations.en,
  },
};

export function getStoredLanguage() {
  if (typeof window === "undefined") {
    return "es";
  }

  const saved = window.localStorage.getItem(LANGUAGE_STORAGE_KEY);
  return isSupportedLanguage(saved) ? saved : "es";
}

export function persistLanguage(language) {
  if (typeof window === "undefined" || !isSupportedLanguage(language)) {
    return;
  }

  window.localStorage.setItem(LANGUAGE_STORAGE_KEY, language);
}

export function t(language, key, replacements = {}) {
  const normalizedLanguage = isSupportedLanguage(language) ? language : "es";
  const dictionary = translations[normalizedLanguage];
  const value = key.split(".").reduce((current, part) => current?.[part], dictionary);
  if (!value) {
    return key;
  }

  return Object.entries(replacements).reduce(
    (current, [replacementKey, replacementValue]) =>
      current.replace(`{${replacementKey}}`, replacementValue),
    value,
  );
}
