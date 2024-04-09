using _750HrsTracker.Helpers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace _750HrsTracker.Extensions
{
    public class CustomPubAccessAuthenticationHandler : AuthenticationHandler<CustomPubAccessAuthenticationSchemeOption>
    {
        private ILoggerFactory _logger;
        private readonly AppSettings _appSettings;

        public CustomPubAccessAuthenticationHandler(
            IOptionsMonitor<CustomPubAccessAuthenticationSchemeOption> options,
            IOptionsSnapshot<AppSettings> appSettings,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
            _logger = logger;
            _appSettings = appSettings.Value;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            // skip authentication if endpoint has [AllowAnonymous] attribute
            var endpoint = Context.GetEndpoint();
            string authValue = "";
            if (endpoint?.Metadata?.GetMetadata<IAllowAnonymous>() != null)
                return AuthenticateResult.NoResult();

            if (!Request.Headers.ContainsKey("pub-access-key"))
                return AuthenticateResult.Fail("Missing Authorization Header");

            try
            {
                var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["pub-access-key"]);
                authValue = authHeader.ToString();
            }
            catch
            {
                return AuthenticateResult.Fail("Invalid Authorization Header");
            }

            if (string.IsNullOrEmpty(authValue) || authValue != _appSettings.PublicApiAccessKey)
                return AuthenticateResult.Fail("Invalid Access Key");

            var claims = new[] {
                new Claim(ClaimTypes.NameIdentifier, "Public API"),
                new Claim(ClaimTypes.Name, "Public API"),
            };
            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return AuthenticateResult.Success(ticket);
        }
    }

    public class CustomPubAccessAuthenticationSchemeOption
        : AuthenticationSchemeOptions
    {
        public const string Name = "CustomPubAccessAuthenticationSchemeOption";
    }
}
