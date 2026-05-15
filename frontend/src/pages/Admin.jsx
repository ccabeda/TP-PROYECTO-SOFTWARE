import { useEffect, useMemo, useState } from "react";
import DatePicker from "react-datepicker";
import { format } from "date-fns";
import { enUS, es } from "date-fns/locale";
import { useNavigate } from "react-router-dom";
import AppShell from "../components/layout/AppShell";
import useAuthContext from "../context/useAuthContext";
import useLanguageContext from "../context/useLanguageContext";
import useToastContext from "../context/useToastContext";
import useDocumentTitle from "../hooks/useDocumentTitle";
import getErrorMessage from "../lib/getErrorMessage";
import { t } from "../lib/i18n";
import { createAdminEventBundle } from "../services/adminService";

const ADMIN_PATH = "/admin";
const MAX_ADMIN_SECTORS = 5;
const MAX_ADMIN_ROWS = 10;
const MAX_ADMIN_SEATS_PER_ROW = 20;
const MAX_ADMIN_SECTOR_CAPACITY = 200;
const INITIAL_EVENT_FORM = {
  name: "",
  eventDate: "",
  venue: "",
  status: "Scheduled",
  imageUrl: "",
  description: "",
};

function createEmptySectorForm() {
  const fallbackId = `${Date.now()}-${Math.random().toString(16).slice(2)}`;
  const generatedId = globalThis.crypto?.randomUUID?.() ?? fallbackId;

  return {
    id: generatedId,
    name: "",
    price: "",
    capacity: "",
    rowCount: "",
    seatsPerRow: "",
  };
}

function isAdminSession(session) {
  return session?.role?.trim().toLowerCase() === "admin";
}

function clampNumericField(name, value) {
  if (value === "") {
    return value;
  }

  const numericValue = Number(value);

  if (!Number.isFinite(numericValue)) {
    return value;
  }

  if (name === "capacity") {
    return String(Math.min(Math.max(numericValue, 1), MAX_ADMIN_SECTOR_CAPACITY));
  }

  if (name === "rowCount") {
    return String(Math.min(Math.max(numericValue, 1), MAX_ADMIN_ROWS));
  }

  if (name === "seatsPerRow") {
    return String(Math.min(Math.max(numericValue, 1), MAX_ADMIN_SEATS_PER_ROW));
  }

  return value;
}

function validateSectorForms(sectors, capacityExceededMessage) {
  for (const [index, sector] of sectors.entries()) {
    const sectorNumber = index + 1;
    const capacity = Number(sector.capacity);
    const price = Number(sector.price);
    const rowCount = Number(sector.rowCount);
    const seatsPerRow = Number(sector.seatsPerRow);

    if (!sector.name.trim()) {
      return `Debes completar el nombre del sector ${sectorNumber}.`;
    }

    if (!Number.isFinite(price) || price <= 0) {
      return `Debes ingresar un precio válido para el sector ${sectorNumber}.`;
    }

    if (!Number.isFinite(capacity) || capacity <= 0) {
      return `Debes ingresar una capacidad válida para el sector ${sectorNumber}.`;
    }

    if (!Number.isFinite(rowCount) || rowCount <= 0) {
      return `Debes ingresar una cantidad de filas válida para el sector ${sectorNumber}.`;
    }

    if (!Number.isFinite(seatsPerRow) || seatsPerRow <= 0) {
      return `Debes ingresar una cantidad válida de asientos por fila para el sector ${sectorNumber}.`;
    }

    if (
      Number.isFinite(capacity) &&
      Number.isFinite(rowCount) &&
      Number.isFinite(seatsPerRow) &&
      rowCount > 0 &&
      seatsPerRow > 0 &&
      rowCount * seatsPerRow > capacity
    ) {
      return capacityExceededMessage;
    }
  }

  return "";
}

function validateEventForm(eventForm, dateRequiredMessage) {
  if (!eventForm.name.trim()) {
    return "Debes completar el nombre del evento.";
  }

  if (!eventForm.venue.trim()) {
    return "Debes completar el estadio del evento.";
  }

  if (!eventForm.eventDate) {
    return dateRequiredMessage;
  }

  return "";
}

