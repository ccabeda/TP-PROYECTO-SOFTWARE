import { useEffect, useMemo, useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import LanguageSelector from "../components/LanguageSelector";
import ThemeToggle from "../components/ThemeToggle";
import { t } from "../i18n";
import { registerUser } from "../services/authService";

function Register({
  darkMode,
  setDarkMode,
  session,
  language,
  onLanguageChange,
}) {
  const navigate = useNavigate();
  const [formData, setFormData] = useState({
    name: "",
    email: "",
    password: "",
  });
  const [showPassword, setShowPassword] = useState(false);
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [message, setMessage] = useState("");
  const [messageType, setMessageType] = useState("");
  const [showLoginLink, setShowLoginLink] = useState(false);

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
    const { name, value } = event.target;
    setFormData((current) => ({
      ...current,
      [name]: value,
    }));
  }

  async function handleSubmit(event) {
    event.preventDefault();

    if (
      !formData.name.trim() ||
      !formData.email.trim() ||
      !formData.password.trim()
    ) {
      setMessage(t(language, "auth.validationMissingRegister"));
      setMessageType("error");
      setShowLoginLink(false);
      return;
    }

    if (!/\S+@\S+\.\S+/.test(formData.email)) {
      setMessage(t(language, "auth.validationEmail"));
      setMessageType("error");
      setShowLoginLink(false);
      return;
    }

    if (formData.password.length < 4) {
      setMessage(t(language, "auth.validationPassword"));
      setMessageType("error");
      setShowLoginLink(false);
      return;
    }

    setIsSubmitting(true);
    setMessage("");
    setMessageType("");
    setShowLoginLink(false);

    try {
      await registerUser(formData);
      setMessage(t(language, "auth.successRegister"));
      setMessageType("success");
      setShowLoginLink(true);
      setFormData({
        name: "",
        email: "",
        password: "",
      });
    } catch (error) {
      setMessage(error.message);
      setMessageType("error");
      setShowLoginLink(false);
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
          <p className="auth-eyebrow">{t(language, "auth.registerEyebrow")}</p>
          <h1 className="auth-title">{t(language, "auth.registerTitle")}</h1>
          <p className="auth-subtitle">
            {t(language, "auth.registerSubtitle")}
          </p>

          <form className="auth-form" onSubmit={handleSubmit} noValidate>
            <label className="auth-label" htmlFor="register-name">
              {t(language, "auth.name")}
            </label>
            <input
              id="register-name"
              className="auth-input"
              name="name"
              type="text"
              autoComplete="name"
              placeholder={t(language, "auth.namePlaceholder")}
              value={formData.name}
              onChange={handleChange}
              disabled={isSubmitting}
              autoFocus
            />

            <label className="auth-label" htmlFor="register-email">
              {t(language, "auth.email")}
            </label>
            <input
              id="register-email"
              className="auth-input"
              name="email"
              type="email"
              autoComplete="email"
              placeholder={t(language, "auth.emailPlaceholder")}
              value={formData.email}
              onChange={handleChange}
              disabled={isSubmitting}
            />

            <label className="auth-label" htmlFor="register-password">
              {t(language, "auth.password")}
            </label>
            <div className="password-input-group">
              <input
                id="register-password"
                className="auth-input"
                name="password"
                type={showPassword ? "text" : "password"}
                autoComplete="new-password"
                placeholder={t(language, "auth.registerPasswordPlaceholder")}
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

            {message ? (
              <div className={`auth-alert auth-alert-${messageType}`}>
                {message}
                {showLoginLink ? (
                  <Link className="auth-inline-link" to="/login">
                    {t(language, "auth.goToLogin")}
                  </Link>
                ) : null}
              </div>
            ) : null}

            <button className="btn btn-primary auth-submit" disabled={isSubmitting}>
              <span
                className={`auth-button-spinner ${isSubmitting ? "is-visible" : ""}`}
                aria-hidden="true"
              />
              <span className="auth-button-label">
                {isSubmitting
                  ? t(language, "auth.registerSubmitting")
                  : t(language, "auth.registerSubmit")}
              </span>
            </button>
          </form>

          <div className="auth-links">
            <Link className="auth-secondary-link" to="/login">
              {t(language, "auth.alreadyHaveAccount")}
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

export default Register;
