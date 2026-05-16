import { API_BASE_URL } from "../lib/api";
import { getToken } from "./authService";

function buildEventQuery(filters = {}) {
  const query = new URLSearchParams();

  if (filters.name?.trim()) {
    query.set("name", filters.name.trim());
  }

  if (filters.eventDate) {
    query.set("eventDate", filters.eventDate);
  }

  if (Number.isInteger(filters.page) && filters.page > 0) {
    query.set("page", String(filters.page));
  }

  if (Number.isInteger(filters.pageSize) && filters.pageSize > 0) {
    query.set("pageSize", String(filters.pageSize));
  }

  return query.toString() ? `?${query}` : "";
}

function buildAuthHeaders(token) {
  return token
    ? {
        Authorization: `Bearer ${token}`,
      }
    : undefined;
}

function sortEventsByDate(left, right) {
  return new Date(left.eventDate) - new Date(right.eventDate);
}

function sortSectorsByPrice(left, right) {
  return left.price - right.price;
}

function sortSeatsByRowAndNumber(left, right) {
  if (left.rowIdentifier === right.rowIdentifier) {
    return left.seatNumber - right.seatNumber;
  }

  return left.rowIdentifier.localeCompare(right.rowIdentifier);
}

async function fetchJson(path, options) {
  const response = await fetch(`${API_BASE_URL}${path}`, options);
  return handleJsonResponse(response);
}

export async function getEvents(filters = {}) {
  const data = await fetchJson(`/events${buildEventQuery(filters)}`);
  const items = Array.isArray(data) ? data : data?.items ?? [];
  const sortedItems = items.map(mapEventDto).sort(sortEventsByDate);

  if (Array.isArray(data)) {
    return {
      items: sortedItems,
      totalCount: sortedItems.length,
      page: 1,
      pageSize: sortedItems.length,
      totalPages: sortedItems.length > 0 ? 1 : 0,
    };
  }

  return {
    items: sortedItems,
    totalCount: data?.totalCount ?? sortedItems.length,
    page: data?.page ?? 1,
    pageSize: data?.pageSize ?? sortedItems.length,
    totalPages: data?.totalPages ?? (sortedItems.length > 0 ? 1 : 0),
  };
}

export async function getEventById(id) {
  if (!id) {
    throw new Error("No se pudo cargar el evento porque falta su identificador.");
  }

  const data = await fetchJson(`/events/${id}`);

  return mapEventDto(data);
}

export async function getSectorsByEventId(eventId) {
  if (!eventId) {
    throw new Error("No se pudieron cargar los sectores porque falta el evento.");
  }

  const data = await fetchJson(`/events/${eventId}/sectors`);

  return data.map(mapSectorDto).sort(sortSectorsByPrice);
}

export async function getSeatsBySectorId(sectorId) {
  if (!sectorId) {
    throw new Error("No se pudieron cargar las butacas porque falta el sector.");
  }

  const token = getToken();
  const data = await fetchJson(`/sectors/${sectorId}/seats`, {
    headers: buildAuthHeaders(token),
  });

  return data.map(mapSeatDto).sort(sortSeatsByRowAndNumber);
}

function mapEventDto(eventDto) {
  return {
    id: eventDto.id,
    titulo: eventDto.name ?? "Evento sin nombre",
    fecha: eventDto.eventDate,
    eventDate: eventDto.eventDate,
    venue: eventDto.venue ?? "",
    status: eventDto.status ?? "",
    imageUrl: eventDto.imageUrl ?? null,
    description: eventDto.description ?? "",
  };
}

function mapSectorDto(sectorDto) {
  return {
    id: sectorDto.id,
    eventId: sectorDto.eventId,
    name: sectorDto.name ?? "Sector",
    price: sectorDto.price ?? 0,
    capacity: sectorDto.capacity ?? 0,
  };
}

function mapSeatDto(seatDto) {
  return {
    id: seatDto.id,
    sectorId: seatDto.sectorId,
    rowIdentifier: seatDto.rowIdentifier ?? "",
    seatNumber: seatDto.seatNumber ?? 0,
    status: seatDto.status ?? "",
    reservedByCurrentUser: seatDto.reservedByCurrentUser ?? false,
    activeReservationId: seatDto.activeReservationId ?? null,
  };
}

async function handleJsonResponse(response) {
  const data = await response.json().catch(() => null);

  if (!response.ok) {
    throw new Error(
      data?.message ??
        "No se pudieron cargar los eventos. Verifica el backend e intenta de nuevo.",
    );
  }

  return data;
}
