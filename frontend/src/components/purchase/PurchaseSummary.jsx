function PurchaseSummary({
  title,
  copy,
  selectedSector,
  selectedSeat,
  selectedSeatLabel,
  selectedSeatStateLabel,
  labels,
  serviceFee,
  finalPrice,
  formatPrice,
}) {
  return (
    <aside className="purchase-card purchase-summary-card">
      <h2>{title}</h2>
      <p className="event-detail-copy">{copy}</p>

      {selectedSector ? (
        <div className="purchase-summary-box">
          <span className="purchase-seat-map-label">{selectedSector.name}</span>

          <div className="purchase-selected-seat-card">
            <div className="purchase-selected-seat-copy">
              <span>{labels.selectedSeat}</span>
              <strong>{selectedSeatLabel || labels.selectedSeatEmpty}</strong>
            </div>
            {selectedSeat ? (
              <span
                className={`purchase-selected-seat-status ${
                  selectedSeat.reservedByCurrentUser ? "is-mine" : "is-ready"
                }`}
              >
                {selectedSeatStateLabel}
              </span>
            ) : null}
          </div>

          <div className="purchase-summary-lines">
            <div className="purchase-summary-line">
              <span>{labels.basePrice}</span>
              <strong>{formatPrice(selectedSector.price)}</strong>
            </div>
            <div className="purchase-summary-line">
              <span>{labels.fee}</span>
              <strong>{formatPrice(serviceFee)}</strong>
            </div>
            <div className="purchase-summary-line purchase-summary-line-total">
              <span>{labels.finalPrice}</span>
              <strong>{formatPrice(finalPrice)}</strong>
            </div>
          </div>
        </div>
      ) : (
        <p className="event-detail-copy">{labels.selectSectorFirst}</p>
      )}
    </aside>
  );
}

export default PurchaseSummary;
