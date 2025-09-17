import { Route } from "wouter";
import Nav from "./components/nav.component";
import Home from "./components/home.component";
import Login from "./components/login.component";
import Player from "./components/player.component";
import Register from "./components/register.component";
import Scoreboard from "./components/scoreboard.component";
import Provinces from "./components/provinces.component";
import ProvinceDetail from "./components/provincedetail.component";
import Logs from "./components/logs.component";
import Info from "./components/info.component";
import { useAuth } from "./services/AuthProvider";

function App() {
  const { auth } = useAuth();

  return (
    <div className="App">
      {auth && <Nav />}
      <div className="container">
        <Route path="/" component={Home} />
        <Route path="/player" component={Player} />
        <Route path="/register" component={Register} />
        <Route path="/login" component={Login} />
        <Route path="/scoreboard" component={Scoreboard} />
        <Route path="/provinces" component={Provinces} />
        <Route path="/provinces/:provinceName" component={ProvinceDetail} />
        <Route path="/logs" component={Logs} />
        <Route path="/info" component={Info} />
      </div>
    </div>
  );
}

export default App;
