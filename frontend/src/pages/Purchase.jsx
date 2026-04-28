import { useEffect, useMemo, useState } from "react";
import AppShell from "../components/layout/AppShell";
import useAuthContext from "../context/useAuthContext";
import useLanguageContext from "../context/useLanguageContext";
import useDocumentTitle from "../hooks/useDocumentTitle";
import useEvent from "../hooks/useEvent";
import useSectors from "../hooks/useSectors";
import useSeats from "../hooks/useSeats";
import { formatEventDate } from "../lib/eventFormat";
import getErrorMessage from "../lib/getErrorMessage";
import { useNavigate, useParams } from "react-router-dom";
import { t } from "../lib/i18n";
import { getSeatsBySectorId } from "../services/eventsService";
import { createReservation } from "../services/checkoutService";

const SERVICE_FEE_RATE = 0.12;
const SECTOR_SLOT_POSITIONS = [
  { position: "left-front", tone: "tone-1" },
  { position: "center-front", tone: "tone-2" },
  { position: "right-front", tone: "tone-3" },
  { position: "left-back", tone: "tone-4" },
  { position: "right-back", tone: "tone-5" },
];

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

function buildCheckoutState(selectedSector, selectedSeat, reservationId, serviceFee, finalPrice) {
  return {
    sectorId: selectedSector.id,
    sectorName: selectedSector.name,
    seatId: selectedSeat.id,
    reservationId,
    seatLabel: `${selectedSeat.rowIdentifier}${selectedSeat.seatNumber}`,
    basePrice: selectedSector.price,
    serviceFee,
    finalPrice,
  };
}

