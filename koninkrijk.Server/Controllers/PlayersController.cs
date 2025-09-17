using koninkrijk.Server.Data;
using koninkrijk.Server.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace koninkrijk.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PlayersController : ControllerBase
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

        public PlayersController(DataContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] PlayerRegister playerRegister)
        {
            string nick = playerRegister.Nickname.Trim();
            if (string.IsNullOrWhiteSpace(nick) || string.IsNullOrEmpty(nick))
            {
                return Conflict(new { message = "Deze nickname kan niet worden gebruikt" });
            }

            Player player = new Player(nick);
            if (_context.Players.Any(p => p.Name == nick))
            {
                return Conflict(new { message = "Deze nickname bestaat al" });
            }

            List<Province> provinces = await _context.Provinces.ToListAsync();

            foreach(Province p in provinces)
            {
                PlayerProvince playerProvince = new PlayerProvince() { Player = player, Province = p, Tries = 0 };
                await _context.PlayerProvince.AddAsync(playerProvince);
            }


            await _context.Logs.AddAsync(new Log { Message = $"Onderdanen van het koninkrijk let op! {nick} is toegetreden." });
            await _context.Players.AddAsync(player);
            await _context.SaveChangesAsync();

            return Ok(new { info = player, message = "Speler succesvol aangemaakt, gebruik de ApiKey om in te loggen", apiKey = $"{player.ApiKey}" });
        }

        [Authorize]
        [HttpPost("login")]
        public async Task<IActionResult> LoginPlayer()
        {
            var player = _currentPlayer;
            if (player == null)
            {
                return Unauthorized();
            }

            return Ok(new { message = "Succesvol ingelogd" });
        }


        [Authorize]
        [HttpGet("info")]
        public async Task<IActionResult> GetPlayerInfo()
        {
            var player = _currentPlayer;
            if (player == null)
            {
                return Unauthorized();
            }

            return Ok(player);
        }

        [Authorize]
        [HttpGet("scoreboard")]
        public IActionResult GetScoreboard()
        {
            if (!_context.Players.Any())
            {
                return NotFound(new { message = "Geen spelers gevonden" });
            }

            var scoreboard = _context.Players.Select(s => new
            {
                Nick = s.Name,
                Score = s.Score
            }).OrderByDescending(s => s.Score);

            return Ok(scoreboard);
        }

        public class PlayerRegister
        {
            public string Nickname { get; set; }
        }
    }
}
