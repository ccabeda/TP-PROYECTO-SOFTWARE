import { useEffect, useMemo, useState } from "react";
import AppShell from "../components/layout/AppShell";
import PurchaseSummary from "../components/purchase/PurchaseSummary";
import PurchaseVenueMap from "../components/purchase/PurchaseVenueMap";
import SeatSelectionPanel from "../components/purchase/SeatSelectionPanel";
import useAuthContext from "../context/useAuthContext";
import useLanguageContext from "../context/useLanguageContext";
import useToastContext from "../context/useToastContext";
import useDocumentTitle from "../hooks/useDocumentTitle";
import useEvent from "../hooks/useEvent";
import useSectors from "../hooks/useSectors";
import useSeats from "../hooks/useSeats";
import { formatEventDate } from "../lib/eventFormat";
import getErrorMessage from "../lib/getErrorMessage";
import { useNavigate, useParams } from "react-router-dom";
import { t } from "../lib/i18n";
import { getSeatsBySectorId } from "../services/eventsService";
import { createReservation, getReservationById } from "../services/checkoutService";

const SERVICE_FEE_RATE = 0.12;
const SEAT_MAP_REFRESH_INTERVAL_MS = 10_000;

function getPriceLocale(language) {
  return language === "en" ? "en-US" : "es-AR";
}

function getPurchasePath(eventId) {
  return `/event/${eventId}/purchase`;
}

function getCheckoutPath(eventId) {
  return `/event/${eventId}/checkout`;
}

function getEventTitle(event) {
  return event?.titulo || event?.title || event?.name || "";
}

async function loadSectorSeatLists(sectors) {
  return Promise.all(
    sectors.map(async (sector) => ({
      sectorId: sector.id,
      seats: await getSeatsBySectorId(sector.id),
    })),
  );
}

function getAvailableSectorIdSet(seatLists) {
  return new Set(
    seatLists.filter((item) => item.seats.length > 0).map((item) => item.sectorId),
  );
}

function buildCheckoutState(selectedSector, selectedSeat, reservationId, expiresAt, serviceFee, finalPrice) {
  return {
    sectorId: selectedSector.id,
    sectorName: selectedSector.name,
    seatId: selectedSeat.id,
    reservationId,
    expiresAt,
    seatLabel: `${selectedSeat.rowIdentifier}${selectedSeat.seatNumber}`,
    basePrice: selectedSector.price,
    serviceFee,
    finalPrice,
  };
}

function getSelectedSeatLabel(seat) {
  if (!seat) {
    return "";
  }

  return `${seat.rowIdentifier}${seat.seatNumber}`;
}

