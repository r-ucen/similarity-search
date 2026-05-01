using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using SimilaritySearch.Application.Abstractions.Facades;

namespace SimilaritySearch.Api.Controllers;

[EnableRateLimiting("DataPolicy")]
[ApiController]
[Authorize]
public class UserActionController : Controller
{
    private readonly IUserFacade _userFacade;

    public UserActionController(IUserFacade userFacade)
    {
        _userFacade = userFacade;
    }
    
    [HttpPost]
    [Route("logout")]
    public async Task<IActionResult> Logout()
    {
        await _userFacade.LogOutAsync();
        return NoContent();
    }
}