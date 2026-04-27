import { useNavigate } from "react-router-dom";
import { t } from "../../lib/i18n";

function EventCard({ event, language }) {
  const navigate = useNavigate();
  const eventPath = `/event/${event.id}`;
  const eventImageUrl = event.imagen || event.imageUrl;
  const badgeLabel = t(language, "home.badge");
  const moreInfoLabel = t(language, "home.moreInfo");
  const buyLabel = t(language, "home.buy");

  function goToEvent() {
    navigate(eventPath);
  }

  return (
    <article className="event-container">
      {eventImageUrl ? (
        <img
          src={eventImageUrl}
          alt={event.titulo}
          onClick={goToEvent}
        />
      ) : (
        <button
          type="button"
          className="event-image-placeholder"
          onClick={goToEvent}
        >
          <span className="event-placeholder-badge">{badgeLabel}</span>
          <strong>{event.titulo}</strong>
          <small>{event.fecha}</small>
        </button>
      )}

      <div className="event-body">
        <h3>{event.titulo}</h3>
        <p>{event.fecha}</p>

        <div className="event-actions">
          <button
            type="button"
            className="btn link-button"
            onClick={goToEvent}
          >
            {moreInfoLabel}
          </button>

          <button
            type="button"
            className="btn btn-event"
            onClick={goToEvent}
          >
            {buyLabel}
          </button>
        </div>
      </div>
    </article>
  );
}

export default EventCard;
