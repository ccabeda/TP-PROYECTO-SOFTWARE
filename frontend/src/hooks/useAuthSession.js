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

  useEffect(() => {
    let isMounted = true;

    async function syncCurrentUser() {
      const currentSession = getSession();
      if (!currentSession?.token) {
        if (isMounted) {
          setSession(null);
        }
        return;
      }

      try {
        const currentUser = await getCurrentUser();
        if (!currentUser) {
          if (isMounted) {
            setSession(null);
          }
          return;
        }

        if (isMounted) {
          setSession(mergeSessionWithCurrentUser(currentSession, currentUser));
        }
      } catch {
        logoutUser();
        if (isMounted) {
          setSession(null);
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
    logoutUser();
    setSession(null);
  }

  return {
    session,
    setSession: saveSession,
    clearSession,
  };
}

export default useAuthSession;
