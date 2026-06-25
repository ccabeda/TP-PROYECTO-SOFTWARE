import { API_BASE_URL } from "../lib/api";
import { createApiError } from "./apiError";

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

function getStoredSessionStorage() {
  if (!isBrowser()) {
    return null;
  }

  if (window.localStorage.getItem(AUTH_STORAGE_KEY)) {
    return window.localStorage;
  }

  if (window.sessionStorage.getItem(AUTH_STORAGE_KEY)) {
    return window.sessionStorage;
  }

  return null;
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
    const refreshedSession = await refreshSession();
    if (!refreshedSession?.token) {
      clearSession();
      return null;
    }

    const retryResponse = await fetch(`${API_BASE_URL}/users/me`, {
      headers: {
        Authorization: `Bearer ${refreshedSession.token}`,
      },
    });

    if (retryResponse.status === 401) {
      clearSession();
      return null;
    }

    return handleJsonResponse(retryResponse);
  }

  return handleJsonResponse(response);
}

export async function refreshSession() {
  const session = getSession();
  if (!session?.refreshToken) {
    return null;
  }

  const response = await fetch(`${API_BASE_URL}/users/refresh-token`, {
    method: "POST",
    headers: JSON_HEADERS,
    body: JSON.stringify({ refreshToken: session.refreshToken }),
  });

  if (!response.ok) {
    clearSession();
    return null;
  }

  const nextSession = await handleJsonResponse(response);
  saveSessionInCurrentStorage(nextSession);
  return nextSession;
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

function saveSessionInCurrentStorage(session) {
  if (!session) {
    return;
  }

  const storage = getStoredSessionStorage() ?? getPersistentStorage(true);
  clearSession();
  storage?.setItem(AUTH_STORAGE_KEY, JSON.stringify(session));
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

export async function logoutUser() {
  const token = getToken();

  try {
    if (token) {
      await fetch(`${API_BASE_URL}/users/logout`, {
        method: "POST",
        headers: {
          Authorization: `Bearer ${token}`,
        },
      });
    }
  } finally {
    clearSession();
  }
}

async function handleJsonResponse(response) {
  const data = await response.json().catch(() => null);

  if (!response.ok) {
    throw createApiError(
      response,
      data,
      "No se pudo conectar con la API. Verifica el backend e intenta de nuevo.",
    );
  }

  return data;
}
