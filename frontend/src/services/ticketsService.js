import { API_BASE_URL } from "../lib/api";
import { createApiError } from "./apiError";

function buildAuthHeaders(token) {
  return {
    Authorization: `Bearer ${token}`,
  };
}

async function fetchJson(path, options) {
  const response = await fetch(`${API_BASE_URL}${path}`, options);
  return handleJsonResponse(response);
}

export async function getMyTickets({ token }) {
  if (!token) {
    throw new Error("No se pudieron cargar tus entradas porque falta la sesión.");
  }

  const data = await fetchJson("/reservations/mine", {
    headers: buildAuthHeaders(token),
  });

  return Array.isArray(data) ? data.map(mapTicketDto) : [];
}

function mapTicketDto(ticket) {
  return {
    id: ticket.id,
    status: ticket.status,
    reservedAt: ticket.reservedAt,
    eventId: ticket.eventId,
    eventName: ticket.eventName,
    eventDate: ticket.eventDate,
    venue: ticket.venue,
    imageUrl: ticket.imageUrl,
    sectorName: ticket.sectorName,
    sectorPrice: ticket.sectorPrice,
    rowIdentifier: ticket.rowIdentifier,
    seatNumber: ticket.seatNumber,
  };
}

async function handleJsonResponse(response) {
  const data = await response.json().catch(() => null);

  if (!response.ok) {
    throw createApiError(
      response,
      data,
      "No se pudieron cargar tus entradas. Verifica la API e intenta de nuevo.",
    );
  }

  return data;
}
