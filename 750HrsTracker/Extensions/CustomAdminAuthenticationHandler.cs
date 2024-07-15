using _750HrsTracker.Helpers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace _750HrsTracker.Extensions
{
    public class CustomAdminAuthenticationHandler : AuthenticationHandler<CustomAdminAuthenticationSchemeOption>
    {
        private ILoggerFactory _logger;
        private readonly AppSettings _appSettings;

        public CustomAdminAuthenticationHandler(
            IOptionsMonitor<CustomAdminAuthenticationSchemeOption> options,
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

            if (!Request.Headers.ContainsKey("a-auth"))
                return AuthenticateResult.Fail("Missing Authorization Header");

            try
            {
                var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["a-auth"]);
                authValue = authHeader.ToString();
            }
            catch
            {
                return AuthenticateResult.Fail("Invalid Authorization Header");
            }

            if (string.IsNullOrEmpty(authValue) || authValue != _appSettings.AdminApiAccessKey)
                return AuthenticateResult.Fail("Invalid Key");

            var claims = new[] {
                new Claim(ClaimTypes.NameIdentifier, "ADMIN API"),
                new Claim(ClaimTypes.Name, "ADMIN API"),
            };
            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return AuthenticateResult.Success(ticket);
        }
    }

    public class CustomAdminAuthenticationSchemeOption
        : AuthenticationSchemeOptions
    {
        public const string Name = "CustomAdminAuthenticationSchemeOption";
    }
}
