import { BrowserRouter, Routes, Route, Navigate } from "react-router-dom";
import Landing from "./pages/Landing";
import Login from "./pages/Login";
import Register from "./pages/Register";
import Feed from "./pages/Feed";
import Profile from "./pages/Profile";
import Navbar from "./components/layout/Navbar";
import { authStore } from "./store/auth.store";
import "./styles/app.css";

function Protected({ children }) {
  return authStore.isAuthenticated()
    ? children
    : <Navigate to="/login" replace />;
}

export default function App() {
  const isAuth = authStore.isAuthenticated();

  return (
    <BrowserRouter>
      <Navbar />
      <div className="container">
        <Routes>

          {/* ROOT */}
          <Route
            path="/"
            element={
              isAuth
                ? <Navigate to="/feed" replace />
                : <Landing />
            }
          />

          {/* AUTH */}
          <Route path="/login" element={<Login />} />
          <Route path="/register" element={<Register />} />

          {/* PROTECTED */}
          <Route
            path="/feed"
            element={
              <Protected>
                <Feed />
              </Protected>
            }
          />

          <Route
            path="/profile"
            element={
              <Protected>
                <Profile />
              </Protected>
            }
          />

        </Routes>
      </div>
    </BrowserRouter>
  );
}
