function isBlank(value) {
  return !value?.trim();
}

export function isValidEmail(email) {
  return /\S+@\S+\.\S+/.test(email.trim());
}

export function validateLoginForm({ email, password }, t, language) {
  if (isBlank(email) || isBlank(password)) {
    return t(language, "auth.validationMissingLogin");
  }

  if (!isValidEmail(email)) {
    return t(language, "auth.validationEmail");
  }

  return null;
}

export function validateRegisterForm({ name, email, password }, t, language) {
  if (isBlank(name) || isBlank(email) || isBlank(password)) {
    return t(language, "auth.validationMissingRegister");
  }

  if (!isValidEmail(email)) {
    return t(language, "auth.validationEmail");
  }

  if (password.length < 4) {
    return t(language, "auth.validationPassword");
  }

  return null;
}
