export const LANGUAGE_STORAGE_KEY = "ticketing_language";

export const translations = {
  es: {
    topbar: {
      home: "Inicio",
      events: "Eventos",
      login: "Iniciar sesión",
      register: "Crear cuenta",
      logout: "Cerrar sesión",
      greeting: "Hola, {name}",
      themeToLight: "Cambiar al modo claro",
      themeToDark: "Cambiar al modo oscuro",
    },
    auth: {
      loginEyebrow: "Bienvenido",
      loginTitle: "Iniciar sesión",
      loginSubtitle: "Accedé a tu cuenta para seguir con tus reservas.",
      registerEyebrow: "Tu próxima entrada empieza acá",
      registerTitle: "Crear cuenta",
      registerSubtitle:
        "Registrate para ver eventos, reservar y gestionar tus entradas.",
      name: "Nombre",
      email: "Email",
      password: "Contraseña",
      emailPlaceholder: "nombre@correo.com",
      namePlaceholder: "Tu nombre",
      loginPasswordPlaceholder: "Ingresá tu contraseña",
      registerPasswordPlaceholder: "Creá una contraseña",
      remember: "Recordarme",
      loginSubmit: "Ingresar",
      loginSubmitting: "Ingresando...",
      registerSubmit: "Crear cuenta",
      registerSubmitting: "Creando cuenta...",
      createAccount: "Crear cuenta",
      backHome: "Volver al inicio",
      alreadyHaveAccount: "Ya tengo cuenta",
      goToLogin: "Ir a iniciar sesión",
      showPassword: "Mostrar contraseña",
      hidePassword: "Ocultar contraseña",
      validationMissingLogin: "Completá email y contraseña.",
      validationMissingRegister: "Completá nombre, email y contraseña.",
      validationEmail: "Ingresá un email válido.",
      validationPassword: "La contraseña debe tener al menos 4 caracteres.",
      successRegister: "Cuenta creada con éxito.",
      languageEnglish: "ENGLISH (US)",
      languageSpanish: "Español",
    },
    home: {
      badge: "Eventos en vivo",
      title: "Viví tus eventos favoritos",
      copy:
        "Descubrí conciertos, festivales y experiencias únicas. Explorá la cartelera y conseguí tus entradas.",
      upcoming: "Próximos Eventos",
      viewAll: "Ver todos",
      moreInfo: "Más info",
      buy: "Comprar",
      allEvents: "Todos los Eventos",
      detailBadge: "Evento destacado",
      detailCopy:
        "Próximamente vamos a conectar esta vista con el backend para mostrar sectores, disponibilidad y compra real de entradas.",
      detailTitle: "Detalle del Evento",
    },
  },
  en: {
    topbar: {
      home: "Home",
      events: "Events",
      login: "Log in",
      register: "Create account",
      logout: "Log out",
      greeting: "Hi, {name}",
      themeToLight: "Switch to light mode",
      themeToDark: "Switch to dark mode",
    },
    auth: {
      loginEyebrow: "Welcome",
      loginTitle: "Log in",
      loginSubtitle: "Access your account to continue with your reservations.",
      registerEyebrow: "Your next ticket starts here",
      registerTitle: "Create account",
      registerSubtitle:
        "Sign up to browse events, book seats and manage your tickets.",
      name: "Name",
      email: "Email",
      password: "Password",
      emailPlaceholder: "name@email.com",
      namePlaceholder: "Your name",
      loginPasswordPlaceholder: "Enter your password",
      registerPasswordPlaceholder: "Create a password",
      remember: "Remember me",
      loginSubmit: "Log in",
      loginSubmitting: "Logging in...",
      registerSubmit: "Create account",
      registerSubmitting: "Creating account...",
      createAccount: "Create account",
      backHome: "Back home",
      alreadyHaveAccount: "I already have an account",
      goToLogin: "Go to log in",
      showPassword: "Show password",
      hidePassword: "Hide password",
      validationMissingLogin: "Complete email and password.",
      validationMissingRegister: "Complete name, email and password.",
      validationEmail: "Enter a valid email address.",
      validationPassword: "Password must be at least 4 characters long.",
      successRegister: "Account created successfully.",
      languageEnglish: "ENGLISH (US)",
      languageSpanish: "Español",
    },
    home: {
      badge: "Live events",
      title: "Live your favorite events",
      copy:
        "Discover concerts, festivals and unique experiences. Explore the lineup and get your tickets.",
      upcoming: "Upcoming Events",
      viewAll: "View all",
      moreInfo: "More info",
      buy: "Buy",
      allEvents: "All Events",
      detailBadge: "Featured event",
      detailCopy:
        "Soon we will connect this view to the backend to show sectors, availability and real ticket purchase.",
      detailTitle: "Event Detail",
    },
  },
};

export function getStoredLanguage() {
  const saved = localStorage.getItem(LANGUAGE_STORAGE_KEY);
  return saved === "en" ? "en" : "es";
}

export function persistLanguage(language) {
  localStorage.setItem(LANGUAGE_STORAGE_KEY, language);
}

export function t(language, key, replacements = {}) {
  const value = key.split(".").reduce((current, part) => current?.[part], translations[language]);
  if (!value) {
    return key;
  }

  return Object.entries(replacements).reduce(
    (current, [replacementKey, replacementValue]) =>
      current.replace(`{${replacementKey}}`, replacementValue),
    value,
  );
}
