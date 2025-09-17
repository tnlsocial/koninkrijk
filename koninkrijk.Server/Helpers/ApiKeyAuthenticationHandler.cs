using koninkrijk.Server.Data;
using koninkrijk.Server.Helpers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace koninkrijk.Server.Helpers
{
    public class ApiKeyAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly DataContext _context;

        public ApiKeyAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            DataContext context)
            : base(options, logger, encoder, clock)
        {
            _context = context;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (Request.Method.Equals("OPTIONS", StringComparison.OrdinalIgnoreCase))
            {
                return AuthenticateResult.NoResult();
            }

            if (!Request.Headers.TryGetValue("Authorization", out var apiKeyValues))
            {
                return AuthenticateResult.Fail("API key not found");
            }

            var apiKey = apiKeyValues.ToString();

            if (await IsValidApiKey(apiKey))
            {
                var claims = new[] { new Claim(ClaimTypes.Name, "ApiKeyUser") };

                var identity = new ClaimsIdentity(claims, Scheme.Name);
                var principal = new ClaimsPrincipal(identity);

                var ticket = new AuthenticationTicket(principal, Scheme.Name);

                return AuthenticateResult.Success(ticket);
            }

            return AuthenticateResult.Fail("Invalid API key");
        }

        private async Task<bool> IsValidApiKey(string apiKey)
        {
            if (await _context.Players.Include(p => p.Province).FirstOrDefaultAsync(p => p.ApiKey == apiKey) != null)
            {
                return true;
            }

            return false;
        }

    }
}