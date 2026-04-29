import { createContext } from "react";
import useAuthSession from "../hooks/useAuthSession";
import AppLoader from "../components/ui/AppLoader";

const AuthContext = createContext(null);
AuthContext.displayName = "AuthContext";

export function AuthProvider({ children }) {
  const value = useAuthSession();

  if (!value.isReady) {
    return <AppLoader />;
  }

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
}

export { AuthContext };
