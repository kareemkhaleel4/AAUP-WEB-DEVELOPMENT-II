import { Link, NavLink, useNavigate } from "react-router-dom";
import { authStore } from "../../store/auth.store";
import { API_BASE_URL } from "../../config";


export default function Navbar() {
  const navigate = useNavigate();
  const user = authStore.getUser();

  const logout = () => {
    authStore.logout();
    navigate("/");
  };

  return (
    <header className="navbar">
      <div className="navbar-inner">
        <Link to="/" className="logo">LinkedInLite</Link>

        <nav className="nav-links">
          <NavLink to="/">Home</NavLink>
          {user && <NavLink to="/feed">Feed</NavLink>}
          {user && <NavLink to="/profile">Profile</NavLink>}
        </nav>

        <div className="nav-actions">
          {!user && (
            <>
              <Link to="/login" className="btn btn-ghost">Sign in</Link>
              <Link to="/register" className="btn btn-primary">Join now</Link>
            </>
          )}

          {user && (
            <div className="nav-user">
              <img
                className="nav-avatar"
                src={`${API_BASE_URL}${user.profileImageUrl}`}
              />
              <span>{user.fullName}</span>
              <button onClick={logout} className="btn btn-ghost">Logout</button>
            </div>
          )}
        </div>
      </div>
    </header>
  );
}