function Purchase() {
  const { id } = useParams();
  const navigate = useNavigate();
  const { session } = useAuthContext();
  const { language } = useLanguageContext();
  const { showToast } = useToastContext();
  const purchasePath = getPurchasePath(id);
  const checkoutPath = getCheckoutPath(id);
  const [selectedSectorId, setSelectedSectorId] = useState(null);
  const [selectedSeatId, setSelectedSeatId] = useState(null);
  const [seatWarning, setSeatWarning] = useState("");
  const [availableSectorIds, setAvailableSectorIds] = useState(null);
  const [isFilteringSectors, setIsFilteringSectors] = useState(false);
  const [isReservingSeat, setIsReservingSeat] = useState(false);
  const { event, isLoading, error } = useEvent(id);
  const {
    sectors,
    isLoading: isLoadingSectors,
    error: sectorsError,
  } = useSectors(id);
  const priceLocale = getPriceLocale(language);
  const eventTitle = getEventTitle(event);
  const loadingLabel = t(language, "home.eventLoading");
  const notFoundLabel = t(language, "home.eventNotFound");
  const buyLabel = t(language, "home.buy");
  const purchaseMapTitle = t(language, "home.purchaseMapTitle");
  const purchaseMapCopy = t(language, "home.purchaseMapCopy");
  const purchaseSummaryTitle = t(language, "home.purchaseSummaryTitle");
  const purchaseSummaryCopy = t(language, "home.purchaseSummaryCopy");
  const purchaseSectorsEmpty = t(language, "home.purchaseSectorsEmpty");
  const detailSectorsLoading = t(language, "home.detailSectorsLoading");
  const purchaseSelectSectorFirst = t(language, "home.purchaseSelectSectorFirst");
  const purchaseBasePriceLabel = t(language, "home.purchaseBasePriceLabel");
  const purchaseFeeLabel = t(language, "home.purchaseFeeLabel");
  const purchaseFinalPriceLabel = t(language, "home.purchaseFinalPriceLabel");
  const purchaseSelectedSeatLabel = t(language, "home.purchaseSelectedSeatLabel");
  const purchaseSelectedSeatEmpty = t(language, "home.purchaseSelectedSeatEmpty");
  const purchaseSelectedSeatReady = t(language, "home.purchaseSelectedSeatReady");
  const purchaseSelectedSeatMine = t(language, "home.purchaseSelectedSeatMine");
  const purchaseSeatsTitle = t(language, "home.purchaseSeatsTitle");
  const purchaseSeatsCopy = t(language, "home.purchaseSeatsCopy");
  const purchaseSeatsEmpty = t(language, "home.purchaseSeatsEmpty");
  const purchaseSeatsLoading = t(language, "home.purchaseSeatsLoading");
  const purchaseLegendAvailable = t(language, "home.purchaseLegendAvailable");
  const purchaseLegendMine = t(language, "home.purchaseLegendMine");
  const purchaseLegendReserved = t(language, "home.purchaseLegendReserved");
  const purchaseLegendSold = t(language, "home.purchaseLegendSold");
  const purchaseContinueButton = t(language, "home.purchaseContinueButton");
  const purchaseReservingButton = t(language, "home.purchaseReservingButton");
  const purchaseSelectSeatWarning = t(language, "home.purchaseSelectSeatWarning");
  const seatStageLabel = t(language, "home.seatStage");
  const formatPrice = (value) => `$${value.toLocaleString(priceLocale)}`;
  useDocumentTitle(eventTitle || purchaseMapTitle);

  const visibleSectors = useMemo(() => {
    if (!availableSectorIds) {
      return sectors;
    }

    return sectors.filter((sector) => availableSectorIds.has(sector.id));
  }, [availableSectorIds, sectors]);
  const resolvedSelectedSectorId = useMemo(() => {
    if (visibleSectors.length === 0) {
      return null;
    }

    if (
      selectedSectorId &&
      visibleSectors.some((sector) => sector.id === selectedSectorId)
    ) {
      return selectedSectorId;
    }

    return visibleSectors[0]?.id ?? null;
  }, [selectedSectorId, visibleSectors]);
  const {
    seats,
    isLoading: isLoadingSeats,
    error: seatsError,
    refreshSeats,
  } = useSeats(resolvedSelectedSectorId);
  const selectedSector = useMemo(
    () =>
      visibleSectors.find((sector) => sector.id === resolvedSelectedSectorId) ?? null,
    [resolvedSelectedSectorId, visibleSectors]
  );
  const selectedSeat = useMemo(
    () => seats.find((seat) => seat.id === selectedSeatId) ?? null,
    [seats, selectedSeatId]
  );
  const selectedSeatLabel = getSelectedSeatLabel(selectedSeat);
  const selectedSeatStateLabel = selectedSeat?.reservedByCurrentUser
    ? purchaseSelectedSeatMine
    : purchaseSelectedSeatReady;
  const serviceFee = selectedSector ? Math.round(selectedSector.price * SERVICE_FEE_RATE) : 0;
  const finalPrice = selectedSector ? selectedSector.price + serviceFee : 0;
  const summaryLabels = {
    selectedSeat: purchaseSelectedSeatLabel,
    selectedSeatEmpty: purchaseSelectedSeatEmpty,
    basePrice: purchaseBasePriceLabel,
    fee: purchaseFeeLabel,
    finalPrice: purchaseFinalPriceLabel,
    selectSectorFirst: purchaseSelectSectorFirst,
  };
  const seatPanelLabels = {
    selectSectorFirst: purchaseSelectSectorFirst,
    seatsEmpty: purchaseSeatsEmpty,
    seatsLoading: purchaseSeatsLoading,
    legendAvailable: purchaseLegendAvailable,
    legendMine: purchaseLegendMine,
    legendReserved: purchaseLegendReserved,
    legendSold: purchaseLegendSold,
    continueButton: purchaseContinueButton,
    reservingButton: purchaseReservingButton,
  };

  useEffect(() => {
    if (sectors.length === 0) {
      return;
    }

    let isCancelled = false;

    async function filterSectorsWithSeats() {
      setIsFilteringSectors(true);

      try {
        const seatLists = await loadSectorSeatLists(sectors);

        if (isCancelled) {
          return;
        }

        setAvailableSectorIds(getAvailableSectorIdSet(seatLists));
      } catch {
        if (!isCancelled) {
          setAvailableSectorIds(new Set(sectors.map((sector) => sector.id)));
        }
      } finally {
        if (!isCancelled) {
          setIsFilteringSectors(false);
        }
      }
    }

    void filterSectorsWithSeats();

    return () => {
      isCancelled = true;
    };
  }, [sectors]);

  useEffect(() => {
    if (!resolvedSelectedSectorId || isLoadingSeats) {
      return undefined;
    }

    const intervalId = window.setInterval(() => {
      refreshSeats();
    }, SEAT_MAP_REFRESH_INTERVAL_MS);

    return () => {
      window.clearInterval(intervalId);
    };
  }, [isLoadingSeats, refreshSeats, resolvedSelectedSectorId]);

  if (isLoading) {
    return (
      <AppShell>
        <p className="events-feedback">{loadingLabel}</p>
      </AppShell>
    );
  }

  if (error || !event) {
    return (
      <AppShell>
        <p className="events-feedback">{error || notFoundLabel}</p>
      </AppShell>
    );
  }

  async function handleContinue() {
    if (!selectedSector || !selectedSeat) {
      setSeatWarning(purchaseSelectSeatWarning);
      return;
    }

    if (!session?.token) {
      navigate("/login", { replace: true, state: { redirectTo: purchasePath } });
      return;
    }

    setSeatWarning("");
    setIsReservingSeat(true);

    try {
      const reservation = selectedSeat.activeReservationId
        ? await getReservationById({
            reservationId: selectedSeat.activeReservationId,
            token: session.token,
          })
        : await createReservation({
            seatId: selectedSeat.id,
            token: session.token,
          });

      navigate(checkoutPath, {
        state: buildCheckoutState(
          selectedSector,
          selectedSeat,
          reservation.id,
          reservation.expiresAt ?? null,
          serviceFee,
          finalPrice,
        ),
      });
    } catch (reservationError) {
      const errorMessage = getErrorMessage(
        reservationError,
        purchaseSelectSeatWarning,
      );

      if (reservationError.status === 409) {
        setSelectedSeatId(null);
        refreshSeats();
      }

      setSeatWarning(errorMessage);
      showToast(errorMessage, "error");
    } finally {
      setIsReservingSeat(false);
    }
  }

  return (
    <AppShell>
      <section className="purchase-page">
        <div className="purchase-header">
          <div className="purchase-header-badge-row">
            <p className="header-badge purchase-header-badge">{buyLabel}</p>
          </div>
          <h1>{eventTitle}</h1>
          <div className="purchase-header-meta">
            <span className="purchase-header-date">{formatEventDate(event.eventDate, language)}</span>
          </div>
        </div>

        <section className="purchase-top-layout">
          <PurchaseVenueMap
            title={purchaseMapTitle}
            copy={purchaseMapCopy}
            isLoading={isLoadingSectors || isFilteringSectors ? detailSectorsLoading : ""}
            error={sectorsError}
            emptyMessage={purchaseSectorsEmpty}
            sectors={visibleSectors}
            selectedSectorId={resolvedSelectedSectorId}
            stageLabel={seatStageLabel}
            formatPrice={formatPrice}
            onSelectSector={(sectorId) => {
              setSelectedSectorId(sectorId);
              setSelectedSeatId(null);
              setSeatWarning("");
            }}
          />

          <PurchaseSummary
            title={purchaseSummaryTitle}
            copy={purchaseSummaryCopy}
            selectedSector={selectedSector}
            selectedSeat={selectedSeat}
            selectedSeatLabel={selectedSeatLabel}
            selectedSeatStateLabel={selectedSeatStateLabel}
            labels={summaryLabels}
            serviceFee={serviceFee}
            finalPrice={finalPrice}
            formatPrice={formatPrice}
          />
        </section>

        <SeatSelectionPanel
          title={purchaseSeatsTitle}
          copy={purchaseSeatsCopy}
          selectedSector={selectedSector}
          selectedSeat={selectedSeat}
          selectedSeatId={selectedSeatId}
          selectedSeatLabel={selectedSeatLabel}
          selectedSeatStateLabel={selectedSeatStateLabel}
          seats={seats}
          seatsError={seatsError}
          isLoadingSeats={isLoadingSeats}
          language={language}
          labels={seatPanelLabels}
          seatWarning={seatWarning}
          isReservingSeat={isReservingSeat}
          onSelectSeat={(seatId) => {
            setSelectedSeatId(seatId);
            setSeatWarning("");
          }}
          onContinue={handleContinue}
        />
      </section>
    </AppShell>
  );
}

export default Purchase;
