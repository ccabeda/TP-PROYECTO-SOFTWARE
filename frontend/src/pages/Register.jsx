import { useEffect, useMemo, useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import AuthLayout from "../components/auth/AuthLayout";
import PasswordField from "../components/auth/PasswordField";
import { useAuthContext } from "../context/AuthContext";
import { useLanguageContext } from "../context/LanguageContext";
import useDocumentTitle from "../hooks/useDocumentTitle";
import getErrorMessage from "../lib/getErrorMessage";
import { t } from "../lib/i18n";
import { validateRegisterForm } from "../lib/authValidation";
import { registerUser } from "../services/authService";

const INITIAL_FORM_DATA = {
  name: "",
  email: "",
  password: "",
};

function Register() {
  const navigate = useNavigate();
  const { language } = useLanguageContext();
  const { session } = useAuthContext();
  const [formData, setFormData] = useState(INITIAL_FORM_DATA);
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [message, setMessage] = useState("");
  const [messageType, setMessageType] = useState("");
  const [showLoginLink, setShowLoginLink] = useState(false);
  const registerEyebrow = t(language, "auth.registerEyebrow");
  const registerTitle = t(language, "auth.registerTitle");
  const registerSubtitle = t(language, "auth.registerSubtitle");
  const nameLabel = t(language, "auth.name");
  const namePlaceholder = t(language, "auth.namePlaceholder");
  const emailLabel = t(language, "auth.email");
  const emailPlaceholder = t(language, "auth.emailPlaceholder");
  const passwordLabel = t(language, "auth.password");
  const passwordPlaceholder = t(language, "auth.registerPasswordPlaceholder");
  const goToLoginLabel = t(language, "auth.goToLogin");
  const registerSubmittingLabel = t(language, "auth.registerSubmitting");
  const registerSubmitLabel = t(language, "auth.registerSubmit");
  const alreadyHaveAccountLabel = t(language, "auth.alreadyHaveAccount");
  const backHomeLabel = t(language, "auth.backHome");
  const successRegisterLabel = t(language, "auth.successRegister");
  useDocumentTitle(t(language, "topbar.register"));

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

    const validationMessage = validateRegisterForm(formData, t, language);
    if (validationMessage) {
      setMessage(validationMessage);
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
      setMessage(successRegisterLabel);
      setMessageType("success");
      setShowLoginLink(true);
      setFormData(INITIAL_FORM_DATA);
    } catch (error) {
      setMessage(getErrorMessage(error, t(language, "auth.registerError")));
      setMessageType("error");
      setShowLoginLink(false);
    } finally {
      setIsSubmitting(false);
    }
  }

  return (
    <AuthLayout
      eyebrow={registerEyebrow}
      title={registerTitle}
      subtitle={registerSubtitle}
    >
      <form className="auth-form" onSubmit={handleSubmit} noValidate>
        <label className="auth-label" htmlFor="register-name">
          {nameLabel}
        </label>
        <input
          id="register-name"
          className="auth-input"
          name="name"
          type="text"
          autoComplete="name"
          placeholder={namePlaceholder}
          value={formData.name}
          onChange={handleChange}
          disabled={isSubmitting}
          autoFocus
        />

        <label className="auth-label" htmlFor="register-email">
          {emailLabel}
        </label>
        <input
          id="register-email"
          className="auth-input"
          name="email"
          type="email"
          autoComplete="email"
          placeholder={emailPlaceholder}
          value={formData.email}
          onChange={handleChange}
          disabled={isSubmitting}
        />

        <label className="auth-label" htmlFor="register-password">
          {passwordLabel}
        </label>
        <PasswordField
          id="register-password"
          name="password"
          autoComplete="new-password"
          placeholder={passwordPlaceholder}
          value={formData.password}
          onChange={handleChange}
          disabled={isSubmitting}
          language={language}
        />

        {message ? (
          <div className={`auth-alert auth-alert-${messageType}`}>
            {message}
            {showLoginLink ? (
              <Link className="auth-inline-link" to="/login">
                {goToLoginLabel}
              </Link>
            ) : null}
          </div>
        ) : null}

        <button type="submit" className="btn btn-primary auth-submit" disabled={isSubmitting}>
          <span
            className={`auth-button-spinner ${isSubmitting ? "is-visible" : ""}`}
            aria-hidden="true"
          />
          <span className="auth-button-label">
            {isSubmitting ? registerSubmittingLabel : registerSubmitLabel}
          </span>
        </button>
      </form>

      <div className="auth-links">
        <Link className="auth-secondary-link" to="/login">
          {alreadyHaveAccountLabel}
        </Link>
        <Link className="auth-secondary-link" to="/">
          {backHomeLabel}
        </Link>
      </div>
    </AuthLayout>
  );
}

export default Register;
