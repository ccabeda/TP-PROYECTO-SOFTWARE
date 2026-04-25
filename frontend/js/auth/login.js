import { getSession, loginUser } from "./auth-service.js";
import { isValidEmail, setLoadingState, setMessage, togglePasswordFieldVisibility } from "./auth-ui.js";
import { initI18n, t } from "../i18n.js";
import { initTheme } from "../theme.js";

const form = document.querySelector("#login-form");
const messageElement = document.querySelector("#login-message");
const submitButton = document.querySelector("#login-submit");
const passwordInput = document.querySelector("#password");
const togglePasswordButton = document.querySelector("#toggle-login-password");

initI18n();
initTheme();
redirectIfAuthenticated();

form?.addEventListener("submit", handleLoginSubmit);
togglePasswordButton?.addEventListener("click", handlePasswordToggle);

async function handleLoginSubmit(event) {
    event.preventDefault();

    const formData = new FormData(form);
    const email = formData.get("email")?.toString().trim() ?? "";
    const password = formData.get("password")?.toString() ?? "";
    const rememberMe = formData.get("rememberMe") === "on";

    const validationError = validateLoginForm({ email, password });
    if (validationError) {
        setMessage(messageElement, validationError, "error");
        return;
    }

    setMessage(messageElement, "", "neutral");
    setLoadingState(submitButton, true);

    try {
        await loginUser({ email, password, rememberMe });
        window.location.href = "./index.html";
    } catch (error) {
        setMessage(messageElement, error.message, "error");
        setLoadingState(submitButton, false);
    }
}

function redirectIfAuthenticated() {
    if (getSession()) {
        window.location.href = "./index.html";
    }
}

function validateLoginForm({ email, password }) {
    if (!email || !password) {
        return t("validation.login.required");
    }

    if (!isValidEmail(email)) {
        return t("validation.email.invalid");
    }

    return "";
}

function handlePasswordToggle() {
    togglePasswordFieldVisibility(passwordInput, togglePasswordButton, t);
}
