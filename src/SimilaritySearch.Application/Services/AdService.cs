using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SimilaritySearch.Application.Abstractions;
using SimilaritySearch.Application.Abstractions.Repositories;
using SimilaritySearch.Application.DTOs;
using SimilaritySearch.Application.Exceptions.Ad;
using SimilaritySearch.Domain;
using SimilaritySearch.Domain.Entities;

namespace SimilaritySearch.Application.Services;

public class AdService : IAdService
{
    private readonly IUserContext _userContext;
    private readonly IAdRepository _adRepository;
    private readonly CurrencySettings _currencySettings;
    
    public AdService(IUserContext userContext, IAdRepository adRepository, CurrencySettings currencySettings)
    {
        _userContext = userContext;
        _adRepository = adRepository;
        _currencySettings = currencySettings;
    }
    
    public async Task<IEnumerable<AdDto>> GetAllAdsAsync()
    {
        return await _adRepository.GetAllAdsAsync() ?? new List<AdDto>();
    }

    public async Task<AdDto> CreateAdAsync(CreateAdCommand ad)
    {
        var missingInfo = string.IsNullOrWhiteSpace(ad.Description) ||
                           string.IsNullOrWhiteSpace(ad.Location) ||
                           ad.Price <= 0 ||
                           string.IsNullOrWhiteSpace(ad.PhoneNumber) ||
                           string.IsNullOrWhiteSpace(ad.UserName);
        
        if (missingInfo)
        {
            throw new DataMissingException("Some info is missing");
        }
        
        var userId = await _userContext.GetCurrentUserIdAsync();

        var entity = new Ad
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            UserName = ad.UserName.Trim(),
            PhoneNumber = ad.PhoneNumber.Trim(),
            Email = ad.Email?.Trim(),
            Description = ad.Description.Trim(),
            Location = ad.Location.Trim(),
            Price = ad.Price,
            Currency = _currencySettings.CurrencySymbol,
            IsReupload = false,
            ReuploadReason = null,
            IsDeleted = false
        };
        
        var result = await _adRepository.CreateAdAsync(entity);
        
        if (result <= 0)
        {
            throw new AdCreationFailedException("Failed to create ad.");
        }

        return new AdDto()
        {
            Id = entity.Id,
            UserId = entity.UserId,
            UserName = entity.UserName,
            PhoneNumber = entity.PhoneNumber,
            Email = entity.Email,
            Description = entity.Description,
            Location = entity.Location,
            Price = entity.Price,
            Currency = _currencySettings.CurrencySymbol,
            IsReupload = entity.IsReupload,
            ReuploadReason = entity.ReuploadReason
        };
    }

    public async Task<AdDto> GetAdAsync(Guid id)
    {
        var currentUserId = await _userContext.GetCurrentUserIdAsync();
        
        var ad = await _adRepository.GetAdAsync(id);

        return ad ?? throw new AdNotFoundException("Ad not found.");
    }

    public async Task<AdDto> EditAdAsync(Guid adId, EditAdCommand ad)
    {
        var currentUserId = await _userContext.GetCurrentUserIdAsync();
        
        var existing = await _adRepository.GetAdAsync(adId);
        
        if (existing == null)
        {
            throw new AdNotFoundException("Ad not found.");
        }
        
        if (existing.UserId != currentUserId)
        {
            throw new UnauthorizedAccessException($"Not authorized to edit ad with id: {adId}");
        }
        
        if (!string.IsNullOrWhiteSpace(ad.Description))
            existing.Description = ad.Description.Trim();

        if (!string.IsNullOrWhiteSpace(ad.Location))
            existing.Location = ad.Location.Trim();

        if (!string.IsNullOrWhiteSpace(ad.PhoneNumber))
            existing.PhoneNumber = ad.PhoneNumber.Trim();

        if (!string.IsNullOrWhiteSpace(ad.UserName))
            existing.UserName = ad.UserName.Trim();

        if (ad.Price > 1)
            existing.Price = ad.Price;

        if (ad.Email != null)
            existing.Email = ad.Email.Trim();
        
        var result = await _adRepository.UpdateAdAsync(adId, ad);

        if (result > 0)
        {
            return new AdDto()
            {
                Id = adId,
                UserId = existing.UserId,
                UserName = existing.UserName.Trim(),
                PhoneNumber = existing.PhoneNumber.Trim(),
                Email = existing.Email?.Trim(),
                Description = existing.Description.Trim(),
                Location = existing.Location.Trim(),
                Price = existing.Price,
                Currency = _currencySettings.CurrencySymbol,
                IsReupload = existing.IsReupload,
                ReuploadReason = existing.ReuploadReason
            };
        }
        
        throw new AdEditFailedException("Failed to edit ad.");
    }

    public async Task DeleteAdAsync(Guid adId)
    {
        var currentUserId = await _userContext.GetCurrentUserIdAsync();
        
        var existing = await _adRepository.GetAdAsync(adId);
        if (existing == null)
        {
            throw new AdNotFoundException("Ad not found.");
        }

        if (existing.UserId != currentUserId)
        {
            throw new UnauthorizedAccessException($"Not authorized to delete ad with id: {adId}");
        }
        
        var result = await _adRepository.DeleteAdAsync(adId);
        if (result <= 0)
        {
            throw new AdDeleteFailedException("Failed to delete ad.");
        }
    }
}