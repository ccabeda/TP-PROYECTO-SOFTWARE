import { Suspense, lazy } from "react";
import { BrowserRouter, Routes, Route, Navigate } from "react-router-dom";

const Home = lazy(() => import("./pages/Home"));
const Eventos = lazy(() => import("./pages/Eventos"));
const Evento = lazy(() => import("./pages/Evento"));
const Purchase = lazy(() => import("./pages/Purchase"));
const Checkout = lazy(() => import("./pages/Checkout"));
const MyTickets = lazy(() => import("./pages/MyTickets"));
const Login = lazy(() => import("./pages/Login"));
const Register = lazy(() => import("./pages/Register"));
const Admin = lazy(() => import("./pages/Admin"));
const AdminUsers = lazy(() => import("./pages/AdminUsers"));
const AdminAuditLogs = lazy(() => import("./pages/AdminAuditLogs"));

const ROUTES = {
  home: "/",
  events: "/events",
  event: "/event/:id",
  purchase: "/event/:id/purchase",
  checkout: "/event/:id/checkout",
  myTickets: "/my-tickets",
  admin: "/admin",
  adminUsers: "/admin/users",
  adminAuditLogs: "/admin/audit-logs",
  login: "/login",
  register: "/register",
};

const appRoutes = [
  { path: ROUTES.home, element: <Home /> },
  { path: ROUTES.events, element: <Eventos /> },
  { path: ROUTES.event, element: <Evento /> },
  { path: ROUTES.purchase, element: <Purchase /> },
  { path: ROUTES.checkout, element: <Checkout /> },
  { path: ROUTES.myTickets, element: <MyTickets /> },
  { path: ROUTES.admin, element: <Admin /> },
  { path: ROUTES.adminUsers, element: <AdminUsers /> },
  { path: ROUTES.adminAuditLogs, element: <AdminAuditLogs /> },
  { path: ROUTES.login, element: <Login /> },
  { path: ROUTES.register, element: <Register /> },
];

function App() {
  return (
    <BrowserRouter>
      <Suspense fallback={<div className="events-feedback">Cargando...</div>}>
        <Routes>
          {appRoutes.map((route) => (
            <Route key={route.path} path={route.path} element={route.element} />
          ))}
          <Route path="*" element={<Navigate to={ROUTES.home} replace />} />
        </Routes>
      </Suspense>
    </BrowserRouter>
  );
}

export default App;
