import { useEffect, useState } from "react";
import getErrorMessage from "../lib/getErrorMessage";
import { getSeatsBySectorId } from "../services/eventsService";

function useSeats(sectorId) {
  const [seats, setSeats] = useState([]);
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState("");

  useEffect(() => {
    let isMounted = true;

    if (!sectorId) {
      if (isMounted) {
        setSeats([]);
        setError("");
        setIsLoading(false);
      }
      return;
    }

    async function loadSeats() {
      if (isMounted) {
        setIsLoading(true);
        setError("");
      }

      try {
        const nextSeats = await getSeatsBySectorId(sectorId);
        if (isMounted) {
          setSeats(nextSeats);
        }
      } catch (loadError) {
        if (isMounted) {
          setError(getErrorMessage(loadError, "No se pudieron cargar las butacas."));
          setSeats([]);
        }
      } finally {
        if (isMounted) {
          setIsLoading(false);
        }
      }
    }

    void loadSeats();

    return () => {
      isMounted = false;
    };
  }, [sectorId]);

  return { seats, isLoading, error };
}

export default useSeats;
