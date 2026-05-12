import { useCallback, useEffect, useState } from "react";
import getErrorMessage from "../lib/getErrorMessage";
import { getSeatsBySectorId } from "../services/eventsService";

function useSeats(sectorId) {
  const [seats, setSeats] = useState([]);
  const [isLoading, setIsLoading] = useState(Boolean(sectorId));
  const [error, setError] = useState("");
  const [refreshIndex, setRefreshIndex] = useState(0);

  useEffect(() => {
    if (!sectorId) {
      return undefined;
    }

    let isMounted = true;

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
  }, [refreshIndex, sectorId]);

  const refreshSeats = useCallback(() => {
    setRefreshIndex((current) => current + 1);
  }, []);

  return {
    seats: sectorId ? seats : [],
    isLoading: sectorId ? isLoading : false,
    error: sectorId ? error : "",
    refreshSeats,
  };
}

export default useSeats;
