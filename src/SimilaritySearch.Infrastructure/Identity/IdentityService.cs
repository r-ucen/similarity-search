using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SimilaritySearch.Application.Abstractions;
using SimilaritySearch.Application.DTOs;

namespace SimilaritySearch.Infrastructure.Identity;

public class IdentityService : IIdentityService
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;

    public IdentityService(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
    }
    
    public async Task LogOutAsync()
    { 
        await _signInManager.SignOutAsync();
    }
    
    public async Task<bool> DeleteAsync(string userId)
    {
        if (userId == null)
        {
            throw new ArgumentNullException(userId);
        }

        var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
        {
            throw new InvalidOperationException($"The user with id: {userId} was not found");
        }

        var result = await _userManager.DeleteAsync(user);
        return result.Succeeded;
    }

    public async Task<List<UserDto>> GetAllAsync()
    {
        var users = await _userManager.Users.ToArrayAsync();
        var userViewModels = new List<UserDto>();

        foreach (var user in users)
        {
            userViewModels.Add(new UserDto
            {
                Id = user.Id,
                UserName = user.UserName ?? string.Empty,
                Email = user.Email ?? string.Empty,
                Roles = await _userManager.GetRolesAsync(user)
            });
        }

        return userViewModels;
    }

    public async Task<UserDto> GetByIdAsync(string id)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id);

        if (user == null)
        {
            return new UserDto();
        }

        var vm = new UserDto
        {
            Id = user.Id,
            UserName = user.UserName ?? string.Empty,
            Email = user.Email ?? string.Empty,
            Roles = await _userManager.GetRolesAsync(user)
        };

        return vm;
    }
}