using SimilaritySearch.Application.DTOs;

namespace SimilaritySearch.Application.Abstractions;

public interface IAdService
{
    public Task<IEnumerable<AdDto>> GetAllAdsAsync();
    public Task<AdDto> CreateAdAsync(CreateAdCommand ad);
    public Task<AdDto> GetAdAsync(Guid id);
    public Task<AdDto> EditAdAsync(Guid adId, EditAdCommand data);
    public Task DeleteAdAsync(Guid adId);
}