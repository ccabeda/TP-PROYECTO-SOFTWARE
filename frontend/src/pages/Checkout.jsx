import { useEffect, useMemo, useState } from "react";
import { useLocation, useNavigate, useParams } from "react-router-dom";
import AppShell from "../components/layout/AppShell";
import useAuthContext from "../context/useAuthContext";
import useLanguageContext from "../context/useLanguageContext";
import useToastContext from "../context/useToastContext";
import useDocumentTitle from "../hooks/useDocumentTitle";
import useEvent from "../hooks/useEvent";
import getErrorMessage from "../lib/getErrorMessage";
import { t } from "../lib/i18n";
import { formatEventDate } from "../lib/eventFormat";
import {
  createPayment,
  createReservation,
  getReservationById,
} from "../services/checkoutService";

function getPriceLocale(language) {
  return language === "en" ? "en-US" : "es-AR";
}

function buildCheckoutSummary(checkoutState) {
  if (!checkoutState?.seatId || !checkoutState?.sectorId) {
    return null;
  }

  return {
    sectorId: checkoutState.sectorId,
    sectorName: checkoutState.sectorName,
    seatId: checkoutState.seatId,
    reservationId: checkoutState.reservationId ?? null,
    expiresAt: checkoutState.expiresAt ?? null,
    seatLabel: checkoutState.seatLabel,
    basePrice: checkoutState.basePrice ?? 0,
    serviceFee: checkoutState.serviceFee ?? 0,
    finalPrice: checkoutState.finalPrice ?? 0,
  };
}

function getEventTitle(event) {
  return event?.titulo || event?.title || event?.name || "";
}

function formatRemainingTime(remainingMs) {
  const totalSeconds = Math.max(0, Math.ceil(remainingMs / 1000));
  const minutes = Math.floor(totalSeconds / 60);
  const seconds = totalSeconds % 60;

  return `${String(minutes).padStart(2, "0")}:${String(seconds).padStart(2, "0")}`;
}

