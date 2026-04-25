import { registerUser } from "./auth-service.js";
import { isValidEmail, setLoadingState, setMessage, togglePasswordFieldVisibility } from "./auth-ui.js";
import { initI18n, t } from "../i18n.js";
import { initTheme } from "../theme.js";

const form = document.querySelector("#register-form");
const messageElement = document.querySelector("#register-message");
const submitButton = document.querySelector("#register-submit");
const successBox = document.querySelector("#register-success-box");
const passwordInput = document.querySelector("#password");
const togglePasswordButton = document.querySelector("#toggle-register-password");

initI18n();
initTheme();
form?.addEventListener("submit", handleRegisterSubmit);
togglePasswordButton?.addEventListener("click", handlePasswordToggle);

async function handleRegisterSubmit(event) {
    event.preventDefault();

    const formData = new FormData(form);
    const name = formData.get("name")?.toString().trim() ?? "";
    const email = formData.get("email")?.toString().trim() ?? "";
    const password = formData.get("password")?.toString() ?? "";

    const validationError = validateRegisterForm({ name, email, password });
    if (validationError) {
        setMessage(messageElement, validationError, "error");
        successBox.hidden = true;
        return;
    }

    setMessage(messageElement, "", "neutral");
    setLoadingState(submitButton, true);
    successBox.hidden = true;

    try {
        await registerUser({ name, email, password });
        setMessage(messageElement, "", "neutral");
        form.reset();
        successBox.hidden = false;
        setLoadingState(submitButton, false);
    } catch (error) {
        setMessage(messageElement, error.message, "error");
        setLoadingState(submitButton, false);
    }
}

function validateRegisterForm({ name, email, password }) {
    if (!name || !email || !password) {
        return t("validation.register.required");
    }

    if (!isValidEmail(email)) {
        return t("validation.email.invalid");
    }

    if (password.length < 4) {
        return t("validation.password.min");
    }

    return "";
}

function handlePasswordToggle() {
    togglePasswordFieldVisibility(passwordInput, togglePasswordButton, t);
}
