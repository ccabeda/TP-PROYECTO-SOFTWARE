import LanguageSelector from "../ui/LanguageSelector";
import ThemeToggle from "../ui/ThemeToggle";
import useLanguageContext from "../../context/useLanguageContext";
import useThemeContext from "../../context/useThemeContext";
import { t } from "../../lib/i18n";

function AuthLayout({ eyebrow, title, subtitle, children }) {
  const { darkMode, setDarkMode } = useThemeContext();
  const { language, setLanguage } = useLanguageContext();
  const themeAriaLabel = t(
    language,
    darkMode ? "topbar.themeToLight" : "topbar.themeToDark",
  );
  const englishLabel = t(language, "auth.languageEnglish");
  const spanishLabel = t(language, "auth.languageSpanish");

  return (
    <div className={darkMode ? "page dark" : "page"}>
      <main className="auth-page">
        <ThemeToggle
          className="auth-theme-button"
          darkMode={darkMode}
          onToggle={() => setDarkMode((current) => !current)}
          ariaLabel={themeAriaLabel}
        />

        <section className="auth-card">
          <p className="auth-eyebrow">{eyebrow}</p>
          <h1 className="auth-title">{title}</h1>
          <p className="auth-subtitle">{subtitle}</p>
          {children}
        </section>

        <LanguageSelector
          language={language}
          onChange={setLanguage}
          englishLabel={englishLabel}
          spanishLabel={spanishLabel}
        />
      </main>
    </div>
  );
}

export default AuthLayout;
