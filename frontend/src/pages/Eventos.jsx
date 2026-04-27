import { useState } from "react";
import DatePicker from "react-datepicker";
import { format } from "date-fns";
import { enUS, es } from "date-fns/locale";
import AppShell from "../components/layout/AppShell";
import EventCard from "../components/events/EventCard";
import useDocumentTitle from "../hooks/useDocumentTitle";
import useEvents from "../hooks/useEvents";
import { useLanguageContext } from "../context/LanguageContext";
import { formatEventDate } from "../lib/eventFormat";
import { t } from "../lib/i18n";

function Eventos() {
  const { language } = useLanguageContext();
  const [searchTerm, setSearchTerm] = useState("");
  const [selectedDate, setSelectedDate] = useState(null);
  const locale = language === "es" ? es : enUS;
  const today = new Date();
  const allEventsLabel = t(language, "home.allEvents");
  const searchPlaceholder = t(language, "home.searchPlaceholder");
  const datePlaceholder = t(language, "home.datePlaceholder");
  const dateFilterLabel = t(language, "home.dateFilterLabel");
  const eventsLoadingLabel = t(language, "home.eventsLoading");
  const emptyEventsLabel = t(language, "home.emptyEvents");
  const { events, isLoading, error } = useEvents({
    name: searchTerm,
    eventDate: selectedDate ? format(selectedDate, "yyyy-MM-dd") : "",
  });
  useDocumentTitle(t(language, "topbar.events"));

  return (
    <AppShell>
      <div className="events-toolbar">
        <h1>{allEventsLabel}</h1>
        <div className="events-filters">
          <input
            className="events-search"
            type="search"
            placeholder={searchPlaceholder}
            value={searchTerm}
            onChange={(event) => setSearchTerm(event.target.value)}
          />
          <DatePicker
            selected={selectedDate}
            onChange={(date) => setSelectedDate(date)}
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
        </>
      )}
    </AppShell>
  );
}

export default Eventos;
