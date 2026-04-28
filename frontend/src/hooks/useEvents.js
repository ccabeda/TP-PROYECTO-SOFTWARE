import { useEffect, useState } from "react";
import { getEvents } from "../services/eventsService";
import getErrorMessage from "../lib/getErrorMessage";

function useEvents(filters = {}) {
  const { eventDate = "", name = "" } = filters;
  const [events, setEvents] = useState([]);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState("");

  useEffect(() => {
    let isMounted = true;

    async function loadEvents() {
      if (isMounted) {
        setIsLoading(true);
        setError("");
      }

      try {
        const nextEvents = await getEvents({ eventDate, name });
        if (isMounted) {
          setEvents(nextEvents);
        }
      } catch (loadError) {
        if (isMounted) {
          setError(
            getErrorMessage(loadError, "No se pudieron cargar los eventos."),
          );
          setEvents([]);
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
  }, [eventDate, name]);

  return { events, isLoading, error };
}

export default useEvents;
