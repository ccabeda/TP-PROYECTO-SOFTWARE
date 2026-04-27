import { useEffect, useState } from "react";
import { getEventById } from "../services/eventsService";
import getErrorMessage from "../lib/getErrorMessage";

function useEvent(id) {
  const [event, setEvent] = useState(null);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState("");

  useEffect(() => {
    let isMounted = true;

    async function loadEvent() {
      if (!id) {
        if (isMounted) {
          setEvent(null);
          setError("");
          setIsLoading(false);
        }
        return;
      }

      if (isMounted) {
        setIsLoading(true);
        setError("");
      }

      try {
        const nextEvent = await getEventById(id);
        if (isMounted) {
          setEvent(nextEvent);
        }
      } catch (loadError) {
        if (isMounted) {
          setError(getErrorMessage(loadError, "No se pudo cargar el evento."));
          setEvent(null);
        }
      } finally {
        if (isMounted) {
          setIsLoading(false);
        }
      }
    }

    void loadEvent();

    return () => {
      isMounted = false;
    };
  }, [id]);

  return { event, isLoading, error };
}

export default useEvent;
