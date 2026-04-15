using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace GameStore.IntegrationTests.AuthHandler
{
    //AuthenticationHandler<TOptions> is the base class for all the Authentication handler in asp.net.
    //AuthenticationSchemeOptions: This is the simplest configuration object. Since this is a hard-coded test handler,
    //we don't need complex settings (like Secret Keys or Issuer URLs), so we use the default options.
    public class TestAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {        
        public TestAuthHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder) : base(options, logger, encoder)
        {
            //base constructor of AuthenticationHandler

            //IOptionsMonitor: Tracks changes to the authentication options.
            //ILoggerFactory: Provides logging capabilities to the handler (e.g., logging when a user fails to authenticate).
            //UrlEncoder: Used if the handler needs to parse or generate URLs.
        }


        //This is the "heart" of the handler. Every time a request hits an endpoint protected by this scheme,
        //the framework calls this method to ask: "Who is this user, and are they allowed in?"
        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            // Seed the test user with all roles and permissions expected by the policies
            var claims = new[]
            {
            new Claim(ClaimTypes.Name, "TestUser"),
            new Claim(ClaimTypes.Role, "Admin"),
            new Claim("Permission", "Create"),
            new Claim("Permission", "Update"),
            new Claim("Permission", "Delete")
            };

            var identity = new ClaimsIdentity(claims, "Test");
            var principal = new ClaimsPrincipal(identity);

            //It represents a successful "log in" for this specific request.
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            var result = AuthenticateResult.Success(ticket);

            return Task.FromResult(result);
        }
    }
}
