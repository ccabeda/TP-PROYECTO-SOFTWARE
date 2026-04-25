export function setMessage(messageElement, message, type = "neutral") {
    messageElement.textContent = message;
    messageElement.classList.remove("is-error", "is-success");

    if (type === "error") {
        messageElement.classList.add("is-error");
    }

    if (type === "success") {
        messageElement.classList.add("is-success");
    }
}

export function setLoadingState(submitButton, isLoading) {
    submitButton.disabled = isLoading;
    submitButton.classList.toggle("is-loading", isLoading);
}

export function isValidEmail(email) {
    return /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email);
}

export function togglePasswordFieldVisibility(passwordInput, togglePasswordButton, translate) {
    const isPasswordHidden = passwordInput.type === "password";
    passwordInput.type = isPasswordHidden ? "text" : "password";
    togglePasswordButton.classList.toggle("is-visible", isPasswordHidden);

    const labelKey = isPasswordHidden ? "auth.hidePassword" : "auth.showPassword";
    togglePasswordButton.dataset.i18nAriaLabel = labelKey;
    togglePasswordButton.setAttribute("aria-label", translate(labelKey));
}
