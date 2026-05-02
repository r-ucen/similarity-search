using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using SimilaritySearch.Application.Abstractions;
using SimilaritySearch.Application.DTOs;

namespace SimilaritySearch.Api.Controllers;

[ApiController]
[Route("ads")]
public class AdController : Controller
{
    private readonly IAdService _adService;
    
    public AdController(IAdService adService)
    {
        _adService = adService;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllAds()
    {
        var ads = await _adService.GetAllAdsAsync();
        return Ok(ads);
    }
    
    [HttpGet]
    [Route("{id:guid}")]
    public async Task<IActionResult> GetAd(Guid id)
    {
        var ad = await _adService.GetAdAsync(id);
        return Ok(ad);
    }
    
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateAd(CreateAdCommand ad)
    {
        var createdAd = await _adService.CreateAdAsync(ad);
        
        return CreatedAtAction(
            nameof(GetAd),
            new { id = createdAd.Id },
            createdAd);
    }
    
    [Authorize]
    [HttpPut]
    [Route("{id:guid}")]
    public async Task<IActionResult> EditAd(Guid id, EditAdCommand ad)
    {
        var editedAd = await _adService.EditAdAsync(id, ad);
        return Ok(editedAd);
    }

    [Authorize]
    [HttpDelete]
    [Route("{id:guid}")]
    public async Task<IActionResult> DeleteAd(Guid id)
    {
        await _adService.DeleteAdAsync(id);
        return NoContent();
    }
}