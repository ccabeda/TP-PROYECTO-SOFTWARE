import Topbar from "./Topbar";
import { useThemeContext } from "../../context/ThemeContext";

function AppShell({ children }) {
  const { darkMode } = useThemeContext();

  return (
    <div className={darkMode ? "page dark" : "page"}>
      <Topbar />

      <main className="content">{children}</main>
      <p className="app-signature">© 2026 TicketUnaj</p>
    </div>
  );
}

export default AppShell;
