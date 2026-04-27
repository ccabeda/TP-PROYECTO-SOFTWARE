function LanguageSelector({
  language,
  onChange,
  englishLabel,
  spanishLabel,
  ariaLabel = "Language selector",
}) {
  return (
    <div className="language-selector" aria-label={ariaLabel}>
      <button
        type="button"
        className={`language-option ${language === "en" ? "active" : ""}`}
        onClick={() => onChange("en")}
        aria-pressed={language === "en"}
      >
        {englishLabel}
      </button>
      <span className="language-separator">|</span>
      <button
        type="button"
        className={`language-option ${language === "es" ? "active" : ""}`}
        onClick={() => onChange("es")}
        aria-pressed={language === "es"}
      >
        {spanishLabel}
      </button>
    </div>
  );
}

export default LanguageSelector;