function Admin() {
  const navigate = useNavigate();
  const { session } = useAuthContext();
  const { language } = useLanguageContext();
  const { showToast } = useToastContext();
  const [selectedEventDate, setSelectedEventDate] = useState(null);
  const [eventForm, setEventForm] = useState(INITIAL_EVENT_FORM);
  const [sectorForms, setSectorForms] = useState([createEmptySectorForm()]);
  const [submitMessage, setSubmitMessage] = useState("");
  const [submitError, setSubmitError] = useState("");
  const [isSubmitting, setIsSubmitting] = useState(false);
  const isAdmin = isAdminSession(session);
  const locale = language === "es" ? es : enUS;
  const now = new Date();

  const adminLabel = t(language, "topbar.admin");
  const adminTitle = t(language, "home.adminTitle");
  const adminCopy = t(language, "home.adminCopy");
  const adminForbiddenTitle = t(language, "home.adminForbiddenTitle");
  const adminForbiddenCopy = t(language, "home.adminForbiddenCopy");
  const adminEventsTitle = t(language, "home.adminEventsTitle");
  const adminEventsCopy = t(language, "home.adminEventsCopy");
  const adminNameLabel = t(language, "home.adminNameLabel");
  const adminEventDateLabel = t(language, "home.adminEventDateLabel");
  const adminVenueLabel = t(language, "home.adminVenueLabel");
  const adminImageUrlLabel = t(language, "home.adminImageUrlLabel");
  const adminDescriptionLabel = t(language, "home.adminDescriptionLabel");
  const adminSectorLabel = t(language, "home.adminSectorLabel");
  const adminPriceLabel = t(language, "home.adminPriceLabel");
  const adminCapacityLabel = t(language, "home.adminCapacityLabel");
  const adminRowCountLabel = t(language, "home.adminRowCountLabel");
  const adminSeatsPerRowLabel = t(language, "home.adminSeatsPerRowLabel");
  const adminCreateBundleButton = t(language, "home.adminCreateBundleButton");
  const adminCreatingBundle = t(language, "home.adminCreatingBundle");
  const adminBundleCreated = t(language, "home.adminBundleCreated");
  const adminAddSector = t(language, "home.adminAddSector");
  const adminRemoveSector = t(language, "home.adminRemoveSector");
  const adminSectorCardTitle = t(language, "home.adminSectorCardTitle");
  const adminSectorCardCopy = t(language, "home.adminSectorCardCopy");
  const adminMinimumSectorError = t(language, "home.adminMinimumSectorError");
  const adminMaxSectorsReached = t(language, "home.adminMaxSectorsReached");
  const adminCapacityHelper = t(language, "home.adminCapacityHelper");
  const adminRowCountHelper = t(language, "home.adminRowCountHelper");
  const adminSeatsPerRowHelper = t(language, "home.adminSeatsPerRowHelper");
  const adminCapacityExceededError = t(language, "home.adminCapacityExceededError");
  const adminEventDateRequiredError = t(language, "home.adminEventDateRequiredError");

  useDocumentTitle(adminTitle);

  useEffect(() => {
    if (!session?.token) {
      navigate("/login", { replace: true, state: { redirectTo: ADMIN_PATH } });
    }
  }, [navigate, session]);

  const canRemoveSectors = sectorForms.length > 1;
  const canAddSectors = sectorForms.length < MAX_ADMIN_SECTORS;
  const sectorCountLabel = useMemo(
    () =>
      sectorForms.length === 1
        ? `${adminSectorCardTitle} 1`
        : `${adminSectorCardTitle} ${sectorForms.length}`,
    [adminSectorCardTitle, sectorForms.length],
  );

  function handleEventFormChange(event) {
    const { name, value } = event.target;
    setEventForm((current) => ({ ...current, [name]: value }));
  }

  function handleEventDateChange(date) {
    setSelectedEventDate(date);
    setEventForm((current) => ({
      ...current,
      eventDate: date ? format(date, "yyyy-MM-dd'T'HH:mm:ss") : "",
    }));
  }

  function handleSectorFormChange(sectorId, event) {
    const { name, value } = event.target;
    const nextValue = clampNumericField(name, value);
    setSectorForms((current) =>
      current.map((sector) =>
        sector.id === sectorId ? { ...sector, [name]: nextValue } : sector,
      ),
    );
  }

  function handleAddSector() {
    setSectorForms((current) => {
      if (current.length >= MAX_ADMIN_SECTORS) {
        return current;
      }

      return [...current, createEmptySectorForm()];
    });
  }

  function handleRemoveSector(sectorId) {
    setSectorForms((current) => {
      if (current.length === 1) {
        return current;
      }

      return current.filter((sector) => sector.id !== sectorId);
    });
  }

  async function handleSubmit(event) {
    event.preventDefault();

    if (!session?.token) {
      return;
    }

    setSubmitError("");
    setSubmitMessage("");

    if (sectorForms.length === 0) {
      setSubmitError(adminMinimumSectorError);
      return;
    }

    const eventValidationError = validateEventForm(
      eventForm,
      adminEventDateRequiredError,
    );

    if (eventValidationError) {
      setSubmitError(eventValidationError);
      return;
    }

    const sectorValidationError = validateSectorForms(
      sectorForms,
      adminCapacityExceededError,
    );

    if (sectorValidationError) {
      setSubmitError(sectorValidationError);
      return;
    }

    setIsSubmitting(true);

    try {
      await createAdminEventBundle(
        {
          name: eventForm.name,
          eventDate: eventForm.eventDate,
          venue: eventForm.venue,
          status: eventForm.status,
          imageUrl: eventForm.imageUrl || null,
          description: eventForm.description || null,
          sectors: sectorForms.map((sector) => ({
            name: sector.name,
            price: Number(sector.price),
            capacity: Number(sector.capacity),
            rowCount: Number(sector.rowCount),
            seatsPerRow: Number(sector.seatsPerRow),
          })),
        },
        session.token,
      );

      setEventForm(INITIAL_EVENT_FORM);
      setSelectedEventDate(null);
      setSectorForms([createEmptySectorForm()]);
      setSubmitMessage(adminBundleCreated);
      showToast(adminBundleCreated, "success");
    } catch (error) {
      const errorMessage = getErrorMessage(
        error,
        "No se pudo crear el evento completo con sus sectores y butacas.",
      );
      setSubmitError(errorMessage);
      showToast(errorMessage, "error");
    } finally {
      setIsSubmitting(false);
    }
  }

  if (!session?.token) {
    return null;
  }

  if (!isAdmin) {
    return (
      <AppShell>
        <section className="admin-page">
          <article className="purchase-card admin-panel-card">
            <h1>{adminForbiddenTitle}</h1>
            <p className="event-detail-copy">{adminForbiddenCopy}</p>
          </article>
        </section>
      </AppShell>
    );
  }

  return (
    <AppShell>
      <section className="admin-page">
        <div className="purchase-header admin-header">
          <div className="purchase-header-badge-row">
            <p className="header-badge purchase-header-badge">{adminLabel}</p>
          </div>
          <h1>{adminTitle}</h1>
          <p className="header-text admin-header-copy">{adminCopy}</p>
        </div>

        <article className="purchase-card admin-panel-card admin-bundle-card">
          <div className="admin-bundle-head">
            <div>
              <h2>{adminEventsTitle}</h2>
              <p className="event-detail-copy">{adminEventsCopy}</p>
            </div>
            <span className="purchase-header-date admin-sector-count">
              {sectorCountLabel}
            </span>
          </div>

          <form className="admin-form admin-bundle-form" onSubmit={handleSubmit} noValidate>
            <div className="admin-form-grid">
              <div className="admin-field-group">
                <label className="auth-label" htmlFor="admin-event-name">
                  {adminNameLabel}
                </label>
                <input
                  id="admin-event-name"
                  className="auth-input"
                  name="name"
                  value={eventForm.name}
                  onChange={handleEventFormChange}
                />
              </div>

              <div className="admin-field-group">
                <label className="auth-label" htmlFor="admin-event-date">
                  {adminEventDateLabel}
                </label>
                <DatePicker
                  id="admin-event-date"
                  selected={selectedEventDate}
                  onChange={handleEventDateChange}
                  minDate={now}
                  minTime={
                    selectedEventDate &&
                    selectedEventDate.toDateString() === now.toDateString()
                      ? now
                      : new Date(new Date().setHours(0, 0, 0, 0))
                  }
                  maxTime={new Date(new Date().setHours(23, 45, 0, 0))}
                  locale={locale}
                  dateFormat="dd/MM/yyyy HH:mm"
                  showTimeSelect
                  timeIntervals={15}
                  placeholderText={adminEventDateLabel}
                  className="auth-input events-search events-date-filter admin-date-filter"
                  showPopperArrow={false}
                  popperPlacement="bottom-start"
                />
              </div>

              <div className="admin-field-group">
                <label className="auth-label" htmlFor="admin-event-venue">
                  {adminVenueLabel}
                </label>
                <input
                  id="admin-event-venue"
                  className="auth-input"
                  name="venue"
                  value={eventForm.venue}
                  onChange={handleEventFormChange}
                />
              </div>
            </div>

            <div className="admin-field-group">
              <label className="auth-label" htmlFor="admin-event-image">
                {adminImageUrlLabel}
              </label>
              <input
                id="admin-event-image"
                className="auth-input"
                name="imageUrl"
                value={eventForm.imageUrl}
                onChange={handleEventFormChange}
              />
            </div>

            <div className="admin-field-group">
              <label className="auth-label" htmlFor="admin-event-description">
                {adminDescriptionLabel}
              </label>
              <textarea
                id="admin-event-description"
                className="auth-input admin-textarea"
                name="description"
                value={eventForm.description}
                onChange={handleEventFormChange}
              />
            </div>

            <div className="admin-sector-list">
              {sectorForms.map((sector, index) => (
                <article key={sector.id} className="admin-sector-card">
                  <div className="admin-sector-card-head">
                    <div>
                      <strong>
                        {adminSectorCardTitle} {index + 1}
                      </strong>
                      <p>{adminSectorCardCopy}</p>
                    </div>
                    <button
                      type="button"
                      className="button button-secondary admin-sector-remove"
                      onClick={() => handleRemoveSector(sector.id)}
                      disabled={!canRemoveSectors}
                    >
                      {adminRemoveSector}
                    </button>
                  </div>

                  <div className="admin-form-grid">
                    <div className="admin-field-group">
                      <label className="auth-label" htmlFor={`admin-sector-name-${sector.id}`}>
                        {adminSectorLabel}
                      </label>
                      <input
                        id={`admin-sector-name-${sector.id}`}
                        className="auth-input"
                        name="name"
                        value={sector.name}
                        onChange={(event) => handleSectorFormChange(sector.id, event)}
                      />
                    </div>

                    <div className="admin-field-group">
                      <label className="auth-label" htmlFor={`admin-sector-price-${sector.id}`}>
                        {adminPriceLabel}
                      </label>
                      <input
                        id={`admin-sector-price-${sector.id}`}
                        className="auth-input"
                        type="number"
                        step="1"
                        name="price"
                        value={sector.price}
                        onChange={(event) => handleSectorFormChange(sector.id, event)}
                      />
                    </div>

                    <div className="admin-field-group">
                      <label className="auth-label" htmlFor={`admin-sector-capacity-${sector.id}`}>
                        {adminCapacityLabel}
                      </label>
                      <input
                        id={`admin-sector-capacity-${sector.id}`}
                        className="auth-input"
                        type="number"
                        step="1"
                        name="capacity"
                        value={sector.capacity}
                        onChange={(event) => handleSectorFormChange(sector.id, event)}
                      />
                      <p className="admin-helper admin-helper-note">
                        {adminCapacityHelper}
                      </p>
                    </div>

                    <div className="admin-field-group">
                      <label className="auth-label" htmlFor={`admin-sector-rows-${sector.id}`}>
                        {adminRowCountLabel}
                      </label>
                      <input
                        id={`admin-sector-rows-${sector.id}`}
                        className="auth-input"
                        type="number"
                        step="1"
                        name="rowCount"
                        value={sector.rowCount}
                        onChange={(event) => handleSectorFormChange(sector.id, event)}
                      />
                      <p className="admin-helper admin-helper-note">
                        {adminRowCountHelper}
                      </p>
                    </div>

                    <div className="admin-field-group">
                      <label className="auth-label" htmlFor={`admin-sector-seats-${sector.id}`}>
                        {adminSeatsPerRowLabel}
                      </label>
                      <input
                        id={`admin-sector-seats-${sector.id}`}
                        className="auth-input"
                        type="number"
                        step="1"
                        name="seatsPerRow"
                        value={sector.seatsPerRow}
                        onChange={(event) => handleSectorFormChange(sector.id, event)}
                      />
                      <p className="admin-helper admin-helper-note">
                        {adminSeatsPerRowHelper}
                      </p>
                    </div>
                  </div>
                </article>
              ))}
            </div>

            <div className="admin-action-row">
              <button
                type="button"
                className="button button-secondary admin-sector-add"
                onClick={handleAddSector}
                disabled={!canAddSectors}
              >
                {adminAddSector}
              </button>
            </div>

            {!canAddSectors ? (
              <p className="admin-helper admin-helper-warning">
                {adminMaxSectorsReached}
              </p>
            ) : null}

            {submitError ? (
              <p className="auth-message auth-message-error">{submitError}</p>
            ) : null}
            {submitMessage ? (
              <p className="auth-message auth-message-success">{submitMessage}</p>
            ) : null}

            <button
              type="submit"
              className="button button-primary purchase-continue-button admin-submit-button"
              disabled={isSubmitting}
            >
              {isSubmitting ? adminCreatingBundle : adminCreateBundleButton}
            </button>
          </form>
        </article>
      </section>
    </AppShell>
  );
}

export default Admin;
