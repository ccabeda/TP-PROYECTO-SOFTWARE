import { useMemo } from "react";
import { t } from "../../lib/i18n";

function SeatSelectionPanel({
  title,
  copy,
  selectedSector,
  selectedSeat,
  selectedSeatId,
  selectedSeatLabel,
  selectedSeatStateLabel,
  seats,
  seatsError,
  isLoadingSeats,
  language,
  labels,
  seatWarning,
  isReservingSeat,
  onSelectSeat,
  onContinue,
}) {
  const groupedSeats = useMemo(() => groupSeatsByRow(seats), [seats]);

  return (
    <section className="purchase-card purchase-seats-panel">
      <h2>{title}</h2>
      <p className="event-detail-copy">{copy}</p>

      {!selectedSector ? (
        <p className="event-detail-copy">{labels.selectSectorFirst}</p>
      ) : seatsError ? (
        <p className="event-detail-copy">{seatsError}</p>
      ) : !isLoadingSeats && seats.length === 0 ? (
        <p className="event-detail-copy">{labels.seatsEmpty}</p>
      ) : (
        <div className={`purchase-seat-map-shell ${isLoadingSeats ? "is-loading" : ""}`}>
          <div className="purchase-seat-map-header">
            <div>
              <span className="purchase-seat-map-label">{selectedSector.name}</span>
              {selectedSeat ? <strong>{selectedSeatLabel}</strong> : null}
            </div>
            {selectedSeat ? (
              <span
                className={`purchase-seat-focus-badge ${
                  selectedSeat.reservedByCurrentUser ? "is-mine" : "is-ready"
                }`}
              >
                {selectedSeatStateLabel}
              </span>
            ) : null}
          </div>

          {isLoadingSeats ? (
            <p className="purchase-seat-loading">{labels.seatsLoading}</p>
          ) : null}

          <div className="seat-legend">
            <span className="seat-legend-item">
              <i className="seat-legend-swatch seat-legend-available" />
              {labels.legendAvailable}
            </span>
            <span className="seat-legend-item">
              <i className="seat-legend-swatch seat-legend-mine" />
              {labels.legendMine}
            </span>
            <span className="seat-legend-item">
              <i className="seat-legend-swatch seat-legend-reserved" />
              {labels.legendReserved}
            </span>
            <span className="seat-legend-item">
              <i className="seat-legend-swatch seat-legend-sold" />
              {labels.legendSold}
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
                      aria-label={getSeatAriaLabel(row, seat, language)}
                      onClick={() => onSelectSeat(seat.id)}
                    >
                      {row}
                      {seat.seatNumber}
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
              onClick={onContinue}
              disabled={isReservingSeat}
            >
              {isReservingSeat ? labels.reservingButton : labels.continueButton}
            </button>
          </div>
        </div>
      )}
    </section>
  );
}

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

function getSeatAriaLabel(row, seat, language) {
  const seatLabel = `${row}${seat.seatNumber}`;
  return language === "en"
    ? `Seat ${seatLabel}, ${getSeatStatusLabel(seat.status, language)}`
    : `Butaca ${seatLabel}, ${getSeatStatusLabel(seat.status, language)}`;
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

export default SeatSelectionPanel;
