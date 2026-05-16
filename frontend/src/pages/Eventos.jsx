import { useState } from "react";
import DatePicker from "react-datepicker";
import { format } from "date-fns";
import { enUS, es } from "date-fns/locale";
import AppShell from "../components/layout/AppShell";
import EventCard from "../components/events/EventCard";
import useDocumentTitle from "../hooks/useDocumentTitle";
import useEvents from "../hooks/useEvents";
import useLanguageContext from "../context/useLanguageContext";
import { formatEventDate } from "../lib/eventFormat";
import { t } from "../lib/i18n";

function Eventos() {
  const { language } = useLanguageContext();
  const [searchTerm, setSearchTerm] = useState("");
  const [selectedDate, setSelectedDate] = useState(null);
  const [page, setPage] = useState(1);
  const locale = language === "es" ? es : enUS;
  const today = new Date();
  const allEventsLabel = t(language, "home.allEvents");
  const searchPlaceholder = t(language, "home.searchPlaceholder");
  const datePlaceholder = t(language, "home.datePlaceholder");
  const dateFilterLabel = t(language, "home.dateFilterLabel");
  const eventsLoadingLabel = t(language, "home.eventsLoading");
  const emptyEventsLabel = t(language, "home.emptyEvents");
  const previousPageLabel = t(language, "home.previousPage");
  const nextPageLabel = t(language, "home.nextPage");
  const {
    events,
    isLoading,
    error,
    totalCount,
    totalPages,
  } = useEvents({
    name: searchTerm,
    eventDate: selectedDate ? format(selectedDate, "yyyy-MM-dd") : "",
    page,
  });
  const pageIndicatorLabel = t(language, "home.pageIndicator", {
    page: String(page),
    totalPages: String(totalPages || 1),
  });
  const eventsCountLabel = t(language, "home.eventsCount", {
    count: String(totalCount),
  });
  useDocumentTitle(t(language, "topbar.events"));

  function handleSearchChange(event) {
    setSearchTerm(event.target.value);
    setPage(1);
  }

  function handleDateChange(date) {
    setSelectedDate(date);
    setPage(1);
  }

  function handlePreviousPage() {
    setPage((currentPage) => Math.max(1, currentPage - 1));
  }

  function handleNextPage() {
    setPage((currentPage) => currentPage + 1);
  }

  return (
    <AppShell>
      <section className="events-section">
        <div className="events-toolbar">
          <h1>{allEventsLabel}</h1>
          <div className="events-filters">
            <input
              className="events-search"
              type="search"
              placeholder={searchPlaceholder}
              value={searchTerm}
              onChange={handleSearchChange}
            />
            <DatePicker
              selected={selectedDate}
              onChange={handleDateChange}
              minDate={today}
              locale={locale}
              dateFormat="dd/MM/yyyy"
              placeholderText={datePlaceholder}
              className="events-search events-date-filter"
              ariaLabelClose={dateFilterLabel}
              isClearable
              popperPlacement="bottom-end"
              showPopperArrow={false}
            />
          </div>
        </div>

        {isLoading ? (
          <p className="events-feedback">{eventsLoadingLabel}</p>
        ) : error ? (
          <p className="events-feedback">{error}</p>
        ) : (
          <>
            {events.length === 0 ? (
              <p className="events-feedback">{emptyEventsLabel}</p>
            ) : null}

            {events.length > 0 ? (
              <div className="events-pagination-summary">
                <span>{eventsCountLabel}</span>
                <span>{pageIndicatorLabel}</span>
              </div>
            ) : null}

            <div className="events-grid">
              {events.map((event) => (
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

            {totalPages > 1 ? (
              <div className="events-pagination">
                <button
                  type="button"
                  className="btn events-pagination-button events-pagination-button-secondary"
                  onClick={handlePreviousPage}
                  disabled={page === 1}
                >
                  {previousPageLabel}
                </button>

                <span className="events-pagination-indicator">
                  {pageIndicatorLabel}
                </span>

                <button
                  type="button"
                  className="btn btn-event events-pagination-button"
                  onClick={handleNextPage}
                  disabled={page >= totalPages}
                >
                  {nextPageLabel}
                </button>
              </div>
            ) : null}
          </>
        )}
      </section>
    </AppShell>
  );
}

export default Eventos;
