import { useQuery } from "@tanstack/react-query";
import { getEvents } from "../services/eventsService";
import getErrorMessage from "../lib/getErrorMessage";

function useEvents(filters = {}) {
  const { eventDate = "", name = "", page = 1, pageSize = 12 } = filters;
  const eventsQuery = useQuery({
    queryKey: ["events", { eventDate, name, page, pageSize }],
    queryFn: () => getEvents({ eventDate, name, page, pageSize }),
  });
  const result = eventsQuery.data;

  return {
    events: result?.items ?? [],
    isLoading: eventsQuery.isLoading,
    error: eventsQuery.error
      ? getErrorMessage(eventsQuery.error, "No se pudieron cargar los eventos.")
      : "",
    totalCount: result?.totalCount ?? 0,
    page: result?.page ?? page,
    pageSize: result?.pageSize ?? pageSize,
    totalPages: result?.totalPages ?? 0,
  };
}

export default useEvents;
