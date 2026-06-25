import { API_BASE_URL } from "../lib/api";
import { createApiError } from "./apiError";

function buildAuthJsonHeaders(token) {
  return {
    "Content-Type": "application/json",
    Authorization: `Bearer ${token}`,
  };
}

function buildAuthHeaders(token) {
  return {
    Authorization: `Bearer ${token}`,
  };
}

async function postJson(path, body, token) {
  const response = await fetch(`${API_BASE_URL}${path}`, {
    method: "POST",
    headers: buildAuthJsonHeaders(token),
    body: JSON.stringify(body),
  });

  return handleJsonResponse(response);
}

async function getJson(path, token) {
  const response = await fetch(`${API_BASE_URL}${path}`, {
    headers: buildAuthHeaders(token),
  });

  return handleJsonResponse(response);
}

export async function createReservation({ seatId, token }) {
  if (!seatId) {
    throw new Error("No se pudo crear la reserva porque falta la butaca.");
  }

  const reservation = await postJson("/reservations", { seatId }, token);
  return normalizeReservationDates(reservation);
}

export async function createPayment({ reservationId, token }) {
  if (!reservationId) {
    throw new Error("No se pudo procesar el pago porque falta la reserva.");
  }

  return postJson("/payments", { reservationId }, token);
}

export async function getReservationById({ reservationId, token }) {
  if (!reservationId) {
    throw new Error("No se pudo cargar la reserva porque falta su identificador.");
  }

  const reservation = await getJson(`/reservations/${reservationId}`, token);
  return normalizeReservationDates(reservation);
}

async function handleJsonResponse(response) {
  const data = await response.json().catch(() => null);

  if (!response.ok) {
    throw createApiError(
      response,
      data,
      "No se pudo procesar la compra. Verifica la API e intenta de nuevo.",
    );
  }

  return data;
}

function normalizeReservationDates(reservation) {
  if (!reservation || typeof reservation !== "object") {
    return reservation;
  }

  return {
    ...reservation,
    reservedAt: normalizeApiDateTime(reservation.reservedAt),
    expiresAt: normalizeApiDateTime(reservation.expiresAt),
  };
}

function normalizeApiDateTime(value) {
  if (typeof value !== "string" || value.length === 0) {
    return value;
  }

  const hasExplicitTimezone = /([zZ]|[-+]\d{2}:\d{2})$/.test(value);
  return hasExplicitTimezone ? value : `${value}Z`;
}
