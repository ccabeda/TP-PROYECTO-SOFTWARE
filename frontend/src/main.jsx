import { StrictMode } from "react";
import { QueryClientProvider } from "@tanstack/react-query";
import { createRoot } from "react-dom/client";
import "bootstrap/dist/css/bootstrap.min.css";
import "react-datepicker/dist/react-datepicker.css";
import "../css/base.css";
import "../css/layout.css";
import "../css/home.css";
import "../css/events-list.css";
import "../css/event-detail.css";
import "../css/purchase.css";
import "../css/auth.css";
import App from "./App.jsx";
import AppProviders from "./context/AppProviders.jsx";
import queryClient from "./lib/queryClient.js";

const rootElement = document.getElementById("root");

if (!rootElement) {
  throw new Error("No se encontró el elemento raíz para iniciar la aplicación.");
}

const root = createRoot(rootElement);

root.render(
  <StrictMode>
    <QueryClientProvider client={queryClient}>
      <AppProviders>
        <App />
      </AppProviders>
    </QueryClientProvider>
  </StrictMode>,
);