function Checkout() {
  const { id } = useParams();
  const navigate = useNavigate();
  const location = useLocation();
  const checkoutState = location.state ?? null;
  const { session } = useAuthContext();
  const { language } = useLanguageContext();
  const { showToast } = useToastContext();
  const { event, isLoading, error } = useEvent(id);
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [checkoutError, setCheckoutError] = useState("");
  const [successMessage, setSuccessMessage] = useState("");
  const [isReservationExpired, setIsReservationExpired] = useState(false);
  const [fallbackReservationExpiresAt, setFallbackReservationExpiresAt] = useState(null);
  const [currentTime, setCurrentTime] = useState(0);
  const checkoutPath = `/event/${id}/checkout`;
  const purchasePath = `/event/${id}/purchase`;
  const myTicketsPath = "/my-tickets";
  const priceLocale = getPriceLocale(language);
  const eventTitle = getEventTitle(event);
  const formatPrice = (value) => `$${value.toLocaleString(priceLocale)}`;
  const loadingLabel = t(language, "home.eventLoading");
  const notFoundLabel = t(language, "home.eventNotFound");
  const checkoutTitle = t(language, "home.checkoutTitle");
  const checkoutSummaryTitle = t(language, "home.checkoutSummaryTitle");
  const checkoutSummaryCopy = t(language, "home.checkoutSummaryCopy");
  const checkoutReservationTimerLabel = t(language, "home.checkoutReservationTimerLabel");
  const checkoutReservationExpiredLabel = t(language, "home.checkoutReservationExpiredLabel");
  const checkoutPaymentTitle = t(language, "home.checkoutPaymentTitle");
  const checkoutPaymentCopy = t(language, "home.checkoutPaymentCopy");
  const checkoutPaymentMethodTitle = t(language, "home.checkoutPaymentMethodTitle");
  const checkoutPaymentMethodCopy = t(language, "home.checkoutPaymentMethodCopy");
  const checkoutSuccessCopy = t(language, "home.checkoutSuccessCopy");
  const checkoutSuccessButton = t(language, "home.checkoutSuccessButton");
  const checkoutMissingSelection = t(language, "home.checkoutMissingSelection");
  const checkoutBackToPurchase = t(language, "home.checkoutBackToPurchase");
  const checkoutExpiredError = t(language, "home.checkoutExpiredError");
  const checkoutExpiredAction = t(language, "home.checkoutExpiredAction");
  const purchaseBasePriceLabel = t(language, "home.purchaseBasePriceLabel");
  const purchaseFeeLabel = t(language, "home.purchaseFeeLabel");
  const purchaseFinalPriceLabel = t(language, "home.purchaseFinalPriceLabel");
  const checkoutPayButton = t(language, "home.checkoutPayButton");
  const checkoutPayingButton = t(language, "home.checkoutPayingButton");
  const checkoutBadgeLabel = t(language, "home.checkoutBadge");
  useDocumentTitle(eventTitle || checkoutTitle);

  const summary = useMemo(() => buildCheckoutSummary(checkoutState), [checkoutState]);
  const reservationExpiresAt = fallbackReservationExpiresAt ?? summary?.expiresAt ?? null;
  const remainingReservationMs = reservationExpiresAt
    ? new Date(reservationExpiresAt).getTime() - currentTime
    : null;
  const isReservationUrgent =
    remainingReservationMs !== null &&
    remainingReservationMs > 0 &&
    remainingReservationMs <= 60_000;
  const isReservationTimeOver =
    remainingReservationMs !== null && remainingReservationMs <= 0;
  const effectiveReservationExpired = isReservationExpired || isReservationTimeOver;
  const effectiveCheckoutError =
    checkoutError || (effectiveReservationExpired ? checkoutExpiredError : "");

  useEffect(() => {
    if (!session?.token) {
      navigate("/login", { replace: true, state: { redirectTo: checkoutPath } });
    }
  }, [checkoutPath, navigate, session]);

  useEffect(() => {
    if (!session?.token || !summary?.reservationId || reservationExpiresAt) {
      return undefined;
    }

    let isMounted = true;

    async function loadReservation() {
      try {
        const reservation = await getReservationById({
          reservationId: summary.reservationId,
          token: session.token,
        });

        if (isMounted) {
          setFallbackReservationExpiresAt(reservation.expiresAt ?? null);
        }
      } catch {
        // Keep checkout usable; payment flow will surface backend validation if needed.
      }
    }

    void loadReservation();

    return () => {
      isMounted = false;
    };
  }, [reservationExpiresAt, session, summary?.reservationId]);

  useEffect(() => {
    if (!reservationExpiresAt || effectiveReservationExpired || successMessage) {
      return undefined;
    }

    const timeoutId = window.setTimeout(() => {
      setCurrentTime(Date.now());
    }, 0);

    const intervalId = window.setInterval(() => {
      setCurrentTime(Date.now());
    }, 1000);

    return () => {
      window.clearTimeout(timeoutId);
      window.clearInterval(intervalId);
    };
  }, [effectiveReservationExpired, reservationExpiresAt, successMessage]);

  async function ensureReservationId() {
    if (!session?.token || !summary) {
      return null;
    }

    if (summary.reservationId) {
      return summary.reservationId;
    }

    const reservation = await createReservation({
      seatId: summary.seatId,
      token: session.token,
    });

    return reservation.id;
  }

  async function handlePay() {
    if (!session?.token || !summary) {
      return;
    }

    setIsSubmitting(true);
    setCheckoutError("");
    setSuccessMessage("");
    setIsReservationExpired(false);

    try {
      const reservationId = await ensureReservationId();
      if (!reservationId) {
        return;
      }

      await createPayment({
        reservationId,
        token: session.token,
      });

      const successLabel = t(language, "home.checkoutSuccessMessage");
      setSuccessMessage(successLabel);
      showToast(successLabel, "success");
    } catch (paymentError) {
      const errorMessage = getErrorMessage(
        paymentError,
        t(language, "home.checkoutError"),
      );
      const expiredReservation =
        errorMessage.toLowerCase().includes("expir") &&
        errorMessage.toLowerCase().includes("reserva");

      setIsReservationExpired(expiredReservation);
      setCheckoutError(expiredReservation ? "" : errorMessage);
      showToast(
        expiredReservation ? checkoutExpiredError : errorMessage,
        expiredReservation ? "warning" : "error",
      );
    } finally {
      setIsSubmitting(false);
    }
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

  if (!summary) {
    return (
      <AppShell>
        <section className="checkout-page">
          <article className="purchase-card checkout-card">
            <h1>{checkoutTitle}</h1>
            <p className="event-detail-copy">{checkoutMissingSelection}</p>
            <button
              type="button"
              className="button button-primary purchase-continue-button"
              onClick={() => navigate(purchasePath)}
            >
              {checkoutBackToPurchase}
            </button>
          </article>
        </section>
      </AppShell>
    );
  }

  return (
    <AppShell>
      <section className="checkout-page">
        <div className="purchase-header">
          <p className="header-badge">{checkoutBadgeLabel}</p>
          <h1>{checkoutTitle}</h1>
          <p className="header-text">{eventTitle}</p>
        </div>

        <section className="checkout-layout">
          <article className="purchase-card checkout-card">
            <h2>{checkoutSummaryTitle}</h2>
            <p className="event-detail-copy">{checkoutSummaryCopy}</p>

              <div className="checkout-summary-box">
                <div className="checkout-summary-main">
                  <span className="purchase-seat-map-label">{summary.sectorName}</span>
                  <strong>{summary.seatLabel}</strong>
                  <p>{formatEventDate(event.eventDate, language)}</p>
                  <p>{event.venue || "-"}</p>
                </div>

                {reservationExpiresAt ? (
                  <div
                    className={`checkout-reservation-timer ${
                      isReservationTimeOver
                        ? "is-expired"
                        : isReservationUrgent
                          ? "is-urgent"
                          : ""
                    }`}
                  >
                    <span>{checkoutReservationTimerLabel}</span>
                    <strong>
                      {isReservationTimeOver
                        ? checkoutReservationExpiredLabel
                        : formatRemainingTime(remainingReservationMs)}
                    </strong>
                  </div>
                ) : null}

                <div className="purchase-summary-lines">
                <div className="purchase-summary-line">
                  <span>{purchaseBasePriceLabel}</span>
                  <strong>{formatPrice(summary.basePrice)}</strong>
                </div>
                <div className="purchase-summary-line">
                  <span>{purchaseFeeLabel}</span>
                  <strong>{formatPrice(summary.serviceFee)}</strong>
                </div>
                <div className="purchase-summary-line purchase-summary-line-total">
                  <span>{purchaseFinalPriceLabel}</span>
                  <strong>{formatPrice(summary.finalPrice)}</strong>
                </div>
              </div>
            </div>
          </article>

          <aside className="purchase-card checkout-card checkout-payment-card">
            <h2>{checkoutPaymentTitle}</h2>
            <p className="event-detail-copy">{checkoutPaymentCopy}</p>

            <div className="checkout-method-card is-selected">
              <span className="checkout-method-badge">OK</span>
              <div>
                <strong>{checkoutPaymentMethodTitle}</strong>
                <p>{checkoutPaymentMethodCopy}</p>
              </div>
            </div>

            {effectiveCheckoutError ? (
              <div className="checkout-error-stack">
                <p className="auth-message auth-message-error">{effectiveCheckoutError}</p>
                {effectiveReservationExpired ? (
                  <button
                    type="button"
                    className="button button-secondary checkout-expired-button"
                    onClick={() => navigate(purchasePath, { replace: true })}
                  >
                    {checkoutExpiredAction}
                  </button>
                ) : null}
              </div>
            ) : null}

            {successMessage ? (
              <div className="checkout-success-card">
                <span className="checkout-success-badge">OK</span>
                <div className="checkout-success-copy">
                  <strong>{successMessage}</strong>
                  <p>{checkoutSuccessCopy}</p>
                </div>
                <button
                  type="button"
                  className="button button-primary purchase-continue-button checkout-success-button"
                  onClick={() => navigate(myTicketsPath)}
                >
                  {checkoutSuccessButton}
                </button>
              </div>
            ) : null}

            {!successMessage ? (
              <button
                type="button"
                className="button button-primary purchase-continue-button"
                onClick={handlePay}
                disabled={isSubmitting || effectiveReservationExpired}
                title={
                  effectiveReservationExpired
                    ? checkoutReservationExpiredLabel
                    : undefined
                }
              >
                {isSubmitting ? checkoutPayingButton : checkoutPayButton}
              </button>
            ) : null}
          </aside>
        </section>
      </section>
    </AppShell>
  );
}

export default Checkout;
