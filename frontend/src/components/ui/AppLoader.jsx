function AppLoader() {
  return (
    <div className="app-loader app-loader-inline" role="status" aria-live="polite">
      <div className="app-loader-card">
        <span className="app-loader-badge">TicketUnaj</span>
        <div className="app-loader-spinner" aria-hidden="true" />
        <h1 className="app-loader-title">Cargando</h1>
        <p className="app-loader-copy">
          Preparando tu acceso a eventos, sectores y entradas.
        </p>
      </div>
    </div>
  );
}

export default AppLoader;
