using koninkrijk.Server.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace koninkrijk.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LogsController : ControllerBase
    {
        private readonly DataContext _context;

        public LogsController(DataContext context)
        {
            _context = context;
        }


        [HttpGet]
        [Authorize]
        public IActionResult GetLogs(int page = 0)
        {
            if (!_context.Logs.Any())
            {
                return NotFound(new { message = "Geen logs gevonden" });
            }

            int logsPerPage = 10;
            int totalLogs = _context.Logs.Count();
            int totalPages = (int)Math.Ceiling((double)totalLogs / logsPerPage);

            var logs = _context.Logs
                .OrderByDescending(l => l.Timestamp)
                .Skip(page * logsPerPage)
                .Take(logsPerPage)
                .Select(l => new
                {
                    l.Timestamp,
                    l.Message
                })
                .ToList();

            return Ok(new
            {
                Logs = logs,
                TotalPages = totalPages
            });
        }
    }
}