function Purchase() {
  const { id } = useParams();
  const navigate = useNavigate();
  const { session } = useAuthContext();
  const { language } = useLanguageContext();
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
  } = useSeats(resolvedSelectedSectorId);
  const selectedSector = useMemo(
    () =>
      visibleSectors.find((sector) => sector.id === resolvedSelectedSectorId) ?? null,
    [resolvedSelectedSectorId, visibleSectors]
  );
  const groupedSeats = useMemo(() => groupSeatsByRow(seats), [seats]);
  const sectorSlots = useMemo(() => buildSectorSlots(visibleSectors), [visibleSectors]);
  const selectedSeat = useMemo(
    () => seats.find((seat) => seat.id === selectedSeatId) ?? null,
    [seats, selectedSeatId]
  );
  const serviceFee = selectedSector ? Math.round(selectedSector.price * SERVICE_FEE_RATE) : 0;
  const finalPrice = selectedSector ? selectedSector.price + serviceFee : 0;

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
      const reservationId = selectedSeat.activeReservationId
        ? selectedSeat.activeReservationId
        : (
            await createReservation({
              seatId: selectedSeat.id,
              token: session.token,
            })
          ).id;

      navigate(checkoutPath, {
        state: buildCheckoutState(
          selectedSector,
          selectedSeat,
          reservationId,
          serviceFee,
          finalPrice,
        ),
      });
    } catch (reservationError) {
      setSeatWarning(
        getErrorMessage(reservationError, purchaseSelectSeatWarning),
      );
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
          <section className="purchase-card purchase-map-card">
            <h2>{purchaseMapTitle}</h2>
            <p className="event-detail-copy">{purchaseMapCopy}</p>

            {isLoadingSectors || isFilteringSectors ? (
              <p className="event-detail-copy">{detailSectorsLoading}</p>
            ) : sectorsError ? (
              <p className="event-detail-copy">{sectorsError}</p>
            ) : visibleSectors.length === 0 ? (
              <p className="event-detail-copy">{purchaseSectorsEmpty}</p>
            ) : (
              <div className="purchase-venue-map">
                <div className="purchase-venue-stage">{seatStageLabel}</div>
                <div className="purchase-venue-grid">
                  {sectorSlots.map((slot) => (
                    <button
                      type="button"
                      key={slot.key}
                      className={`purchase-venue-slot purchase-venue-slot-${slot.position} purchase-venue-slot-${slot.tone} ${
                        slot.sector ? "has-sector" : "is-empty"
                      } ${slot.sector?.id === resolvedSelectedSectorId ? "is-selected" : ""}`}
                      onMouseDown={(event) => event.preventDefault()}
                      onClick={() => {
                        if (!slot.sector) {
                          return;
                        }

                        setSelectedSectorId(slot.sector.id);
                        setSelectedSeatId(null);
                        setSeatWarning("");
                      }}
                      disabled={!slot.sector}
                    >
                      {slot.sector ? (
                        <>
                          <strong>{slot.sector.name}</strong>
                          <span>{formatPrice(slot.sector.price)}</span>
                        </>
                      ) : null}
                    </button>
                  ))}
                </div>
              </div>
            )}
          </section>

          <aside className="purchase-card purchase-summary-card">
            <h2>{purchaseSummaryTitle}</h2>
            <p className="event-detail-copy">{purchaseSummaryCopy}</p>

            {selectedSector ? (
              <div className="purchase-summary-box">
                <span className="purchase-seat-map-label">{selectedSector.name}</span>

                <div className="purchase-summary-lines">
                  <div className="purchase-summary-line">
                    <span>{purchaseBasePriceLabel}</span>
                    <strong>{formatPrice(selectedSector.price)}</strong>
                  </div>
                  <div className="purchase-summary-line">
                    <span>{purchaseFeeLabel}</span>
                    <strong>{formatPrice(serviceFee)}</strong>
                  </div>
                  <div className="purchase-summary-line purchase-summary-line-total">
                    <span>{purchaseFinalPriceLabel}</span>
                    <strong>{formatPrice(finalPrice)}</strong>
                  </div>
                </div>
              </div>
            ) : (
              <p className="event-detail-copy">{purchaseSelectSectorFirst}</p>
            )}
          </aside>
        </section>

        <section className="purchase-card purchase-seats-panel">
          <h2>{purchaseSeatsTitle}</h2>
          <p className="event-detail-copy">{purchaseSeatsCopy}</p>

          {!resolvedSelectedSectorId ? (
            <p className="event-detail-copy">{purchaseSelectSectorFirst}</p>
          ) : seatsError ? (
            <p className="event-detail-copy">{seatsError}</p>
          ) : !isLoadingSeats && seats.length === 0 ? (
            <p className="event-detail-copy">{purchaseSeatsEmpty}</p>
          ) : (
            <div className={`purchase-seat-map-shell ${isLoadingSeats ? "is-loading" : ""}`}>
              {selectedSector ? (
                <div className="purchase-seat-map-header">
                  <div>
                    <span className="purchase-seat-map-label">{selectedSector.name}</span>
                  </div>
                </div>
              ) : null}

              {isLoadingSeats ? (
                <p className="purchase-seat-loading">{purchaseSeatsLoading}</p>
              ) : null}

              <div className="seat-legend">
                <span className="seat-legend-item">
                  <i className="seat-legend-swatch seat-legend-available" />
                  {purchaseLegendAvailable}
                </span>
                <span className="seat-legend-item">
                  <i className="seat-legend-swatch seat-legend-mine" />
                  {purchaseLegendMine}
                </span>
                <span className="seat-legend-item">
                  <i className="seat-legend-swatch seat-legend-reserved" />
                  {purchaseLegendReserved}
                </span>
                <span className="seat-legend-item">
                  <i className="seat-legend-swatch seat-legend-sold" />
                  {purchaseLegendSold}
                </span>
              </div>

              <div className="seat-map seat-map-inline">
                {Object.entries(groupedSeats).map(([row, rowSeats]) => (
                  <div className="seat-row" key={row}>
                    <div className="seat-row-grid">
                      {rowSeats.map((seat) => (
                        <button
                          type="button"
                          key={seat.id}
                          className={`seat-chip seat-chip-${getSeatVisualStatus(seat)} ${
                            seat.id === selectedSeatId ? "is-selected" : ""
                          }`}
                          disabled={!isSeatSelectable(seat)}
                          title={`${row}${seat.seatNumber} - ${getSeatStatusLabel(seat.status, language)}`}
                          onClick={() => {
                            setSelectedSeatId(seat.id);
                            setSeatWarning("");
                          }}
                        >
                          {row}{seat.seatNumber}
                        </button>
                      ))}
                    </div>
                  </div>
                ))}
              </div>

              <div className="purchase-seat-actions">
                {seatWarning ? (
                  <p className="purchase-seat-warning">{seatWarning}</p>
                ) : null}
                <button
                  type="button"
                  className="button button-primary purchase-continue-button"
                  onClick={handleContinue}
                  disabled={isReservingSeat}
                >
                  {isReservingSeat ? purchaseReservingButton : purchaseContinueButton}
                </button>
              </div>
            </div>
          )}
        </section>
      </section>
    </AppShell>
  );
}

export default Purchase;

function groupSeatsByRow(seats) {
  return seats.reduce((groups, seat) => {
    const row = seat.rowIdentifier || "?";

    if (!groups[row]) {
      groups[row] = [];
    }

    groups[row].push(seat);
    return groups;
  }, {});
}

function normalizeSeatStatus(status) {
  const normalized = status?.trim().toLowerCase();

  if (normalized === "reserved") {
    return "reserved";
  }

  if (normalized === "sold") {
    return "sold";
  }

  return "available";
}

function getSeatStatusLabel(status, language) {
  const normalized = normalizeSeatStatus(status);

  if (normalized === "reserved") {
    return t(language, "home.seatReserved");
  }

  if (normalized === "sold") {
    return t(language, "home.seatSold");
  }

  return t(language, "home.seatAvailable");
}

function isSeatSelectable(seat) {
  const normalized = normalizeSeatStatus(seat.status);
  return normalized === "available" || (normalized === "reserved" && seat.reservedByCurrentUser);
}

function getSeatVisualStatus(seat) {
  const normalized = normalizeSeatStatus(seat.status);

  if (normalized === "reserved" && seat.reservedByCurrentUser) {
    return "mine";
  }

  return normalized;
}

function buildSectorSlots(sectors) {
  return SECTOR_SLOT_POSITIONS.map(({ position, tone }, index) => ({
    key: position,
    position,
    tone,
    sector: sectors[index] ?? null,
  }));
}
