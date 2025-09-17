import { Link, useLocation } from "wouter";
import torch from "../assets/torch.gif";
import { useAuth } from "../services/AuthProvider";

const Nav = () => {
  const { setAuth, user } = useAuth();
  const [, setLocation] = useLocation();

  const handleLogout = () => {
    sessionStorage.removeItem("apiKey");
    setAuth(false);
    setLocation("/");
  };

  if (!user) {
    return null;
  }

  return (
    <nav className="navbar navbar-expand-lg">
      <div className="container-fluid">
        <img src={torch} className="img-fluid" width={40} />
        <Link className="navbar-brand" to="/">
          koninkrijk v0.2
        </Link>
        <button
          className="navbar-toggler"
          type="button"
          data-bs-toggle="collapse"
          data-bs-target="#navbarNav"
          aria-controls="navbarNav"
          aria-expanded="false"
          aria-label="Toggle navigation"
        >
          <span className="navbar-toggler-icon"></span>
        </button>
        <div className="collapse navbar-collapse" id="navbarNav">
          <ul className="navbar-nav">
            <>
              <li className="nav-item">
                <Link className="nav-link" to="/provinces">
                  Kaart
                </Link>
              </li>
              <li className="nav-item">
                <Link className="nav-link" to="/player">
                  Spelerinfo
                </Link>
              </li>
              <li className="nav-item">
                <Link className="nav-link" to="/scoreboard">
                  Scoreboard
                </Link>
              </li>
              <li className="nav-item">
                <Link className="nav-link" to="/logs">
                  Logboek
                </Link>
              </li>
              <li className="nav-item">
                <Link className="nav-link" to="/info">
                  Info
                </Link>
              </li>
            </>
          </ul>
          <ul className="navbar-nav ms-auto">
            <input className="form-control" type="text" value={user.apiKey} readOnly />
            <li className="nav-item">
              <button
                className="nav-link btn btn-link"
                onClick={handleLogout}
              >
                Uitloggen
              </button>
            </li>
            <img src={torch} className="img-fluid" width={40} />
          </ul>
        </div>
      </div>
    </nav>
  );
};

export default Nav;
