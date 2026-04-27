import { eventDetailTranslations } from "./eventDetail";
import { homeLandingTranslations } from "./homeLanding";
import { purchaseCheckoutTranslations } from "./purchaseCheckout";

export const homeTranslations = {
  es: {
    ...homeLandingTranslations.es,
    ...eventDetailTranslations.es,
    ...purchaseCheckoutTranslations.es,
  },
  en: {
    ...homeLandingTranslations.en,
    ...eventDetailTranslations.en,
    ...purchaseCheckoutTranslations.en,
  },
};
