using SimilaritySearch.Application.Abstractions;
using SimilaritySearch.Application.DTOs;
using SimilaritySearch.Application.Exceptions.Ad;
using SimilaritySearch.Application.Extensions.Ad;
using SimilaritySearch.Domain;
using SimilaritySearch.Domain.Entities;

namespace SimilaritySearch.Application.Services;

public class AdService : IAdService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserContext _userContext;
    private readonly CurrencySettings _currencySettings;
    private readonly IBackgroundJobService _backgroundJobService;
    
    public AdService(IUnitOfWork unitOfWork, IUserContext userContext, CurrencySettings currencySettings, IBackgroundJobService backgroundJobService)
    {
        _unitOfWork = unitOfWork;
        _userContext = userContext;
        _currencySettings = currencySettings;
        _backgroundJobService = backgroundJobService;
    }
    
    public async Task<IEnumerable<AdDto>> GetAllAdsAsync()
    {
        return await _unitOfWork.Ads.GetAllAdsAsync() ?? new List<AdDto>();
    }

    public async Task<AdDto> CreateAdAsync(CreateAdCommand ad)
    {
        var missingInfo = string.IsNullOrWhiteSpace(ad.Description) ||
                           string.IsNullOrWhiteSpace(ad.Location) ||
                           string.IsNullOrWhiteSpace(ad.BrandModel) ||
                           string.IsNullOrWhiteSpace(ad.Motor) ||
                           ad.Price <= 0 ||
                           string.IsNullOrWhiteSpace(ad.PhoneNumber) ||
                           string.IsNullOrWhiteSpace(ad.UserName);
        
        if (missingInfo) { throw new DataMissingException("Some info is missing"); }
        
        var userId = await _userContext.GetCurrentUserIdAsync();

        var entity = Ad.Create(ad.UserName, ad.BrandModel, ad.Motor, ad.PhoneNumber,
            ad.Email, ad.Description, ad.Location, ad.Price, userId, _currencySettings);
        
        await _unitOfWork.Ads.AddAsync(entity);

        var createdAd = entity.DtoFromEntity();
        
        var result = await _unitOfWork.CommitAsync();
        if (result <= 0) { throw new AdCreationFailedException("Failed to create ad."); }
        
        _backgroundJobService.EnqueueAdAnalysisAsync(createdAd);

        return createdAd;
    }

    public async Task<AdDto> GetAdAsync(Guid id)
    {
        var ad = await _unitOfWork.Ads.GetByIdAsync(id) ?? throw new AdNotFoundException("Ad not found.");
        return ad.DtoFromEntity();
    }

    public async Task<AdDto> EditAdAsync(Guid adId, EditAdCommand ad)
    {
        var currentUserId = await _userContext.GetCurrentUserIdAsync();
        
        var existing = await _unitOfWork.Ads.GetByIdAsync(adId);
        if (existing == null) { throw new AdNotFoundException("Ad not found."); }
        if (existing.UserId != currentUserId) { throw new UnauthorizedAccessException($"Not authorized to edit ad with id: {adId}"); }
        
        existing.Description = string.IsNullOrWhiteSpace(ad.Description) ? existing.Description : ad.Description.Trim();
        existing.Location = string.IsNullOrWhiteSpace(ad.Location) ? existing.Location : ad.Location.Trim();
        existing.BrandModel = string.IsNullOrWhiteSpace(ad.BrandModel) ? existing.BrandModel : ad.BrandModel.Trim();
        existing.Motor = string.IsNullOrWhiteSpace(ad.Motor) ? existing.Motor : ad.Motor.Trim();
        existing.PhoneNumber = string.IsNullOrWhiteSpace(ad.PhoneNumber) ? existing.PhoneNumber : ad.PhoneNumber.Trim();
        existing.UserName = string.IsNullOrWhiteSpace(ad.UserName) ? existing.UserName : ad.UserName.Trim();
        existing.Price = ad.Price > 0 ? ad.Price : throw new InvalidDataException("Price must be greater than zero.");
        existing.Email = ad.Email == null ? existing.Email : ad.Email?.Trim();
        
        _unitOfWork.Ads.Update(existing);
        
        var result = await _unitOfWork.CommitAsync();

        if (result <= 0) throw new AdEditFailedException("Failed to edit ad.");
        
        var createdAd = existing.DtoFromEntity();
            
        _backgroundJobService.EnqueueAdAnalysisAsync(createdAd);
            
        return createdAd;
    }

    public async Task DeleteAdAsync(Guid adId)
    {
        var currentUserId = await _userContext.GetCurrentUserIdAsync();
        
        var existing = await _unitOfWork.Ads.GetByIdAsync(adId);
        if (existing == null) { throw new AdNotFoundException("Ad not found."); }
        if (existing.UserId != currentUserId) { throw new UnauthorizedAccessException($"Not authorized to delete ad with id: {adId}"); }
        
        _unitOfWork.Ads.Remove(existing);
        
        var result = await _unitOfWork.CommitAsync();
        if (result <= 0) { throw new AdDeleteFailedException("Failed to delete ad."); }
    }
}