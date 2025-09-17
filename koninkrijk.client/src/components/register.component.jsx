import { useState, Fragment } from "react";
import { useLocation } from "wouter";
import ApiService from "../services/api.service";
import { useAuth } from "../services/AuthProvider";

const Register = () => {
  const { setAuth } = useAuth();
  const [nickname, setNickname] = useState("");
  const [successful, setSuccessful] = useState(false);
  const [message, setMessage] = useState("");
  const [, setLocation] = useLocation();

  const onChangeNickname = (e) => {
    setNickname(e.target.value);
  };

  const handleRegister = async (e) => {
    e.preventDefault();

    setMessage("");
    setSuccessful(false);

    try {
      const response = await ApiService.registerPlayer(nickname);
      sessionStorage.setItem("apiKey", response.apiKey);
      setMessage(response.message);
      setSuccessful(true);
      setAuth(true);
      setLocation("/provinces");
    } catch (error) {
      const resMessage = error.message || error.toString();
      setMessage(resMessage);
      setSuccessful(false);
    }
  };

  return (
    <Fragment>
      <form className="mx-auto col-4" onSubmit={handleRegister}>
          <input
            type="text"
            className="form-control"
            name="nickname"
            placeholder="Nickname"
            value={nickname}
            onChange={onChangeNickname}
          />
          <button type="submit" className="btn btn-primary mb-3">
            Registreren
          </button>
      </form>
      {message && (
        <div className="mx-auto col-4 form-group">
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

export default Register;
