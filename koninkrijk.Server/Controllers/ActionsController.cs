using koninkrijk.Server.Data;
using koninkrijk.Server.Helpers;
using koninkrijk.Server.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace koninkrijk.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ActionsController : ControllerBase
    {
        private readonly DataContext _context;
        private string _currentApiKey
        {
            get
            {
                if (Request.Headers.TryGetValue("Authorization", out var apiKeyValues))
                {
                    return apiKeyValues;
                }

                return null;
            }
        }
        private Player? _currentPlayer => _context.Players.Include(p => p.Province).FirstOrDefault(p => p.ApiKey == _currentApiKey);

        public ActionsController(DataContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpPost("capture/{id}/{guess}")]
        public async Task<IActionResult> Capture(int id, string guess)
        {
            var player = _currentPlayer;
            if (player == null) return Unauthorized();

            var province = await GetProvinceByIdAsync(id);
            if (province == null) return NotFound(new { message = "Provincie niet gevonden" });

            var playerProvince = await GetPlayerProvinceAsync(province, player);

            if (province.Player == player) return Conflict(new { message = "Provincie is al door jou in bezit." });

            if (!SpellCheck.checkWord(guess)) { return Conflict(new { message = "Dit woord komt niet voor in het woordenboek." }); }

            if (!await CanPlayerGuessAsync(playerProvince))
            {
                return Ok(new { message = "Morgen weer een nieuwe kans!", capped = false });
            }

            var word = WordGen.GenerateWord(province.ProvinceSize, DateTime.Now.Day).ToLower();
            guess = guess.ToLower().Trim();

            await UpdatePlayerWordAsync(playerProvince, word);

            if (!IsValidGuess(guess, word, province.ProvinceSize)) return BadRequest(new { message = $"Ongeldige gok! De lengte van dit woord is {word.Length}!" });

            if (guess == playerProvince.CurrentWord)
            {
                await HandleCorrectGuessAsync(player, province);
                return Ok(new { message = $"Provincie succesvol veroverd door {player.Name}", capped = true });
            }
            else
            {
                var feedback = GenerateFeedback(guess, word);
                playerProvince.CurrentFeedback = string.Join(",", feedback);
                await _context.SaveChangesAsync();
                if (playerProvince.Tries <= 0)
                {
                    return Ok(new { feedback, message = $"Helaas, je kunt niet meer gokken!", capped = false });
                }
                else
                {
                    return Ok(new
                    {
                        feedback,
                        message = $"Incorrecte gok, nog {Config.MaxTries - playerProvince.Tries} over",
                        capped = false,
                        triesLeft = Config.MaxTries - playerProvince.Tries
                    });
                }
            }
        }

        private async Task<Province> GetProvinceByIdAsync(int id)
        {
            return await _context.Provinces.Include(p => p.Player).FirstOrDefaultAsync(p => p.Id == id);
        }


        private async Task<PlayerProvince> GetPlayerProvinceAsync(Province province, Player player)
        {
            var playerProvince = await _context.PlayerProvince.Include(p => p.Player).Include(p => p.Province).FirstOrDefaultAsync(pro => pro.Province.Id == province.Id && pro.Player.Id == player.Id);

            return playerProvince;
        }


        private async Task<bool> CanPlayerGuessAsync(PlayerProvince playerProvince)
        {
            if (playerProvince.CanGuess())
            {
                playerProvince.Tries++;
                await _context.SaveChangesAsync();
                return true;
            }
            else
            {
                playerProvince.CurrentFeedback = null;
                playerProvince.CurrentWord = null;
                await _context.SaveChangesAsync();
                return false;
            }
        }

        private async Task UpdatePlayerWordAsync(PlayerProvince playerProvince, string word)
        {
            if (playerProvince.CurrentWord != word)
            {
                playerProvince.CurrentWord = word;
                await _context.SaveChangesAsync();
            }
            Trace.WriteLine($"Current word: {playerProvince.CurrentWord}");
        }

        private bool IsValidGuess(string guess, string word, int expectedLength)
        {
            return guess.Length == expectedLength && word.Length == guess.Length;
        }

        private async Task HandleCorrectGuessAsync(Player player, Province province)
        {
            if (province.Player != null)
            {
                int score = province.Score(DateTime.Now);
                province.Player.Score += score;
                _context.Logs.Add(new Log($"{province.Player.Name} is van de troon gestoten door {player.Name} en heeft {score} punten verdiend."));
                province.Player = null;
            }

            if (player.Province != null)
            {
                var prevProvince = await _context.Provinces.FirstOrDefaultAsync(p => p.Id == player.Province.Id);
                int score = prevProvince.Score(DateTime.Now);

                if (score > 0)
                {
                    prevProvince.Player.Score += score;

                    _context.Logs.Add(new Log($"{player.Name} heeft {prevProvince.Name} verlaten en heeft {score} punten verdiend."));
                }
                else
                {
                    _context.Logs.Add(new Log($"{player.Name} heeft {prevProvince.Name} verlaten."));
                }
                prevProvince.Player = null;
                player.Province = null;
                await _context.SaveChangesAsync();
            }

            province.Capture(player);
            await _context.AddAsync(new Log($"{player.Name} heeft {province.Name} veroverd!"));
            await _context.SaveChangesAsync();
        }


        private List<string> GenerateFeedback(string guess, string word)
        {
            var feedback = new List<string>();
            var wordArray = word.ToCharArray();

            for (int i = 0; i < word.Length; i++)
            {
                if (guess[i] == word[i])
                {
                    feedback.Add("c");
                    wordArray[i] = '*';
                }
                else
                {
                    feedback.Add("a");
                }
            }

            for (int i = 0; i < word.Length; i++)
            {
                if (feedback[i] == "a" && wordArray.Contains(guess[i]))
                {
                    feedback[i] = "p";
                    wordArray[Array.IndexOf(wordArray, guess[i])] = '*';
                }
            }

            return feedback;
        }
    }
}
