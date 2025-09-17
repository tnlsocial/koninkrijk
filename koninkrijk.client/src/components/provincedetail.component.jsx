import { Fragment, useEffect, useState } from "react";
import { useLocation, useParams } from "wouter";
import apiService from "../services/api.service";
import { MAX_TRIES } from "../constants/constants";
import useAuthRedirect from "../hooks/authRedirect";
import BackSpace from "../assets/backspace.svg?react"

const ProvinceDetail = () => {
  useAuthRedirect();

  const [, setLocation] = useLocation();
  const { provinceName } = useParams();
  const [province, setProvince] = useState(null);
  const [guess, setGuess] = useState("");
  const [keyboardGuess, setKeyboardGuess] = useState([]);
  const [guessed, setGuessed] = useState([]);
  const [feedback, setFeedback] = useState([]);
  const [message, setMessage] = useState("");
  const [successful, setSuccessful] = useState(false);
  const [tries, setTries] = useState(null);
  const [canGuess, setCanGuess] = useState(false);
  const currentApiKey = apiService.getCurrentApiKey();

  apiService.refreshProvinceTries(provinceName);

  useEffect(() => {
    if (currentApiKey) {
      apiService
        .getProvince(provinceName)
        .then((response) => {
          setProvince(response);
        })
        .catch((error) => {
          console.error("An error occurred while fetching the province details:", error);
          setProvince(null);
        });

    apiService
      .getProvinceTries(provinceName)
      .then((response) => {
        setTries(response.tries);
        if (response.tries >= MAX_TRIES) {
          setCanGuess(false);
          setLocation(`/provinces/${provinceName}`);
        } else {
          setCanGuess(true);
        }
      })
      .catch((error) => {
        console.error("An error occurred while fetching province tries:", error);
        setTries(null);
      });
  }
  }, [provinceName, currentApiKey, tries, setLocation]);

  const createMockupRows = () => {
    const mockupRows = [];
    const remainingTries = (MAX_TRIES - tries) - 1;

    for (let i = feedback.length; i < remainingTries; i++) {
      const row = [];
      for (let j = 0; j < province.provinceSize; j++) {
        row.push(
          <td key={j} className="table-cell">
            <span className="letter" style={{ background: "grey" }}>
              {" "}
            </span>
          </td>
        );
      }
      mockupRows.push(<tr key={i}>{row}</tr>);
    }

    return mockupRows;
  };

  const getColor = (feedbackChar) => {
    switch (feedbackChar) {
      case 'c':
        return 'green';
      case 'p':
        return 'olive';
      case 'a':
        return '#4b4b4b';
      default:
        return 'grey';
    }
  };

  const updateKeyboard = (letter, e) => {
    e.preventDefault();
  
    if (letter === "Enter") {
      handleCapture(e);
      return;
    }
  
    if (letter === "Backspace") {
      setKeyboardGuess((prevGuess) => {
        const newGuess = prevGuess.slice(0, -1);
        setGuess(newGuess.join(""));
        return newGuess;
      });
      return;
    }
  
    if (province && keyboardGuess.length < province.provinceSize) {
      setKeyboardGuess((prevGuess) => {
        const newGuess = [...prevGuess, letter];
        setGuess(newGuess.join(""));
        return newGuess;
      });
    }
  };
  
const keyboard = () => {
  const keyboardRows = [];
  const row_1 = ['q', 'w', 'e', 'r', 't', 'y', 'u', 'i', 'o', 'p'];
  const row_2 = ['a', 's', 'd', 'f', 'g', 'h', 'j', 'k', 'l'];
  const row_3 = ['Enter', 'z', 'x', 'c', 'v', 'b', 'n', 'm', 'Backspace'];

  const rows = [row_1, row_2, row_3];

  const letterColors = {};

  guessed.forEach((guessWord, feedbackIndex) => {
    const feedbackForGuess = feedback[feedbackIndex];
    const guessLetters = guessWord.toLowerCase().split('');

    feedbackForGuess.forEach((feedbackChar, index) => {
      const guessLetter = guessLetters[index];
      const color = getColor(feedbackChar);
      if (!letterColors[guessLetter] || color === 'green') {
        letterColors[guessLetter] = color;
      }
    });
  });

  rows.forEach((row, index) => {
    const rowElements = (
      <div key={index} className="d-flex justify-content-center mb-2">
        {row.map((letter) => {
          const backgroundColor = letterColors[letter] || 'grey';
          let buttonDisplay = letter.toUpperCase();
          let buttonStyle = { minWidth: '40px', background: backgroundColor };

          if (letter === 'Backspace') {
            buttonDisplay = <BackSpace className="backspace" />;
            buttonStyle = { minWidth: '40px', background: '#ffb3ba', color: 'black' };
          }

          if (letter === 'Enter'){
            buttonStyle = { minWidth: '75px', background: '#baffc9', color: 'black' };
          }

          return (
            <button
              key={letter}
              value={letter}
              type="button"
              className="btn btn-outline-light"
              style={ buttonStyle }
              onClick={(e) => updateKeyboard(letter, e)}
            >
              {buttonDisplay}
            </button>
          );
        })}
      </div>
    );
    keyboardRows.push(rowElements);
  });

  return (
    <div className="mx-auto col-6 d-flex flex-column justify-content-center">
      {keyboardRows}
    </div>
  );
};


const handleCapture = async (e) => {
  e.preventDefault();
  if (!canGuess || guess === "") return;

  if (guessed.includes(guess)) {
    setMessage("Je hebt dit woord al geprobeerd te raden!");
    setSuccessful(false);
    return;
  }

  setMessage("");
  setSuccessful(false);

  try {
    const response = await apiService.captureProvince(province.id, guess);

    setGuessed((prevGuessed) => [...prevGuessed, guess]);
    if (response.feedback) {
      setFeedback((prevFeedback) => [...prevFeedback, response.feedback]);
    }
    if (response.capped === true) {
      setLocation("/provinces");
    }
    if (response.triesLeft === 0) {
      setCanGuess(false);
    }
    setSuccessful(true);
    setGuess("");
    setKeyboardGuess([]);
  } catch (error) {
    const resMessage = error.message || error.toString();
    setMessage(resMessage);
    setSuccessful(false);
    setGuess("");
    setKeyboardGuess([]);
  }
};


  
  return (
    <Fragment>
      <br />
      {!canGuess && (
        <div
          className="mx-auto col-6 alert alert-warning text-center"
          role="alert"
        >
          <p>Je hebt geen pogingen meer over om deze provincie te veroveren!</p>
          <p>Morgen weer een nieuwe kans!</p>
        </div>
      )}

      {message && (
        <div
          className={`mx-auto col-4 alert show alert-dismissible ${successful ? "alert-success" : "alert-danger"
            }`}
          role="alert"
        >
          {message}
        </div>
      )}
      {province && (
        <div className="mx-auto col-6 text-center">
          <h1>{province.name} ({province.provinceSize})</h1>
          <p>
            {province.currentPlayer
              ? `Bezet door: ${province.currentPlayer}`
              : ""}
          </p>
        </div>
      )}
      {province && (
        <table className="mx-auto">
          <tbody>
            {feedback &&
              feedback.map((f, feedbackIndex) => (
                <tr key={feedbackIndex}>
                  {f.map((feedbackChar, index) => {
                    const currentGuess = guessed[feedbackIndex];
                    const style = { background: getColor(feedbackChar) };

                    return (
                      <td key={index} className="table-cell">
                        <span className="letter" style={style}>
                          {currentGuess[index].toUpperCase()}
                        </span>
                      </td>
                    );
                  })}
                </tr>
              ))}
            <tr>
              {guess.split("").map((char, index) => (
                <td key={index} className="table-cell">
                  <span className="letter" style={{ background: "grey" }}>
                    {char.toUpperCase()}
                  </span>
                </td>
              ))}

              { canGuess &&
              [...Array(province.provinceSize - guess.length)].map((_, index) => (
                <td key={index + guess.length} className="table-cell">
                  <span className="letter" style={{ background: "grey" }}>
                    {""}
                  </span>
                </td>
              ))
              }
            </tr>
            {createMockupRows()}
          </tbody>
        </table>
      )}
      <br />

      {canGuess && province && (
        <>
          {keyboard()}
        </>
      )}
    </Fragment>
  );
};

export default ProvinceDetail;
