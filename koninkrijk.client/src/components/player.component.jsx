import { Fragment, useEffect, useState } from 'react';
import apiService from '../services/api.service';
import useAuthRedirect from "../hooks/authRedirect";

const Player = () => {
  useAuthRedirect();

  const currentApiKey = apiService.getCurrentApiKey();
  const [playerInfo, setPlayerInfo] = useState(null);

  useEffect(() => {
    if (currentApiKey) {
      apiService.getPlayerInfo()
        .then((response) => {
          setPlayerInfo(response);
        })
        .catch((error) => {
          console.error("An error occurred while fetching player info:", error);
          setPlayerInfo(null);
        });
    }
  }, [currentApiKey]);

  return (
    <Fragment>
      <div className="mx-auto col-6">
        <br/>
        {playerInfo ? (
          <div>
            <p><strong>Naam:</strong> {playerInfo.name}</p>
            <p><strong>Laatste veroverpoging:</strong> {new Date(playerInfo.lastCaptureTry).toLocaleString()}</p>
            <p><strong>API Key:</strong> {currentApiKey}</p>
            <p><strong>Huidige provincie in bezit:</strong> {playerInfo.province === null ? 'Geen provincie' : playerInfo.province.name}</p>
          </div>
        ) : (
          <p>No player information available.</p>
        )}
      </div>
    </Fragment>
  );
};

export default Player;
