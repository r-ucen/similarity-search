using Microsoft.AspNetCore.Identity;
using StockMapSvelte.Application.Abstractions;

namespace StockMapSvelte.Infrastructure.Identity;

public class IdentityService : IIdentityService
{
    private readonly SignInManager<ApplicationUser> _signInManager;

    public IdentityService(SignInManager<ApplicationUser> signInManager)
    {
        _signInManager = signInManager;
    }
    
    public async Task LogOutAsync()
    { 
        await _signInManager.SignOutAsync();
    }
}