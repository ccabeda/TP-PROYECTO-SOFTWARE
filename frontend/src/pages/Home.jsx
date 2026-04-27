import { useMemo } from "react";
import { useNavigate } from "react-router-dom";
import AppShell from "../components/layout/AppShell";
import EventCard from "../components/events/EventCard";
import useDocumentTitle from "../hooks/useDocumentTitle";
import useEvents from "../hooks/useEvents";
import { useLanguageContext } from "../context/LanguageContext";
import { t } from "../lib/i18n";
import { formatEventDate } from "../lib/eventFormat";

const TOP_EVENTS_SESSION_KEY = "ticketunaj_top_events";
const MAX_TOP_EVENTS = 8;

function getEventTitle(event) {
  return event?.titulo || event?.title || event?.name || "";
}

function getEventImageUrl(event) {
  return event?.imagen || event?.imageUrl || "";
}

function getStoredTopEventIds() {
  if (typeof window === "undefined") {
    return [];
  }

  try {
    const storedIds = JSON.parse(
      window.sessionStorage.getItem(TOP_EVENTS_SESSION_KEY) ?? "[]",
    );

    return Array.isArray(storedIds) ? storedIds : [];
  } catch {
    window.sessionStorage.removeItem(TOP_EVENTS_SESSION_KEY);
    return [];
  }
}

function persistTopEventIds(events) {
  if (typeof window === "undefined") {
    return;
  }

  window.sessionStorage.setItem(
    TOP_EVENTS_SESSION_KEY,
    JSON.stringify(events.map((event) => event.id)),
  );
}

function getPrioritizedTopEvents(events, storedIds) {
  if (storedIds.length === 0) {
    return [];
  }

  const prioritized = storedIds
    .map((id) => events.find((event) => event.id === id))
    .filter(Boolean);

  if (prioritized.length === 0) {
    return [];
  }

  const remaining = events.filter(
    (event) => !prioritized.some((selected) => selected.id === event.id),
  );

  return [...prioritized, ...remaining].slice(0, MAX_TOP_EVENTS);
}

