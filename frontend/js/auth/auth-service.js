import { API_BASE_URL, AUTH_STORAGE_KEY } from "../config.js";

export async function registerUser({ name, email, password }) {
    const response = await fetch(`${API_BASE_URL}/users`, {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify({ name, email, password })
    });

    return handleJsonResponse(response);
}

export async function loginUser({ email, password, rememberMe = true }) {
    const response = await fetch(`${API_BASE_URL}/users/login`, {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify({ email, password })
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
            Authorization: `Bearer ${token}`
        }
    });

    if (response.status === 401) {
        clearSession();
        return null;
    }

    return handleJsonResponse(response);
}

export function saveSession(session, rememberMe = true) {
    clearSession();
    const storage = rememberMe ? localStorage : sessionStorage;
    storage.setItem(AUTH_STORAGE_KEY, JSON.stringify(session));
}

export function getSession() {
    const rawSession = localStorage.getItem(AUTH_STORAGE_KEY) ?? sessionStorage.getItem(AUTH_STORAGE_KEY);
    return rawSession ? JSON.parse(rawSession) : null;
}

export function getToken() {
    return getSession()?.token ?? null;
}

export function isAuthenticated() {
    return Boolean(getToken());
}

export function clearSession() {
    localStorage.removeItem(AUTH_STORAGE_KEY);
    sessionStorage.removeItem(AUTH_STORAGE_KEY);
}

export function logoutUser() {
    clearSession();
}

async function handleJsonResponse(response) {
    const data = await response.json().catch(() => null);

    if (!response.ok) {
        throw new Error(data?.message ?? "Ocurrió un error inesperado.");
    }

    return data;
}
