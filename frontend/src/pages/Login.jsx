import { useEffect, useState } from "react";
import { Link, useLocation, useNavigate } from "react-router-dom";
import AuthLayout from "../components/auth/AuthLayout";
import PasswordField from "../components/auth/PasswordField";
import useAuthContext from "../context/useAuthContext";
import useLanguageContext from "../context/useLanguageContext";
import useDocumentTitle from "../hooks/useDocumentTitle";
import getErrorMessage from "../lib/getErrorMessage";
import { t } from "../lib/i18n";
import { validateLoginForm } from "../lib/authValidation";
import { loginUser } from "../services/authService";

const LOGIN_REDIRECT_DELAY_MS = 250;

function getRedirectTarget(location) {
  return location.state?.redirectTo ?? "/";
}

function Login() {
  const navigate = useNavigate();
  const location = useLocation();
  const { language } = useLanguageContext();
  const { session, setSession } = useAuthContext();
  const redirectTo = getRedirectTarget(location);
  const [formData, setFormData] = useState({
    email: "",
    password: "",
    rememberMe: true,
  });
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [message, setMessage] = useState("");
  const [messageType, setMessageType] = useState("");
  const loginEyebrow = t(language, "auth.loginEyebrow");
  const loginTitle = t(language, "auth.loginTitle");
  const loginSubtitle = t(language, "auth.loginSubtitle");
  const emailLabel = t(language, "auth.email");
  const passwordLabel = t(language, "auth.password");
  const emailPlaceholder = t(language, "auth.emailPlaceholder");
  const passwordPlaceholder = t(language, "auth.loginPasswordPlaceholder");
  const rememberLabel = t(language, "auth.remember");
  const loginSubmittingLabel = t(language, "auth.loginSubmitting");
  const loginSubmitLabel = t(language, "auth.loginSubmit");
  const createAccountLabel = t(language, "auth.createAccount");
  const backHomeLabel = t(language, "auth.backHome");
  useDocumentTitle(t(language, "topbar.login"));

  useEffect(() => {
    if (session?.token) {
      navigate(redirectTo, { replace: true });
    }
  }, [navigate, redirectTo, session]);

  function handleChange(event) {
    const { name, value, type, checked } = event.target;
    setFormData((current) => ({
      ...current,
      [name]: type === "checkbox" ? checked : value,
    }));
  }

  async function handleSubmit(event) {
    event.preventDefault();

    const validationMessage = validateLoginForm(formData, t, language);
    if (validationMessage) {
      setMessage(validationMessage);
      setMessageType("error");
      return;
    }

    setIsSubmitting(true);
    setMessage("");
    setMessageType("");

    try {
      const authSession = await loginUser(formData);
      setSession(authSession);
      window.setTimeout(() => {
        window.location.assign(redirectTo);
      }, LOGIN_REDIRECT_DELAY_MS);
    } catch (error) {
      setMessage(getErrorMessage(error, t(language, "auth.loginError")));
      setMessageType("error");
    } finally {
      setIsSubmitting(false);
    }
  }

  return (
    <AuthLayout
      eyebrow={loginEyebrow}
      title={loginTitle}
      subtitle={loginSubtitle}
    >
      <form className="auth-form" onSubmit={handleSubmit} noValidate>
        <label className="auth-label" htmlFor="login-email">
          {emailLabel}
        </label>
        <input
          id="login-email"
          className="auth-input"
          name="email"
          type="email"
          autoComplete="email"
          placeholder={emailPlaceholder}
          value={formData.email}
          onChange={handleChange}
          disabled={isSubmitting}
          autoFocus
        />

        <label className="auth-label" htmlFor="login-password">
          {passwordLabel}
        </label>
        <PasswordField
          id="login-password"
          name="password"
          autoComplete="current-password"
          placeholder={passwordPlaceholder}
          value={formData.password}
          onChange={handleChange}
          disabled={isSubmitting}
          language={language}
        />

        <label className="remember-row">
          <input
            name="rememberMe"
            type="checkbox"
            checked={formData.rememberMe}
            onChange={handleChange}
            disabled={isSubmitting}
          />
          <span>{rememberLabel}</span>
        </label>

        {message ? (
          <div className={`auth-alert auth-alert-${messageType}`}>{message}</div>
        ) : null}

        <button type="submit" className="btn btn-primary auth-submit" disabled={isSubmitting}>
          <span
            className={`auth-button-spinner ${isSubmitting ? "is-visible" : ""}`}
            aria-hidden="true"
          />
          <span className="auth-button-label">
            {isSubmitting ? loginSubmittingLabel : loginSubmitLabel}
          </span>
        </button>
      </form>

      <div className="auth-links">
        <Link className="auth-secondary-link" to="/register">
          {createAccountLabel}
        </Link>
        <Link className="auth-secondary-link" to="/">
          {backHomeLabel}
        </Link>
      </div>
    </AuthLayout>
  );
}

export default Login;
