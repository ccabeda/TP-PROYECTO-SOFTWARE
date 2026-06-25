export class ApiError extends Error {
  constructor(message, status, data = null) {
    super(message);
    this.name = "ApiError";
    this.status = status;
    this.data = data;
  }
}

export function getValidationMessage(data) {
  if (!data?.errors || typeof data.errors !== "object") {
    return null;
  }

  const firstErrorGroup = Object.values(data.errors).find(
    (messages) => Array.isArray(messages) && messages.length > 0,
  );

  if (!firstErrorGroup) {
    return null;
  }

  return firstErrorGroup[0] ?? null;
}

export function createApiError(response, data, fallbackMessage) {
  const statusMessage = getStatusMessage(response.status);
  const errorMessage =
    getValidationMessage(data) ??
    data?.message ??
    data?.detail ??
    data?.title ??
    statusMessage ??
    fallbackMessage;

  return new ApiError(errorMessage, response.status, data);
}

function getStatusMessage(status) {
  const messages = {
    400: "La solicitud no es válida. Revisá los datos e intentá de nuevo.",
    401: "Tu sesión no es válida o expiró. Iniciá sesión nuevamente.",
    403: "No tenés permisos para realizar esta acción.",
    404: "No se encontró el recurso solicitado.",
    409: "La operación no se pudo completar porque el estado cambió.",
    500: "Ocurrió un error interno. Intentá nuevamente en unos minutos.",
  };

  return messages[status] ?? null;
}
