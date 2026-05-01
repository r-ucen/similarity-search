using Microsoft.AspNetCore.Identity;
using SimilaritySearch.Application.Abstractions;

namespace SimilaritySearch.Infrastructure.Identity;

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