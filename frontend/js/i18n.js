const LANGUAGE_STORAGE_KEY = "ticketing_language";
let languageOptionsBound = false;

const translations = {
    es: {
        "page.login.title": "Iniciar sesión | Ticketing",
        "page.register.title": "Registrarse | Ticketing",
        "page.index.title": "Ticketing",
        "auth.login.eyebrow": "Bienvenido",
        "auth.login.title": "Iniciar sesión",
        "auth.login.subtitle": "Accede a tu cuenta para seguir con tus reservas.",
        "auth.register.eyebrow": "Ticketing",
        "auth.register.title": "Crear cuenta",
        "auth.register.subtitle": "Registra un nuevo usuario para ingresar al sistema y empezar a reservar tus entradas.",
        "field.name": "Nombre",
        "field.email": "Email",
        "field.password": "Contraseña",
        "field.rememberMe": "Recordarme",
        "button.login": "Ingresar",
        "button.register": "Registrarme",
        "link.createAccount": "Crear cuenta",
        "link.backHome": "Volver al inicio",
        "link.haveAccount": "Ya tengo cuenta",
        "link.goToLogin": "Ir a iniciar sesion",
        "nav.auth": "Navegación de autenticación",
        "index.title": "Frontend en construcción",
        "index.subtitle": "Base inicial para integrar login, registro y catalogo.",
        "index.noSession": "No hay una sesion iniciada.",
        "index.login": "Ir a login",
        "index.register": "Ir a registro",
        "index.logout": "Cerrar sesion",
        "auth.registerSuccess": "Cuenta creada con exito.",
        "auth.sessionActive": "Sesion iniciada como {name}.",
        "auth.showPassword": "Mostrar contraseña",
        "auth.hidePassword": "Ocultar contraseña",
        "validation.login.required": "Completa email y contraseña.",
        "validation.email.invalid": "Ingresa un email valido.",
        "validation.register.required": "Completa todos los campos.",
        "validation.password.min": "La contraseña debe tener al menos 4 caracteres.",
        "theme.toggle": "Cambiar tema",
        "lang.english": "ENGLISH (US)",
        "lang.spanish": "Español"
    },
    en: {
        "page.login.title": "Sign in | Ticketing",
        "page.register.title": "Sign up | Ticketing",
        "page.index.title": "Ticketing",
        "auth.login.eyebrow": "Welcome",
        "auth.login.title": "Sign in",
        "auth.login.subtitle": "Access your account to continue with your reservations.",
        "auth.register.eyebrow": "Ticketing",
        "auth.register.title": "Create account",
        "auth.register.subtitle": "Register a new user to enter the system and start booking your tickets.",
        "field.name": "Name",
        "field.email": "Email",
        "field.password": "Password",
        "field.rememberMe": "Remember me",
        "button.login": "Sign in",
        "button.register": "Sign up",
        "link.createAccount": "Create account",
        "link.backHome": "Back to home",
        "link.haveAccount": "I already have an account",
        "link.goToLogin": "Go to sign in",
        "nav.auth": "Authentication navigation",
        "index.title": "Frontend in progress",
        "index.subtitle": "Initial base to integrate login, registration and catalog.",
        "index.noSession": "There is no active session.",
        "index.login": "Go to sign in",
        "index.register": "Go to sign up",
        "index.logout": "Log out",
        "auth.registerSuccess": "Account created successfully.",
        "auth.sessionActive": "Signed in as {name}.",
        "auth.showPassword": "Show password",
        "auth.hidePassword": "Hide password",
        "validation.login.required": "Complete email and password.",
        "validation.email.invalid": "Enter a valid email.",
        "validation.register.required": "Complete all fields.",
        "validation.password.min": "Password must be at least 4 characters long.",
        "theme.toggle": "Toggle theme",
        "lang.english": "ENGLISH (US)",
        "lang.spanish": "Español"
    }
};

export function initI18n() {
    applyTranslations();
    if (!languageOptionsBound) {
        bindLanguageOptions();
        languageOptionsBound = true;
    }
}

export function t(key, params = {}) {
    const language = getLanguage();
    const value = translations[language]?.[key] ?? translations.es[key] ?? key;

    return Object.entries(params).reduce((result, [paramKey, paramValue]) => {
        return result.replaceAll(`{${paramKey}}`, String(paramValue));
    }, value);
}

export function getLanguage() {
    return localStorage.getItem(LANGUAGE_STORAGE_KEY) ?? "es";
}

export function setLanguage(language) {
    localStorage.setItem(LANGUAGE_STORAGE_KEY, language);
    applyTranslations();
}

export function applyTranslations() {
    document.documentElement.lang = getLanguage();

    document.querySelectorAll("[data-i18n]").forEach((element) => {
        const key = element.dataset.i18n;
        element.textContent = t(key);
    });

    document.querySelectorAll("[data-i18n-placeholder]").forEach((element) => {
        const key = element.dataset.i18nPlaceholder;
        element.setAttribute("placeholder", t(key));
    });

    document.querySelectorAll("[data-i18n-aria-label]").forEach((element) => {
        const key = element.dataset.i18nAriaLabel;
        element.setAttribute("aria-label", t(key));
    });

    const titleKey = document.body.dataset.i18nTitle;
    if (titleKey) {
        document.title = t(titleKey);
    }

    document.querySelectorAll("[data-language-option]").forEach((element) => {
        const optionLanguage = element.dataset.languageOption;
        const isActive = optionLanguage === getLanguage();
        element.classList.toggle("is-active", isActive);
        element.setAttribute("aria-pressed", String(isActive));
    });
}

function bindLanguageOptions() {
    document.querySelectorAll("[data-language-option]").forEach((element) => {
        element.addEventListener("click", () => {
            const language = element.dataset.languageOption ?? "es";
            setLanguage(language);
            window.location.reload();
        });
    });
}
