const THEME_STORAGE_KEY = "ticketing_theme";
let themeTogglesBound = false;

export function initTheme() {
    applySavedTheme();
    if (!themeTogglesBound) {
        bindThemeToggles();
        themeTogglesBound = true;
    }
}

export function getTheme() {
    return localStorage.getItem(THEME_STORAGE_KEY) ?? "light";
}

export function setTheme(theme) {
    localStorage.setItem(THEME_STORAGE_KEY, theme);
    applyTheme(theme);
}

function applySavedTheme() {
    applyTheme(getTheme());
}

function applyTheme(theme) {
    document.documentElement.classList.toggle("theme-dark", theme === "dark");

    document.querySelectorAll("[data-theme-toggle]").forEach((button) => {
        const isDark = theme === "dark";
        button.classList.toggle("is-dark", isDark);
        button.setAttribute("aria-pressed", String(isDark));
    });
}

function bindThemeToggles() {
    document.querySelectorAll("[data-theme-toggle]").forEach((button) => {
        button.addEventListener("click", () => {
            const nextTheme = getTheme() === "dark" ? "light" : "dark";
            setTheme(nextTheme);
        });
    });
}
