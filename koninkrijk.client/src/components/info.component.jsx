import { Fragment } from 'react';
import useAuthRedirect from "../hooks/authRedirect";

const Info = () => {
  useAuthRedirect();

  return (
    <Fragment>
          <div className="mx-auto col-6">
            <br />
            <h4>Info</h4>
            <p>Welkom bij <b>koninkrijk</b>, het wordle meets capture-the-flag spelletje.</p>
            <p>De spelregels zijn simpel, en kunnen nog veranderen:</p>
            <ul>
              <li>Elke speler krijgt een aantal pogingen om een woord te raden op een provincie</li>
              <li>Bij het goed raden van een woord, wordt een provincie veroverd. </li>
              <li>Punten worden uitgedeeld adhv het aantal minuten dat de provincie is vastgehouden * de woordgrootte. 
                De punten worden uitgedeeld op het moment dat de provincie wordt verlaten door de speler of als hij wordt veroverd door een andere speler.</li>
              <li>Er kan één provincie worden vastgehouden per speler.</li>
            </ul>


            <h4>Feature requests/TODO</h4>
            <ul>
            <del><li>Improvement: Betere manier van omgaan met pogingen, bijv. vast aantal pogingen per provincie</li></del>
                <ul>
                  <li>Pogingen globaal naar 6 gezet, provincies hebben nu individueel pogingen i.p.v. gedeeld</li>
                  <del><li>Voor nu pogingen omhoog gezet naar 10 voor alle provincies, later reworken naar iets beters</li></del>
                </ul>
                <del><li>Bug: Off-by-one op aantal rijen + lege rij bij geen pogingen</li></del>
                <del><li>Improvement: Pagination toegevoegd aan het logboek voor volledige logging voor spelers</li></del>
                <del><li>Bug: Puntentelling gaat nog niet goed</li></del>
                <del><li>Improvement: Geen redirect nadat pogingen op zijn</li></del>
                <del><li>Improvement: Woorden die niet bestaan kunnen gebruikt worden, wordt nu gecheckt adhv een <a target="_blank" rel="noopener noreferrer" href="https://github.com/OpenTaal/opentaal-hunspell">woordenboekje</a></li></del>
                <del><li>Bug: Letters die dubbel geraden worden, worden onterecht gemarkeerd als absent</li></del>
                <del><li>Feature: Authentiek wordle toetsenbordje</li></del>
                <ul>
                  <del><li>Toetsenbordje toegevoegd, volgende stap is aanhaken van events zodat je ermee kunt typen + dat formulier weghalen</li></del>
                  <del><li>Todo: Input formulier weghalen en input direct in de bovenliggende table laten zien</li></del>
                </ul>
                <li><del>Bug: Geen redirect naar de homepage bij bezoeken van unauthenticated pagina&apos;s</del></li>
                <li><del>Improvement: Niet kunnen submitten van dubbele woorden</del></li>
                <li><del>Bug: Whitespace als nickname</del></li>
                <li><del>Bug: Punten voor de score werden niet correct bijgewerkt</del></li>
                <li><del>Improvement: Score inzichtelijk maken op kaart</del></li>
            </ul>
          </div>
    </Fragment>
  );
};

export default Info;
