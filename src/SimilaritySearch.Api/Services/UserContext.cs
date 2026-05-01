using System.Security.Claims;
using StockMapSvelte.Application.Abstractions;

namespace StockMapSvelte.Api.Services;

public class UserContext : IUserContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    
    public UserContext(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Task<string> GetCurrentUserIdAsync()
    {
        var user = _httpContextAccessor.HttpContext?.User;
        
        var id = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (!string.IsNullOrEmpty(id))
        {
            return Task.FromResult(id);
        }

        throw new UnauthorizedAccessException("User is not authenticated.");
    }

    public Task<bool> IsUserAuthenticatedAsync()
    {
        var user = _httpContextAccessor.HttpContext?.User;
        return Task.FromResult(user?.Identity?.IsAuthenticated ?? false);
    }
    
    public Task<bool> IsInRoleAsync(string role)
    {
        var user = _httpContextAccessor.HttpContext?.User;
        return Task.FromResult(user?.IsInRole(role) ?? false);
    }
}