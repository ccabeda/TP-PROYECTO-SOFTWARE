import { useEffect, useState } from "react";
import getErrorMessage from "../lib/getErrorMessage";
import { getSectorsByEventId } from "../services/eventsService";

function useSectors(eventId) {
  const [sectors, setSectors] = useState([]);
  const [isLoading, setIsLoading] = useState(Boolean(eventId));
  const [error, setError] = useState("");

  useEffect(() => {
    if (!eventId) {
      return undefined;
    }

    let isMounted = true;

    async function loadSectors() {
      if (isMounted) {
        setIsLoading(true);
        setError("");
      }

      try {
        const nextSectors = await getSectorsByEventId(eventId);
        if (isMounted) {
          setSectors(nextSectors);
        }
      } catch (loadError) {
        if (isMounted) {
          setError(
            getErrorMessage(loadError, "No se pudieron cargar los sectores."),
          );
          setSectors([]);
        }
      } finally {
        if (isMounted) {
          setIsLoading(false);
        }
      }
    }

    void loadSectors();

    return () => {
      isMounted = false;
    };
  }, [eventId]);

  return {
    sectors: eventId ? sectors : [],
    isLoading: eventId ? isLoading : false,
    error: eventId ? error : "",
  };
}

export default useSectors;
