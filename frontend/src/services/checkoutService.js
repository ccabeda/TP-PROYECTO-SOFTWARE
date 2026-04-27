import { API_BASE_URL } from "../lib/api";

function buildAuthJsonHeaders(token) {
  return {
    "Content-Type": "application/json",
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

export async function createReservation({ seatId, token }) {
  if (!seatId) {
    throw new Error("No se pudo crear la reserva porque falta la butaca.");
  }

  return postJson("/reservations", { seatId }, token);
}

export async function createPayment({ reservationId, token }) {
  if (!reservationId) {
    throw new Error("No se pudo procesar el pago porque falta la reserva.");
  }

  return postJson("/payments", { reservationId }, token);
}

async function handleJsonResponse(response) {
  const data = await response.json().catch(() => null);

  if (!response.ok) {
    throw new Error(
      data?.message ??
        "No se pudo procesar la compra. Verifica la API e intenta de nuevo."
    );
  }

  return data;
}
