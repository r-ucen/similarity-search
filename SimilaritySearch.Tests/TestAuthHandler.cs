using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace SimilaritySearch.Tests;

public class TestAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public const string AuthenticationScheme = "Test";
    
    public TestAuthHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder) : base(options, logger, encoder) { }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, "702f52fd-f23b-462d-b84b-0e1acc17785c"),
            new Claim(ClaimTypes.Name, "admin@rucen.me"),
            new Claim(ClaimTypes.Role, "Admin")
        };

        var identity = new ClaimsIdentity(claims, AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, AuthenticationScheme);
        
        var result = AuthenticateResult.Success(ticket);
        
        return Task.FromResult(result);
    }
}