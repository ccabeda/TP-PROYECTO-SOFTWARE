import AppShell from "../components/layout/AppShell";
import { useAuthContext } from "../context/AuthContext";
import { useLanguageContext } from "../context/LanguageContext";
import useDocumentTitle from "../hooks/useDocumentTitle";
import useEvent from "../hooks/useEvent";
import useSectors from "../hooks/useSectors";
import { formatEventDate, formatEventStatus } from "../lib/eventFormat";
import { useNavigate, useParams } from "react-router-dom";
import { t } from "../lib/i18n";
import { getSeatsBySectorId } from "../services/eventsService";
import { useEffect, useMemo, useState } from "react";

function getPriceLocale(language) {
  return language === "en" ? "en-US" : "es-AR";
}

function getEventTitle(event) {
  return event?.titulo || event?.title || event?.name || "";
}

function getEventImageUrl(event) {
  return event?.imagen || event?.imageUrl || "";
}

function getEventDescription(event, fallbackDescription) {
  return event?.description || fallbackDescription;
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

function hasCurrentUserReservation(seatLists) {
  return seatLists.some((item) =>
    item.seats.some((seat) => seat.reservedByCurrentUser),
  );
}

function Evento() {
  const { id } = useParams();
  const navigate = useNavigate();
  const { session } = useAuthContext();
  const { language } = useLanguageContext();
  const { event, isLoading, error } = useEvent(id);
  const { sectors, isLoading: isLoadingSectors, error: sectorsError } = useSectors(id);
  const [availableSectorIds, setAvailableSectorIds] = useState(null);
  const [isFilteringSectors, setIsFilteringSectors] = useState(false);
  const [hasOwnReservation, setHasOwnReservation] = useState(false);
  const purchasePath = `/event/${id}/purchase`;
  const priceLocale = getPriceLocale(language);
  const loadingLabel = t(language, "home.eventLoading");
  const notFoundLabel = t(language, "home.eventNotFound");
  const detailInfoCopy = t(language, "home.detailInfoCopy");
  const detailCopy = t(language, "home.detailCopy");
  const eventTitle = getEventTitle(event);
  const eventImageUrl = getEventImageUrl(event);
  const eventDescription = getEventDescription(event, detailInfoCopy);
  const fallbackDescription = getEventDescription(event, detailCopy);
  const formatPrice = (value) => `$${value.toLocaleString(priceLocale)}`;

  const visibleSectors = useMemo(() => {
    if (!availableSectorIds) {
      return sectors;
    }

    return sectors.filter((sector) => availableSectorIds.has(sector.id));
  }, [availableSectorIds, sectors]);
  const canPurchase = visibleSectors.length > 0;

  useEffect(() => {
    if (sectors.length === 0) {
      setAvailableSectorIds(new Set());
      setIsFilteringSectors(false);
      setHasOwnReservation(false);
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

        setHasOwnReservation(hasCurrentUserReservation(seatLists));
        setAvailableSectorIds(getAvailableSectorIdSet(seatLists));
      } catch {
        if (!isCancelled) {
          setHasOwnReservation(false);
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

  const isSoldOut = event?.status?.trim().toLowerCase() === "soldout";
  useDocumentTitle(eventTitle || t(language, "home.detailTitle"));

  function handleContinuePurchase() {
    if (!session?.token) {
      navigate("/login", { state: { redirectTo: purchasePath } });
      return;
    }

    navigate(purchasePath);
  }

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

  return (
    <AppShell>
      <section className="event-detail-page">
        <section className="event-detail-layout">
          <div className="event-detail-main">
            <section className="event-detail-hero event-detail-hero-split">
              <div className="event-detail-hero-media">
                {eventImageUrl ? (
                  <img
                    className="event-detail-image"
                    src={eventImageUrl}
                    alt={eventTitle}
                  />
                ) : (
                  <div className="event-detail-image event-detail-placeholder">
                    <span className="event-placeholder-badge">
                      {t(language, "home.badge")}
                    </span>
                    <strong>{eventTitle}</strong>
                    <small>{formatEventDate(event.eventDate, language)}</small>
                  </div>
                )}
              </div>

              <div className="event-detail-hero-copy">
                <p className="header-badge">{t(language, "home.detailBadge")}</p>
                <h1>{eventTitle}</h1>
                <p className="header-text">{eventDescription}</p>
              </div>
            </section>

            <article className="event-detail-card">
              <h3>{t(language, "home.detailSectorsTitle")}</h3>

              {isLoadingSectors || isFilteringSectors ? (
                <p className="event-detail-copy">{t(language, "home.detailSectorsLoading")}</p>
              ) : sectorsError ? (
                <p className="event-detail-copy">{sectorsError}</p>
              ) : visibleSectors.length === 0 ? (
                <p className="event-detail-copy">{t(language, "home.detailSectorsEmpty")}</p>
              ) : (
                <div className="event-sectors-list">
                  {visibleSectors.map((sector) => (
                    <article className="event-sector-card" key={sector.id}>
                      <div className="event-sector-copy">
                        <strong>{sector.name}</strong>
                      </div>
                      <span>{formatPrice(sector.price)}</span>
                    </article>
                  ))}
                </div>
              )}
            </article>
          </div>

          <aside className="event-detail-sidebar">
            <article className="event-detail-card event-detail-info-side">
              <h2>{t(language, "home.detailInfoTitle")}</h2>
              <p className="event-detail-copy">{fallbackDescription}</p>

              <div className="event-detail-facts event-detail-facts-compact">
                <article>
                  <span>{t(language, "home.detailDateLabel")}</span>
                  <strong>{formatEventDate(event.eventDate, language)}</strong>
                </article>
                <article>
                  <span>{t(language, "home.detailVenueLabel")}</span>
                  <strong>{event.venue || "-"}</strong>
                </article>
                <article>
                  <span>{t(language, "home.detailStatusLabel")}</span>
                  <strong>
                    {formatEventStatus(event.status, language, {
                      hasSectors: visibleSectors.length > 0,
                    })}
                  </strong>
                </article>
              </div>
            </article>

            <article className="event-detail-action-card">
              <span className="event-placeholder-badge">
                {t(language, "home.buy")}
              </span>
              <h3>{t(language, "home.detailActionTitle")}</h3>
              <p>
                {isSoldOut && hasOwnReservation
                  ? t(language, "home.detailActionResumeCopy")
                  : isSoldOut
                    ? t(language, "home.detailActionSoldOutCopy")
                    : canPurchase
                      ? t(language, "home.detailActionCopy")
                      : t(language, "home.detailActionSoonCopy")}
              </p>
              <button
                className="btn btn-event event-detail-cta"
                onClick={handleContinuePurchase}
                disabled={!canPurchase || (isSoldOut && !hasOwnReservation)}
              >
                {t(language, "home.detailActionButton")}
              </button>
            </article>
          </aside>
        </section>
      </section>
    </AppShell>
  );
}

export default Evento;
