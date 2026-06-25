import { useQuery } from "@tanstack/react-query";
import { getEventById } from "../services/eventsService";
import getErrorMessage from "../lib/getErrorMessage";

function useEvent(id) {
  const eventQuery = useQuery({
    queryKey: ["event", id],
    queryFn: () => getEventById(id),
    enabled: Boolean(id),
  });

  return {
    event: eventQuery.data ?? null,
    isLoading: id ? eventQuery.isLoading : false,
    error: eventQuery.error
      ? getErrorMessage(eventQuery.error, "No se pudo cargar el evento.")
      : "",
  };
}

export default useEvent;
