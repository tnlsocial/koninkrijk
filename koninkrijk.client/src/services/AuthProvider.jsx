import { createContext, useContext, useEffect, useState } from "react";
import apiService from "./api.service";
import PropTypes from "prop-types";

const AuthContext = createContext({
  auth: null,
  setAuth: () => {},
  user: null,
});

export const useAuth = () => useContext(AuthContext);

const AuthProvider = ({ children }) => {
  AuthProvider.propTypes = {
    children: PropTypes.node.isRequired,
  };

  const [auth, setAuth] = useState(() => {
    const storedAuth = sessionStorage.getItem("auth");
    return storedAuth ? JSON.parse(storedAuth) : null;
  });
  const [user, setUser] = useState(null);

  useEffect(() => {
    const isAuth = async () => {
      try {
        const res = await apiService.login();
        const usr = await apiService.getPlayerInfo();
        setUser(usr);
      } catch (error) {
        setUser(null);
      }
    };

    if (auth) {
      isAuth();
    }
  }, [auth]);

  useEffect(() => {
    sessionStorage.setItem("auth", JSON.stringify(auth));
  }, [auth]);

  return (
    <AuthContext.Provider value={{ auth, setAuth, user }}>
      {children}
    </AuthContext.Provider>
  );
};

export default AuthProvider;
