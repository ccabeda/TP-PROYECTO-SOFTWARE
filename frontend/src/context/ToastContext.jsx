import { createContext, useCallback, useMemo, useState } from "react";

const TOAST_DURATION_MS = 3600;
const TOAST_EXIT_MS = 220;

const ToastContext = createContext(null);
ToastContext.displayName = "ToastContext";

function buildToast(message, type, duration) {
  return {
    id: globalThis.crypto?.randomUUID?.() ?? `${Date.now()}-${Math.random().toString(16).slice(2)}`,
    message,
    type,
    duration,
    isClosing: false,
  };
}

function ToastProvider({ children }) {
  const [toasts, setToasts] = useState([]);

  const dismissToast = useCallback((toastId) => {
    setToasts((current) =>
      current.map((toast) =>
        toast.id === toastId ? { ...toast, isClosing: true } : toast,
      ),
    );

    window.setTimeout(() => {
      setToasts((current) => current.filter((toast) => toast.id !== toastId));
    }, TOAST_EXIT_MS);
  }, []);

  const showToast = useCallback(
    (message, type = "info", duration = TOAST_DURATION_MS) => {
      if (!message) {
        return;
      }

      const toast = buildToast(message, type, duration);
      setToasts((current) => [...current, toast]);

      if (duration > 0) {
        window.setTimeout(() => {
          dismissToast(toast.id);
        }, duration);
      }
    },
    [dismissToast],
  );

  const value = useMemo(
    () => ({
      toasts,
      showToast,
      dismissToast,
    }),
    [dismissToast, showToast, toasts],
  );

  return (
    <ToastContext.Provider value={value}>
      {children}
      <div className="toast-stack" aria-live="polite" aria-atomic="true">
        {toasts.map((toast) => (
          <div
            key={toast.id}
            className={`toast-card toast-card-${toast.type} ${
              toast.isClosing ? "is-closing" : ""
            }`}
          >
            <span className="toast-card-copy">{toast.message}</span>
            <button
              type="button"
              className="toast-card-close"
              onClick={() => dismissToast(toast.id)}
              aria-label="Dismiss"
            >
              ×
            </button>
          </div>
        ))}
      </div>
    </ToastContext.Provider>
  );
}

export { ToastContext, ToastProvider };
