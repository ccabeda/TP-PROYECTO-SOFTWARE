const SECTOR_SLOT_POSITIONS = [
  { position: "left-front", tone: "tone-1" },
  { position: "center-front", tone: "tone-2" },
  { position: "right-front", tone: "tone-3" },
  { position: "left-back", tone: "tone-4" },
  { position: "right-back", tone: "tone-5" },
];

function PurchaseVenueMap({
  title,
  copy,
  isLoading,
  error,
  emptyMessage,
  sectors,
  selectedSectorId,
  stageLabel,
  formatPrice,
  onSelectSector,
}) {
  const sectorSlots = buildSectorSlots(sectors);

  return (
    <section className="purchase-card purchase-map-card">
      <h2>{title}</h2>
      <p className="event-detail-copy">{copy}</p>

      {isLoading ? (
        <p className="event-detail-copy">{isLoading}</p>
      ) : error ? (
        <p className="event-detail-copy">{error}</p>
      ) : sectors.length === 0 ? (
        <p className="event-detail-copy">{emptyMessage}</p>
      ) : (
        <div className="purchase-venue-map">
          <div className="purchase-venue-stage">{stageLabel}</div>
          <div className="purchase-venue-grid">
            {sectorSlots.map((slot) => (
              <button
                type="button"
                key={slot.key}
                className={`purchase-venue-slot purchase-venue-slot-${slot.position} purchase-venue-slot-${slot.tone} ${
                  slot.sector ? "has-sector" : "is-empty"
                } ${slot.sector?.id === selectedSectorId ? "is-selected" : ""}`}
                onMouseDown={(event) => event.preventDefault()}
                onClick={() => {
                  if (slot.sector) {
                    onSelectSector(slot.sector.id);
                  }
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
  );
}

function buildSectorSlots(sectors) {
  return SECTOR_SLOT_POSITIONS.map(({ position, tone }, index) => ({
    key: position,
    position,
    tone,
    sector: sectors[index] ?? null,
  }));
}

export default PurchaseVenueMap;
