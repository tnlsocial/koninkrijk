import { Fragment } from 'react';
import Register from './register.component';
import Login from './login.component';
import { useAuth } from "../services/AuthProvider";
import { useLocation } from "wouter";

const Home = () => {
  const { auth } = useAuth();
  const [, setLocation] = useLocation();

  if (auth) {
    setLocation("/provinces");
  }

  return (
    <Fragment>
          <div className="mx-auto col-6 text-center">
            <br />
            <h2 className="display-4">Welkom in het koninkrijk</h2>
            <br />
            <Login />
            <br />
            <Register />
          </div>
    </Fragment>
  );
};

export default Home;
