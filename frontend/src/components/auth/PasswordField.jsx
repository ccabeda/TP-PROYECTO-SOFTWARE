import { useState } from "react";
import { t } from "../../lib/i18n";

function PasswordField({
  id,
  name,
  value,
  onChange,
  placeholder,
  autoComplete,
  disabled,
  language,
}) {
  const [showPassword, setShowPassword] = useState(false);
  const toggleLabel = t(
    language,
    showPassword ? "auth.hidePassword" : "auth.showPassword",
  );
  const toggleIconSrc = showPassword ? "/eye-closed.svg" : "/eye-open.svg";

  return (
    <div className="password-input-group">
      <input
        id={id}
        className="auth-input"
        name={name}
        type={showPassword ? "text" : "password"}
        autoComplete={autoComplete}
        placeholder={placeholder}
        value={value}
        onChange={onChange}
        disabled={disabled}
      />
      <button
        type="button"
        className="password-toggle"
        disabled={disabled}
        onClick={() => setShowPassword((current) => !current)}
        aria-label={toggleLabel}
        aria-pressed={showPassword}
      >
        <img
          src={toggleIconSrc}
          alt=""
          className="password-toggle-icon"
        />
      </button>
    </div>
  );
}

export default PasswordField;
