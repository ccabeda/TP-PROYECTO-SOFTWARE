import { BrowserRouter, Routes, Route, Navigate } from "react-router-dom";
import Home from "./pages/Home";
import Eventos from "./pages/Eventos";
import Evento from "./pages/Evento";
import Purchase from "./pages/Purchase";
import Checkout from "./pages/Checkout";
import MyTickets from "./pages/MyTickets";
import Login from "./pages/Login";
import Register from "./pages/Register";

const ROUTES = {
  home: "/",
  events: "/events",
  event: "/event/:id",
  purchase: "/event/:id/purchase",
  checkout: "/event/:id/checkout",
  myTickets: "/my-tickets",
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
  { path: ROUTES.login, element: <Login /> },
  { path: ROUTES.register, element: <Register /> },
];

function App() {
  return (
    <BrowserRouter>
      <Routes>
        {appRoutes.map((route) => (
          <Route key={route.path} path={route.path} element={route.element} />
        ))}
        <Route path="*" element={<Navigate to={ROUTES.home} replace />} />
      </Routes>
    </BrowserRouter>
  );
}

export default App;
