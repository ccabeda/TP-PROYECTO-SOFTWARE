import { useEffect, useMemo, useState } from "react";
import { useNavigate } from "react-router-dom";
import AppShell from "../components/layout/AppShell";
import { useAuthContext } from "../context/AuthContext";
import { useLanguageContext } from "../context/LanguageContext";
import useDocumentTitle from "../hooks/useDocumentTitle";
import { formatEventDate } from "../lib/eventFormat";
import getErrorMessage from "../lib/getErrorMessage";
import { t } from "../lib/i18n";
import { getMyTickets } from "../services/ticketsService";

const MY_TICKETS_PATH = "/my-tickets";

function getPriceLocale(language) {
  return language === "en" ? "en-US" : "es-AR";
}

function MyTickets() {
  const navigate = useNavigate();
  const { session } = useAuthContext();
  const { language } = useLanguageContext();
  const [tickets, setTickets] = useState([]);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState("");
  const priceLocale = getPriceLocale(language);
  const badgeLabel = t(language, "home.myTicketsBadge");
  const titleLabel = t(language, "home.myTicketsTitle");
  const copyLabel = t(language, "home.myTicketsCopy");
  const loadingLabel = t(language, "home.myTicketsLoading");
  const errorLabel = t(language, "home.myTicketsError");
  const emptyTitle = t(language, "home.myTicketsEmptyTitle");
  const emptyCopy = t(language, "home.myTicketsEmptyCopy");
  const paidLabel = t(language, "home.myTicketsStatusPaid");
  const sectorLabel = t(language, "home.myTicketsSectorLabel");
  const seatLabel = t(language, "home.myTicketsSeatLabel");
  const priceLabel = t(language, "home.myTicketsPriceLabel");
  const qrLabel = t(language, "home.myTicketsQrLabel");
  const viewEventLabel = t(language, "home.myTicketsViewEventButton");
  const formatPrice = (value) => `$${Number(value).toLocaleString(priceLocale)}`;
  useDocumentTitle(t(language, "topbar.myTickets"));

  useEffect(() => {
    if (!session?.token) {
      navigate("/login", { replace: true, state: { redirectTo: MY_TICKETS_PATH } });
      return;
    }

    let isCancelled = false;

    async function loadTickets() {
      setIsLoading(true);
      setError("");

      try {
        const result = await getMyTickets({ token: session.token });

        if (!isCancelled) {
          setTickets(result);
        }
      } catch (loadError) {
        if (!isCancelled) {
          setError(getErrorMessage(loadError, errorLabel));
        }
      } finally {
        if (!isCancelled) {
          setIsLoading(false);
        }
      }
    }

    void loadTickets();

    return () => {
      isCancelled = true;
    };
  }, [language, navigate, session]);

  const upcomingTickets = useMemo(() => {
    const now = new Date();

    return tickets.filter((ticket) => new Date(ticket.eventDate) >= now);
  }, [tickets]);

  return (
    <AppShell>
      <section className="my-tickets-page">
        <div className="purchase-header">
          <p className="header-badge">{badgeLabel}</p>
          <h1>{titleLabel}</h1>
          <p className="header-text">{copyLabel}</p>
        </div>

        {isLoading ? (
          <p className="events-feedback">{loadingLabel}</p>
        ) : error ? (
          <p className="events-feedback">{error}</p>
        ) : upcomingTickets.length === 0 ? (
          <article className="purchase-card my-ticket-empty">
            <h2>{emptyTitle}</h2>
            <p className="event-detail-copy">{emptyCopy}</p>
          </article>
        ) : (
          <div className="my-tickets-grid">
            {upcomingTickets.map((ticket) => (
              <article className="purchase-card my-ticket-card" key={ticket.id}>
                <div className="my-ticket-layout">
                  <div className="my-ticket-main">
                    <div className="my-ticket-card-head">
                      <span className="purchase-seat-map-label">
                        {ticket.sectorName} · {ticket.rowIdentifier}{ticket.seatNumber}
                      </span>
                      <span className="my-ticket-status">{paidLabel}</span>
                    </div>

                    <h2>{ticket.eventName}</h2>
                    <p className="event-detail-copy">{formatEventDate(ticket.eventDate, language)}</p>
                    <p className="event-detail-copy">{ticket.venue}</p>

                    <div className="purchase-summary-lines my-ticket-lines">
                      <div className="purchase-summary-line">
                        <span>{sectorLabel}</span>
                        <strong>{ticket.sectorName}</strong>
                      </div>
                      <div className="purchase-summary-line">
                        <span>{seatLabel}</span>
                        <strong>
                          {ticket.rowIdentifier}
                          {ticket.seatNumber}
                        </strong>
                      </div>
                      <div className="purchase-summary-line">
                        <span>{priceLabel}</span>
                        <strong>{formatPrice(ticket.sectorPrice)}</strong>
                      </div>
                    </div>
                  </div>

                  <aside className="my-ticket-qr-panel" aria-hidden="true">
                    <div className="my-ticket-qr-card">
                      <div className="my-ticket-qr">
                        <span className="my-ticket-qr-corner corner-top-left" />
                        <span className="my-ticket-qr-corner corner-top-right" />
                        <span className="my-ticket-qr-corner corner-bottom-left" />
                        <span className="my-ticket-qr-corner corner-bottom-right" />
                      </div>
                      <span className="my-ticket-qr-label">{qrLabel}</span>
                    </div>
                  </aside>
                </div>

                <button
                  type="button"
                  className="button button-primary purchase-continue-button"
                  onClick={() => navigate(`/event/${ticket.eventId}`)}
                >
                  {viewEventLabel}
                </button>
              </article>
            ))}
          </div>
        )}
      </section>
    </AppShell>
  );
}

export default MyTickets;
