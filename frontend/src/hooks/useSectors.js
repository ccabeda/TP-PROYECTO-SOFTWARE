import { useQuery } from "@tanstack/react-query";
import getErrorMessage from "../lib/getErrorMessage";
import { getSectorsByEventId } from "../services/eventsService";

function useSectors(eventId) {
  const sectorsQuery = useQuery({
    queryKey: ["sectors", eventId],
    queryFn: () => getSectorsByEventId(eventId),
    enabled: Boolean(eventId),
  });

  return {
    sectors: eventId ? sectorsQuery.data ?? [] : [],
    isLoading: eventId ? sectorsQuery.isLoading : false,
    error: eventId && sectorsQuery.error
      ? getErrorMessage(sectorsQuery.error, "No se pudieron cargar los sectores.")
      : "",
  };
}

export default useSectors;
