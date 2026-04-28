import { API_BASE_URL } from "../lib/api";

const JSON_HEADERS = {
  "Content-Type": "application/json",
};

function buildAuthJsonHeaders(token) {
  return {
    ...JSON_HEADERS,
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

async function deleteWithAuth(path, token) {
  const response = await fetch(`${API_BASE_URL}${path}`, {
    method: "DELETE",
    headers: buildAuthJsonHeaders(token),
  });

  if (!response.ok) {
    throw new Error("No se pudo revertir la creación administrativa.");
  }
}

export async function createAdminEvent(eventData, token) {
  return postJson("/events", eventData, token);
}

export async function createAdminSector(eventId, sectorData, token) {
  return postJson(`/events/${eventId}/sectors`, sectorData, token);
}

export async function createAdminSeatsBulk(sectorId, seatBulkData, token) {
  return postJson(`/sectors/${sectorId}/seats/bulk`, seatBulkData, token);
}

export async function deleteAdminEvent(eventId, token) {
  return deleteWithAuth(`/events/${eventId}`, token);
}

export async function getAdminUsers(token) {
  const response = await fetch(`${API_BASE_URL}/users`, {
    method: "GET",
    headers: buildAuthJsonHeaders(token),
  });

  return handleJsonResponse(response);
}

export async function getAdminUsersPage(filters, token) {
  const query = new URLSearchParams();

  if (filters?.name) {
    query.set("name", filters.name);
  }

  if (filters?.email) {
    query.set("email", filters.email);
  }

  if (filters?.page) {
    query.set("page", String(filters.page));
  }

  if (filters?.pageSize) {
    query.set("pageSize", String(filters.pageSize));
  }

  const path = query.size > 0 ? `/users?${query.toString()}` : "/users";
  const response = await fetch(`${API_BASE_URL}${path}`, {
    method: "GET",
    headers: buildAuthJsonHeaders(token),
  });

  return handleJsonResponse(response);
}

export async function getAdminAuditLogs(filters, token) {
  const query = new URLSearchParams();

  if (filters?.search) {
    query.set("search", filters.search);
  }

  if (filters?.dateFrom) {
    query.set("dateFrom", filters.dateFrom);
  }

  if (filters?.dateTo) {
    query.set("dateTo", filters.dateTo);
  }

  if (filters?.page) {
    query.set("page", String(filters.page));
  }

  if (filters?.pageSize) {
    query.set("pageSize", String(filters.pageSize));
  }

  const path = query.size > 0 ? `/auditlogs?${query.toString()}` : "/auditlogs";
  const response = await fetch(`${API_BASE_URL}${path}`, {
    method: "GET",
    headers: buildAuthJsonHeaders(token),
  });

  return handleJsonResponse(response);
}

export async function createAdminEventBundle(bundleData, token) {
  if (!Array.isArray(bundleData?.sectors) || bundleData.sectors.length === 0) {
    throw new Error("Debes cargar al menos un sector con sus butacas.");
  }

  let createdEvent = null;
  const createdSectors = [];

  try {
    createdEvent = await createAdminEvent(
      {
        name: bundleData.name,
        eventDate: bundleData.eventDate,
        venue: bundleData.venue,
        status: bundleData.status,
        imageUrl: bundleData.imageUrl,
        description: bundleData.description,
      },
      token,
    );

    for (const sector of bundleData.sectors) {
      const createdSector = await createAdminSector(
        createdEvent.id,
        {
          name: sector.name,
          price: sector.price,
          capacity: sector.capacity,
        },
        token,
      );

      await createAdminSeatsBulk(
        createdSector.id,
        {
          rowCount: sector.rowCount,
          seatsPerRow: sector.seatsPerRow,
        },
        token,
      );

      createdSectors.push(createdSector);
    }

    return {
      event: createdEvent,
      sectors: createdSectors,
    };
  } catch (error) {
    if (createdEvent?.id) {
      try {
        await deleteAdminEvent(createdEvent.id, token);
      } catch {
        throw new Error(
          `${error instanceof Error ? error.message : "No se pudo completar la operación administrativa."} Además, no se pudo revertir automáticamente el evento creado.`,
        );
      }
    }

    throw error;
  }
}

async function handleJsonResponse(response) {
  const data = await response.json().catch(() => null);

  if (!response.ok) {
    const validationMessage = getValidationMessage(data);

    throw new Error(
      data?.message ??
        validationMessage ??
        data?.detail ??
        data?.title ??
        "No se pudo completar la operación administrativa. Verifica la API e intenta de nuevo.",
    );
  }

  return data;
}

function getValidationMessage(data) {
  if (!data?.errors || typeof data.errors !== "object") {
    return null;
  }

  const firstErrorList = Object.values(data.errors).find(
    (value) => Array.isArray(value) && value.length > 0,
  );

  return Array.isArray(firstErrorList) ? firstErrorList[0] : null;
}
