import { useCallback } from "react";
import { useQuery } from "@tanstack/react-query";
import getErrorMessage from "../lib/getErrorMessage";
import { getSeatsBySectorId } from "../services/eventsService";

function useSeats(sectorId) {
  const seatsQuery = useQuery({
    queryKey: ["seats", sectorId],
    queryFn: () => getSeatsBySectorId(sectorId),
    enabled: Boolean(sectorId),
  });

  const refreshSeats = useCallback(() => {
    void seatsQuery.refetch();
  }, [seatsQuery]);

  return {
    seats: sectorId ? seatsQuery.data ?? [] : [],
    isLoading: sectorId ? seatsQuery.isLoading : false,
    error: sectorId && seatsQuery.error
      ? getErrorMessage(seatsQuery.error, "No se pudieron cargar las butacas.")
      : "",
    refreshSeats,
  };
}

export default useSeats;
