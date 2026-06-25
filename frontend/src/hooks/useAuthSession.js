import { useEffect, useState } from "react";
import {
  getCurrentUser,
  getSession,
  logoutUser,
} from "../services/authService";

function mergeSessionWithCurrentUser(currentSession, currentUser) {
  return {
    ...currentSession,
    ...currentUser,
    token: currentSession.token,
  };
}

function useAuthSession() {
  const [session, setSession] = useState(() => getSession());
  const [isReady, setIsReady] = useState(false);

  useEffect(() => {
    let isMounted = true;

    async function syncCurrentUser() {
      const currentSession = getSession();
      if (!currentSession?.token) {
        if (isMounted) {
          setSession(null);
          setIsReady(true);
        }
        return;
      }

      try {
        const currentUser = await getCurrentUser();
        if (!currentUser) {
          if (isMounted) {
            setSession(null);
            setIsReady(true);
          }
          return;
        }

        if (isMounted) {
          const latestSession = getSession() ?? currentSession;
          setSession(mergeSessionWithCurrentUser(latestSession, currentUser));
          setIsReady(true);
        }
      } catch {
        void logoutUser();
        if (isMounted) {
          setSession(null);
          setIsReady(true);
        }
      }
    }

    void syncCurrentUser();

    return () => {
      isMounted = false;
    };
  }, []);

  function saveSession(nextSession) {
    setSession(nextSession);
  }

  function clearSession() {
    void logoutUser();
    setSession(null);
  }

  return {
    isReady,
    session,
    setSession: saveSession,
    clearSession,
  };
}

export default useAuthSession;
