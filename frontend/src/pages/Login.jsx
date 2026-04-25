import { useEffect, useMemo, useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import LanguageSelector from "../components/LanguageSelector";
import ThemeToggle from "../components/ThemeToggle";
import { t } from "../i18n";
import { loginUser } from "../services/authService";

function Login({
  darkMode,
  setDarkMode,
  session,
  setSession,
  language,
  onLanguageChange,
}) {
  const navigate = useNavigate();
  const [formData, setFormData] = useState({
    email: "",
    password: "",
    rememberMe: true,
  });
  const [showPassword, setShowPassword] = useState(false);
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [message, setMessage] = useState("");
  const [messageType, setMessageType] = useState("");

  const pageClassName = useMemo(
    () => (darkMode ? "page dark" : "page"),
    [darkMode],
  );

  useEffect(() => {
    if (session?.token) {
      navigate("/", { replace: true });
    }
  }, [navigate, session]);

  function handleChange(event) {
    const { name, value, type, checked } = event.target;
    setFormData((current) => ({
      ...current,
      [name]: type === "checkbox" ? checked : value,
    }));
  }

  async function handleSubmit(event) {
    event.preventDefault();

    if (!formData.email.trim() || !formData.password.trim()) {
      setMessage(t(language, "auth.validationMissingLogin"));
      setMessageType("error");
      return;
    }

    if (!/\S+@\S+\.\S+/.test(formData.email)) {
      setMessage(t(language, "auth.validationEmail"));
      setMessageType("error");
      return;
    }

    setIsSubmitting(true);
    setMessage("");
    setMessageType("");

    try {
      const authSession = await loginUser(formData);
      setSession(authSession);
      navigate("/", { replace: true });
    } catch (error) {
      setMessage(error.message);
      setMessageType("error");
    } finally {
      setIsSubmitting(false);
    }
  }

  return (
    <div className={pageClassName}>
      <main className="auth-page">
        <ThemeToggle
          className="auth-theme-button"
          darkMode={darkMode}
          onToggle={() => setDarkMode(!darkMode)}
          ariaLabel={t(
            language,
            darkMode ? "topbar.themeToLight" : "topbar.themeToDark",
          )}
        />

        <section className="auth-card">
          <p className="auth-eyebrow">{t(language, "auth.loginEyebrow")}</p>
          <h1 className="auth-title">{t(language, "auth.loginTitle")}</h1>
          <p className="auth-subtitle">{t(language, "auth.loginSubtitle")}</p>

          <form className="auth-form" onSubmit={handleSubmit} noValidate>
            <label className="auth-label" htmlFor="login-email">
              {t(language, "auth.email")}
            </label>
            <input
              id="login-email"
              className="auth-input"
              name="email"
              type="email"
              autoComplete="email"
              placeholder={t(language, "auth.emailPlaceholder")}
              value={formData.email}
              onChange={handleChange}
              disabled={isSubmitting}
              autoFocus
            />

            <label className="auth-label" htmlFor="login-password">
              {t(language, "auth.password")}
            </label>
            <div className="password-input-group">
              <input
                id="login-password"
                className="auth-input"
                name="password"
                type={showPassword ? "text" : "password"}
                autoComplete="current-password"
                placeholder={t(language, "auth.loginPasswordPlaceholder")}
                value={formData.password}
                onChange={handleChange}
                disabled={isSubmitting}
              />
              <button
                type="button"
                className="password-toggle"
                onClick={() => setShowPassword((current) => !current)}
                aria-label={t(
                  language,
                  showPassword ? "auth.hidePassword" : "auth.showPassword",
                )}
              >
                <img
                  src={showPassword ? "/eye-closed.svg" : "/eye-open.svg"}
                  alt=""
                  className="password-toggle-icon"
                />
              </button>
            </div>

            <label className="remember-row">
              <input
                name="rememberMe"
                type="checkbox"
                checked={formData.rememberMe}
                onChange={handleChange}
                disabled={isSubmitting}
              />
              <span>{t(language, "auth.remember")}</span>
            </label>

            {message ? (
              <div className={`auth-alert auth-alert-${messageType}`}>
                {message}
              </div>
            ) : null}

            <button className="btn btn-primary auth-submit" disabled={isSubmitting}>
              <span
                className={`auth-button-spinner ${isSubmitting ? "is-visible" : ""}`}
                aria-hidden="true"
              />
              <span className="auth-button-label">
                {isSubmitting
                  ? t(language, "auth.loginSubmitting")
                  : t(language, "auth.loginSubmit")}
              </span>
            </button>
          </form>

          <div className="auth-links">
            <Link className="auth-secondary-link" to="/register">
              {t(language, "auth.createAccount")}
            </Link>
            <Link className="auth-secondary-link" to="/">
              {t(language, "auth.backHome")}
            </Link>
          </div>
        </section>

        <LanguageSelector
          language={language}
          onChange={onLanguageChange}
          englishLabel={t(language, "auth.languageEnglish")}
          spanishLabel={t(language, "auth.languageSpanish")}
        />
      </main>
    </div>
  );
}

export default Login;
