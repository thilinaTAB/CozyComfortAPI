// CozyComfortAPI/Auth/ApiKeyAuthenticationHandler.cs
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration; // Added for IConfiguration

namespace CozyComfortAPI.Auth
{
    public class ApiKeyAuthenticationOptions : AuthenticationSchemeOptions
    {
        public const string DefaultScheme = "ApiKey";
        public string HeaderName { get; set; } = "X-API-KEY";
    }

    public class ApiKeyAuthenticationHandler : AuthenticationHandler<ApiKeyAuthenticationOptions>
    {
        private readonly IConfiguration _configuration;

        public ApiKeyAuthenticationHandler(
            IOptionsMonitor<ApiKeyAuthenticationOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IConfiguration configuration)
            : base(options, logger, encoder, clock)
        {
            _configuration = configuration;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey(Options.HeaderName))
            {
                return AuthenticateResult.Fail($"Missing Header: {Options.HeaderName}");
            }

            var extractedApiKey = Request.Headers[Options.HeaderName].ToString();

            var manufacturerApiKey = _configuration.GetValue<string>("ApiKeys:CozyComfortManufacturerKey");
            var distributorKey = _configuration.GetValue<string>("ApiKeys:CozyComfortDistributorKey");
            var sellerKey = _configuration.GetValue<string>("ApiKeys:CozyComfortSellerKey");

            if (extractedApiKey == manufacturerApiKey ||
                extractedApiKey == distributorKey ||
                extractedApiKey == sellerKey)
            {
                var claims = new List<Claim> { new Claim(ClaimTypes.Name, "ApiKeyUser") };

                if (extractedApiKey == manufacturerApiKey) claims.Add(new Claim(ClaimTypes.Role, "Manufacturer"));
                if (extractedApiKey == distributorKey) claims.Add(new Claim(ClaimTypes.Role, "Distributor"));
                if (extractedApiKey == sellerKey) claims.Add(new Claim(ClaimTypes.Role, "Seller"));

                var identity = new ClaimsIdentity(claims, Scheme.Name);
                var principal = new ClaimsPrincipal(identity);
                var ticket = new AuthenticationTicket(principal, Scheme.Name);

                return AuthenticateResult.Success(ticket);
            }

            return AuthenticateResult.Fail("Invalid API Key.");
        }
    }
}