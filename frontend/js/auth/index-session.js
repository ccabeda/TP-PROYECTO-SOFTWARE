import { getSession, logoutUser } from "./auth-service.js";
import { initI18n, t } from "../i18n.js";
import { initTheme } from "../theme.js";

const sessionMessageElement = document.querySelector("#session-message");
const logoutButton = document.querySelector("#logout-button");
const loginLink = document.querySelector("#login-link");
const registerLink = document.querySelector("#register-link");

initI18n();
initTheme();
renderSessionState();

logoutButton?.addEventListener("click", () => {
    logoutUser();
    window.location.href = "./login.html";
});

function renderSessionState() {
    const session = getSession();

    if (!session) {
        sessionMessageElement.textContent = t("index.noSession");
        logoutButton.hidden = true;
        loginLink.hidden = false;
        registerLink.hidden = false;
        return;
    }

    sessionMessageElement.textContent = t("auth.sessionActive", { name: session.name });
    logoutButton.hidden = false;
    loginLink.hidden = true;
    registerLink.hidden = true;
}
