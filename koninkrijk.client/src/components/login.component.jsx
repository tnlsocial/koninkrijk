import { useState, Fragment } from "react";
import { useLocation } from "wouter";
import apiService from "../services/api.service";
import { useAuth } from "../services/AuthProvider";

const Login = () => {
  const { setAuth } = useAuth();
  const [ApiKey, setApiKey] = useState("");
  const [successful, setSuccessful] = useState(false);
  const [message, setMessage] = useState("");
  const [, setLocation] = useLocation();

  const onChangeApiKey = (e) => {
    setApiKey(e.target.value);
  };

  const handleLogin = async (e) => {
    e.preventDefault();

    setMessage("");
    setSuccessful(false);

    sessionStorage.setItem("apiKey", ApiKey);
    try {
      const resp = await apiService.login();
      setAuth(true);
      setSuccessful(true);
      setLocation('/provinces');
    } catch(error) {
      sessionStorage.removeItem("apiKey");
      setMessage("Het is helaas niet gelukt om in te loggen. Probeer het opnieuw.");
      setSuccessful(false);
    }
  };

  return (
    <Fragment>
      <form className="mx-auto col-8" onSubmit={handleLogin}>
          <input
            type="text"
            className="form-control"
            name="apiKey"
            placeholder="Apikey"
            value={ApiKey}
            onChange={onChangeApiKey}
          />
          <button type="submit" className="btn btn-primary mb-3">
            Inloggen
          </button>
      </form>
      {message && (
        <div className="mx-auto col-6 form-group">
          <div
            className={`alert ${successful ? "alert-success" : "alert-danger"}`}
            role="alert"
          >
            {message}
          </div>
        </div>
      )}
    </Fragment>
  );
};

export default Login;
