const LOCALES = {
  es: "es-AR",
  en: "en-US",
};

function getLocale(language) {
  return LOCALES[language] ?? LOCALES.es;
}

export function formatEventDate(eventDate, language) {
  if (!eventDate) {
    return "";
  }

  const parsedDate = new Date(eventDate);
  if (Number.isNaN(parsedDate.getTime())) {
    return "";
  }

  return new Intl.DateTimeFormat(getLocale(language), {
    day: "numeric",
    month: "long",
    year: "numeric",
  }).format(parsedDate);
}

export function formatEventStatus(status, language, options = {}) {
  if (!status) {
    return "-";
  }

  const normalizedStatus = status.trim().toLowerCase();
  const hasSectors = options.hasSectors ?? true;

  const labels = {
    scheduled: {
      es: "Programado",
      en: "Scheduled",
    },
    soldout: {
      es: "Agotado",
      en: "Sold out",
    },
  };

  if (normalizedStatus === "scheduled" && !hasSectors) {
    return language === "es" ? "Próximamente" : "Coming soon";
  }

  return labels[normalizedStatus]?.[language] ?? status.trim();
}
