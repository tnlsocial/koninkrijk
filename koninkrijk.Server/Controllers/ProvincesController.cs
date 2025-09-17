using koninkrijk.Server.Data;
using koninkrijk.Server.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Numerics;

namespace koninkrijk.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProvincesController : ControllerBase
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

        public ProvincesController(DataContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetProvinces()
        {
            if (!_context.Provinces.Any())
            {
                return NotFound(new { message = "Geen provincies gevonden" });
            }

            var provinces = await _context.Provinces.Include(p => p.Player).ToListAsync();
            return Ok(provinces);
        }

        [Authorize]
        [HttpGet("{name}")]
        public async Task<IActionResult> GetProvince(string name)
        {
            Province province = await _context.Provinces.Include(p => p.Player).FirstOrDefaultAsync(province => province.Name == name);
            if (province == null)
            {
                return NotFound(new { message = "Provincie niet gevonden" });
            }

            return Ok(province);
        }

        //var playerProvince = await _context.PlayerProvince.Include(p => p.Player).Include(p => p.Province).Where(p => p.Player.Id == player.Id).ToListAsync();
        //    foreach(PlayerProvince pp in playerProvince)
        //    {
        //        pp.CanGuess();
        //        Trace.WriteLine($"Refreshed tries for {pp.Player.Name} - {pp.Province.Name}");
        //    }

        [Authorize]
        [HttpGet("{name}/refresh")]
        public async Task<IActionResult> Refresh(string name)
        {
            var player = _currentPlayer;
            if (player == null)
            {
                return Unauthorized();
            }

            Province province = await _context.Provinces.Include(p => p.Player).FirstOrDefaultAsync(province => province.Name == name);

            if (province == null)
            {
                return NotFound(new { message = "Deze provincie bestaat niet in het systeem" });
            }

            var playerProvince = await _context.PlayerProvince.Include(p => p.Player)
                                                              .Include(p => p.Province)
                                                              .FirstOrDefaultAsync(p => p.PlayerId == player.Id && p.Province.Name == name);

            if(playerProvince != null)
            {
                playerProvince.CanGuess();
                await _context.SaveChangesAsync();
                return Ok(new { message = $"Pogingen voor {playerProvince.Player.Name} in {playerProvince.Province.Name} ververst" });
            } 
            else
            {
                PlayerProvince newPlayerProvince = new PlayerProvince() { Player = player, Province = province, Tries = 0 };
                await _context.PlayerProvince.AddAsync(newPlayerProvince);
                await _context.SaveChangesAsync();
                return Ok(new { message = $"Pogingen voor {newPlayerProvince.Player.Name} in {newPlayerProvince.Province.Name} aangemaakt" });

            }
        }

        [Authorize]
        [HttpGet("{name}/tries")]
        public async Task<IActionResult> GetTries(string name)
        {
            var player = _currentPlayer;
            if (player == null)
            {
                return Unauthorized();
            }

            Province province = await _context.Provinces.Include(p => p.Player).FirstOrDefaultAsync(province => province.Name == name);

            if(province == null)
            {
                return BadRequest(new { message = "Deze provincie bestaat niet in het systeem" });
            }

            var playerProvince = await _context.PlayerProvince.Include(p => p.Player).Include(p => p.Province).FirstOrDefaultAsync(p => p.Player.Id == player.Id && p.Province.Id == province.Id);

            if (playerProvince == null) {
                return Ok(new { message = "Er zijn nog geen pogingen gedaan op deze provincie door deze speler." });
            }

            return Ok(new
            {
                player = playerProvince.Player.Name,
                province = playerProvince.Province.Name,
                tries = playerProvince.Tries
            }); ;
        }
    }
}
