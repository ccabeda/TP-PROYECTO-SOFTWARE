import { useEffect, useState } from "react";
import { getEvents } from "../services/eventsService";
import getErrorMessage from "../lib/getErrorMessage";

function useEvents(filters = {}) {
  const { eventDate = "", name = "", page = 1, pageSize = 12 } = filters;
  const [events, setEvents] = useState([]);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState("");
  const [pagination, setPagination] = useState({
    totalCount: 0,
    page: 1,
    pageSize,
    totalPages: 0,
  });

  useEffect(() => {
    let isMounted = true;

    async function loadEvents() {
      if (isMounted) {
        setIsLoading(true);
        setError("");
      }

      try {
        const result = await getEvents({ eventDate, name, page, pageSize });
        if (isMounted) {
          setEvents(result.items);
          setPagination({
            totalCount: result.totalCount,
            page: result.page,
            pageSize: result.pageSize,
            totalPages: result.totalPages,
          });
        }
      } catch (loadError) {
        if (isMounted) {
          setError(
            getErrorMessage(loadError, "No se pudieron cargar los eventos."),
          );
          setEvents([]);
          setPagination({
            totalCount: 0,
            page,
            pageSize,
            totalPages: 0,
          });
        }
      } finally {
        if (isMounted) {
          setIsLoading(false);
        }
      }
    }

    void loadEvents();

    return () => {
      isMounted = false;
    };
  }, [eventDate, name, page, pageSize]);

  return { events, isLoading, error, ...pagination };
}

export default useEvents;
