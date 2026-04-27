import { API_BASE_URL } from "../lib/api";

const AUTH_STORAGE_KEY = "ticketing_auth";
const JSON_HEADERS = {
  "Content-Type": "application/json",
};

function isBrowser() {
  return typeof window !== "undefined";
}

function getPersistentStorage(rememberMe) {
  if (!isBrowser()) {
    return null;
  }

  return rememberMe ? window.localStorage : window.sessionStorage;
}

function getStoredSessionValue() {
  if (!isBrowser()) {
    return null;
  }

  return (
    window.localStorage.getItem(AUTH_STORAGE_KEY) ??
    window.sessionStorage.getItem(AUTH_STORAGE_KEY)
  );
}

function parseStoredSession(rawSession) {
  if (!rawSession) {
    return null;
  }

  try {
    return JSON.parse(rawSession);
  } catch {
    clearSession();
    return null;
  }
}

export async function registerUser({ name, email, password }) {
  const response = await fetch(`${API_BASE_URL}/users`, {
    method: "POST",
    headers: JSON_HEADERS,
    body: JSON.stringify({ name, email, password }),
  });

  return handleJsonResponse(response);
}

export async function loginUser({ email, password, rememberMe = true }) {
  const response = await fetch(`${API_BASE_URL}/users/login`, {
    method: "POST",
    headers: JSON_HEADERS,
    body: JSON.stringify({ email, password }),
  });

  const data = await handleJsonResponse(response);
  saveSession(data, rememberMe);
  return data;
}

export async function getCurrentUser() {
  const token = getToken();
  if (!token) {
    return null;
  }

  const response = await fetch(`${API_BASE_URL}/users/me`, {
    headers: {
      Authorization: `Bearer ${token}`,
    },
  });

  if (response.status === 401) {
    clearSession();
    return null;
  }

  return handleJsonResponse(response);
}

export function saveSession(session, rememberMe = true) {
  if (!session) {
    return;
  }

  clearSession();
  const storage = getPersistentStorage(rememberMe);

  if (!storage) {
    return;
  }

  storage.setItem(AUTH_STORAGE_KEY, JSON.stringify(session));
}

export function getSession() {
  return parseStoredSession(getStoredSessionValue());
}

export function getToken() {
  return getSession()?.token ?? null;
}

export function clearSession() {
  if (!isBrowser()) {
    return;
  }

  window.localStorage.removeItem(AUTH_STORAGE_KEY);
  window.sessionStorage.removeItem(AUTH_STORAGE_KEY);
}

export function logoutUser() {
  clearSession();
}

async function handleJsonResponse(response) {
  const data = await response.json().catch(() => null);

  if (!response.ok) {
    throw new Error(
      data?.message ??
        "No se pudo conectar con la API. Verifica el backend e intenta de nuevo.",
    );
  }

  return data;
}
