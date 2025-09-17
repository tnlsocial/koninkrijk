import { Fragment, useEffect, useState } from 'react';
import apiService from '../services/api.service';
import useAuthRedirect from "../hooks/authRedirect";

const Scoreboard = () => {
  useAuthRedirect();

  const [scoreboard, setScoreboard] = useState(null);

  useEffect(() => {
    apiService.getScoreboard()
      .then((response) => {
        setScoreboard(response);
      })
      .catch((error) => {
        console.error("An error occurred while fetching the scoreboard:", error);
        setScoreboard(null);
      });
  }, []);

  return (
    <Fragment>
        <div className="col-6 mx-auto">
          <br />
            {scoreboard ? (
               <table className="table">
              <thead>
               <tr>
                   <th scope="col">Nick</th>
                   <th scope="col">Score</th>
               </tr>
               </thead>
               <tbody>
               {scoreboard.map((val, key) => {
                   return (
                       <tr key={key}>
                           <td scope="row">{val.nick}</td>
                           <td scope="row">{val.score}</td>
                       </tr>
                   )
               })}
              </tbody>
              </table>
            ) : (
              <p>No scoreboard information available.</p>
            )}
        </div>
    </Fragment>
  );
};

export default Scoreboard;