function Home() {
  const navigate = useNavigate();
  const { language } = useLanguageContext();
  const { events, isLoading, error } = useEvents();
  const featuredEvents = useMemo(() => events.slice(0, 3), [events]);
  const spotlightEvents = useMemo(() => getSessionTopEvents(events), [events]);
  const badgeLabel = t(language, "home.badge");
  const titleLabel = t(language, "home.title");
  const copyLabel = t(language, "home.copy");
  const viewAllLabel = t(language, "home.viewAll");
  const statsListedLabel = t(language, "home.statsListed");
  const statsIssuedLabel = t(language, "home.statsIssued");
  const statsSatisfactionLabel = t(language, "home.statsSatisfaction");
  const tagConcertLabel = t(language, "home.tagConcert");
  const tagFestivalLabel = t(language, "home.tagFestival");
  const tagVerifiedLabel = t(language, "home.tagVerified");
  const tagBestPriceLabel = t(language, "home.tagBestPrice");
  const tagLimitedOffersLabel = t(language, "home.tagLimitedOffers");
  const tagEarlyAccessLabel = t(language, "home.tagEarlyAccess");
  const categoryConcertsLabel = t(language, "home.categoryConcerts");
  const categoryConcertsCopy = t(language, "home.categoryConcertsCopy");
  const categorySportsLabel = t(language, "home.categorySports");
  const categorySportsCopy = t(language, "home.categorySportsCopy");
  const upcomingLabel = t(language, "home.upcoming");
  const topEventsTitle = t(language, "home.topEventsTitle");
  const topEventsCopy = t(language, "home.topEventsCopy");
  const legalTitle = t(language, "home.legalTitle");
  const legalCopy = t(language, "home.legalCopy");
  const buyLabel = t(language, "home.buy");
  const loadingEventsLabel = t(language, "home.eventsLoading");
  const loadingFeaturedLabel = t(language, "home.featuredLoading");
  const heroImageAlt =
    language === "en" ? "Crowd at a live event" : "Público en un evento en vivo";
  useDocumentTitle(t(language, "topbar.home"));

  return (
    <AppShell>
      <section className="landing-grid">
        <div className="landing-main">
          <section className="hero-panel">
            <img
              className="hero-panel-image"
              src="/images/hero-main.jpg"
              alt={heroImageAlt}
            />
            <div className="hero-panel-overlay" />

            <div className="hero-panel-content">
              <p className="header-badge">{badgeLabel}</p>
              <h1>{titleLabel}</h1>
              <p className="header-text">{copyLabel}</p>

              <div className="hero-panel-actions">
                <button
                  type="button"
                  className="btn btn-event"
                  onClick={() => navigate("/events")}
                >
                  {viewAllLabel}
                </button>
              </div>

              <div className="hero-stats">
                <article>
                  <strong>100+</strong>
                  <span>{statsListedLabel}</span>
                </article>
                <article>
                  <strong>15K+</strong>
                  <span>{statsIssuedLabel}</span>
                </article>
                <article>
                  <strong>95%</strong>
                  <span>{statsSatisfactionLabel}</span>
                </article>
              </div>
            </div>
          </section>

          <div className="hero-tags">
            <span>{tagConcertLabel}</span>
            <span>{tagFestivalLabel}</span>
            <span>{tagVerifiedLabel}</span>
            <span>{tagBestPriceLabel}</span>
            <span>{tagLimitedOffersLabel}</span>
            <span>{tagEarlyAccessLabel}</span>
          </div>

          <section className="category-strip">
            <article className="category-card category-card-music">
              <div>
                <h3>{categoryConcertsLabel}</h3>
                <p>{categoryConcertsCopy}</p>
              </div>
            </article>
            <article className="category-card category-card-sports">
              <div>
                <h3>{categorySportsLabel}</h3>
                <p>{categorySportsCopy}</p>
              </div>
            </article>
          </section>

          <section className="events-section home-upcoming-section" id="events">
            <div className="section-header">
              <h2>{upcomingLabel}</h2>
            </div>

            {isLoading ? (
              <p className="events-feedback">{loadingEventsLabel}</p>
            ) : error ? (
              <p className="events-feedback">{error}</p>
            ) : (
              <div className="events-grid">
                {featuredEvents.map((event) => (
                  <EventCard
                    key={event.id}
                    event={{
                      ...event,
                      fecha: formatEventDate(event.eventDate, language),
                    }}
                    language={language}
                  />
                ))}
              </div>
            )}
          </section>
        </div>

        <aside className="landing-sidebar">
          <section className="sidebar-panel">
            <div className="sidebar-heading">
              <h2>{topEventsTitle}</h2>
              <p>{topEventsCopy}</p>
            </div>

            {isLoading ? (
              <p className="sidebar-feedback">{loadingFeaturedLabel}</p>
            ) : error ? (
              <p className="sidebar-feedback">{error}</p>
            ) : (
              <div className="sidebar-list">
                {spotlightEvents.map((event) => (
                  <article className="sidebar-event-card" key={event.id}>
                    {getEventImageUrl(event) ? (
                      <img src={getEventImageUrl(event)} alt={getEventTitle(event)} />
                    ) : (
                      <div className="sidebar-image-placeholder">
                        <span>{getEventTitle(event)}</span>
                      </div>
                    )}
                    <div className="sidebar-event-body">
                      <h3>{getEventTitle(event)}</h3>
                      <p>{formatEventDate(event.eventDate, language)}</p>
                      <button
                        type="button"
                        className="btn btn-event sidebar-event-button"
                        onClick={() => navigate(`/event/${event.id}`)}
                      >
                        {buyLabel}
                      </button>
                    </div>
                  </article>
                ))}
              </div>
            )}
          </section>

          <section className="membership-panel legal-panel">
            <h3>{legalTitle}</h3>
            <p>{legalCopy}</p>
          </section>
        </aside>
      </section>
    </AppShell>
  );
}

export default Home;

function getSessionTopEvents(events) {
  if (events.length === 0) {
    return [];
  }

  const storedIds = getStoredTopEventIds();
  const prioritized = getPrioritizedTopEvents(events, storedIds);

  if (prioritized.length > 0) {
    return prioritized;
  }

  const shuffled = [...events].sort(() => Math.random() - 0.5);
  const selected = shuffled.slice(0, MAX_TOP_EVENTS);
  persistTopEventIds(selected);

  return selected;
}
