import { useEffect } from "react";

const APP_NAME = "TicketUnaj";

function buildDocumentTitle(title) {
  const normalizedTitle = title?.trim();
  return normalizedTitle ? `${APP_NAME} | ${normalizedTitle}` : APP_NAME;
}

function useDocumentTitle(title) {
  useEffect(() => {
    document.title = buildDocumentTitle(title);
  }, [title]);
}

export default useDocumentTitle;
