function LanguageSelector({ language, onChange, englishLabel, spanishLabel }) {
  return (
    <div className="language-selector" aria-label="Language selector">
      <button
        type="button"
        className={`language-option ${language === "en" ? "active" : ""}`}
        onClick={() => onChange("en")}
      >
        {englishLabel}
      </button>
      <span className="language-separator">|</span>
      <button
        type="button"
        className={`language-option ${language === "es" ? "active" : ""}`}
        onClick={() => onChange("es")}
      >
        {spanishLabel}
      </button>
    </div>
  );
}

export default LanguageSelector;
